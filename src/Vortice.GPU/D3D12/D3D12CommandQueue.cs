// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Win32;
using Win32.Graphics.Direct3D12;
using Win32.Security;
using static Vortice.GPU.D3D12.D3D12Utils;
using static Win32.Apis;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12CommandQueue : CommandQueue
{
    private readonly ComPtr<ID3D12CommandQueue> _handle;
    private readonly ID3D12Fence* _fence;
    private ulong _nextFenceValue;
    private ulong _lastCompletedFenceValue;
    private readonly D3D12CommandAllocatorPool _allocatorPool;
    private readonly object _fenceMutex = new();
    private readonly object _eventMutex = new();

    private readonly object _cmdBuffersLock = new();
    private readonly List<D3D12CommandBuffer> _cmdBufferPool = new();
    private readonly Queue<D3D12CommandBuffer> _availableCmdBuffers = new();

    public D3D12CommandQueue(D3D12Device device, CommandQueueType type)
        : base(device, type)
    {
        var d3dListType = ToD3D12(type);
        _allocatorPool = new(device.Handle, d3dListType);
        _nextFenceValue = (ulong)d3dListType << 56 | 1;
        _lastCompletedFenceValue = (ulong)d3dListType << 56;

        CommandQueueDescription desc = new()
        {
            Type = d3dListType,
            Priority = (int)CommandQueuePriority.Normal,
            Flags = CommandQueueFlags.None,
            NodeMask = 0
        };

        device.Handle->CreateCommandQueue(
            &desc,
            __uuidof<ID3D12CommandQueue>(),
            _handle.GetVoidAddressOf())
            .ThrowIfFailed();

        _fence = CreateFence();

        switch (type)
        {
            case CommandQueueType.Compute:
                _label = "Compute CommandQueue";
                break;

            case CommandQueueType.Copy:
                _label = "Copy CommandQueue";
                break;

            default:
                _label = "Graphics CommandQueue";
                break;
        }

        fixed (char* labelPtr = _label)
        {
            _handle.Get()->SetName((ushort*)labelPtr);
        }

        ID3D12Fence* CreateFence()
        {
            ID3D12Fence* fence;
            device.Handle->CreateFence(0, FenceFlags.None, __uuidof<ID3D12Fence>(), (void**)&fence).ThrowIfFailed();
            fence->Signal(_lastCompletedFenceValue);
            return fence;
        }
    }

    public ID3D12CommandQueue* NativeHandle => _handle;

    // <summary>
    /// Finalizes an instance of the <see cref="D3D12Buffer" /> class.
    /// </summary>
    ~D3D12CommandQueue() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            for (int i = 0; i < _cmdBufferPool.Count; i++)
            {
                _cmdBufferPool[i].Dispose();
            }
            _cmdBufferPool.Clear();
            _allocatorPool.Dispose();

            _fence->Release();
            _handle.Dispose();
        }
    }


    /// <inheritdoc />
    public override void WaitIdle()
    {
        WaitForFence(Signal());
    }

    /// <inheritdoc />
    public override CommandBuffer BeginCommandBuffer()
    {
        lock (_cmdBuffersLock)
        {
            D3D12CommandBuffer result;
            if (_availableCmdBuffers.Count == 0)
            {
                result = new D3D12CommandBuffer(this);
                _cmdBufferPool.Add(result);
            }
            else
            {
                result = _availableCmdBuffers.Dequeue();
            }

            var allocator = RequestAllocator();
            result.Reset(allocator);

            return result;
        }
    }

    public ulong Commit(D3D12CommandBuffer commandBuffer)
    {
        lock (_fenceMutex)
        {
            var d3d12CommandList = commandBuffer.D3D12CommandList;

            d3d12CommandList->Close();

            // Kickoff the command list
            _handle.Get()->ExecuteCommandLists(1u, (ID3D12CommandList**)&d3d12CommandList);
            _availableCmdBuffers.Enqueue(commandBuffer);

            // Signal the next fence value (with the GPU)
            _handle.Get()->Signal(_fence, _nextFenceValue);

            // And increment the fence value.  
            return _nextFenceValue++;
        }
    }

    public void WaitForFence(ulong fenceValue)
    {
        if (IsFenceComplete(fenceValue))
            return;

        // TODO:  Think about how this might affect a multi-threaded situation.  Suppose thread A
        // wants to wait for fence 100, then thread B comes along and wants to wait for 99.  If
        // the fence can only have one event set on completion, then thread B has to wait for 
        // 100 before it knows 99 is ready.  Maybe insert sequential events?
        lock (_eventMutex)
        {
            _fence->SetEventOnCompletion(fenceValue, Handle.Null);
            _lastCompletedFenceValue = fenceValue;
        }
    }

    public ulong Signal()
    {
        lock (_fenceMutex)
        {
            _handle.Get()->Signal(_fence, _nextFenceValue);
            return _nextFenceValue++;
        }
    }

    public bool IsFenceComplete(ulong fenceValue)
    {
        // Avoid querying the fence value by testing against the last one seen.
        // The max() is to protect against an unlikely race condition that could cause the last
        // completed fence value to regress.
        if (fenceValue > _lastCompletedFenceValue)
        {
            _lastCompletedFenceValue = Math.Max(_lastCompletedFenceValue, _fence->GetCompletedValue());
        }

        return fenceValue <= _lastCompletedFenceValue;
    }

    public void StallForFence(ulong fenceValue)
    {
        //CommandQueue & Producer = Graphics::g_CommandManager.GetQueue((D3D12_COMMAND_LIST_TYPE)(FenceValue >> 56));
        //_handle.Get()->Wait(Producer.m_pFence, fenceValue);
    }

    public void StallForProducer(D3D12CommandQueue producer)
    {
        Debug.Assert(producer._nextFenceValue > 0);

        _handle.Get()->Wait(producer._fence, producer._nextFenceValue - 1);
    }


    private ID3D12CommandAllocator* RequestAllocator()
    {
        ulong completedFence = _fence->GetCompletedValue();

        return _allocatorPool.RequestAllocator(completedFence);
    }

    public void DiscardAllocator(ulong fenceValue, ID3D12CommandAllocator* Allocator)
    {
        _allocatorPool.DiscardAllocator(fenceValue, Allocator);
    }

    private const int EVENT_MODIFY_STATE = 0x0002;
    private const int SYNCHRONIZE = (0x00100000);

    [DllImport("kernel32", ExactSpelling = true)]
    private static extern Bool32 CloseHandle(Handle hObject);

    [DllImport("kernel32", ExactSpelling = true, SetLastError = true)]
    private static extern Handle CreateEventExW(SECURITY_ATTRIBUTES* lpEventAttributes, ushort* lpName, uint dwFlags, uint dwDesiredAccess);
}

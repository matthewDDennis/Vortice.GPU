// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System;
using System.Drawing;
using Vortice.Vulkan;
using Win32;
using Win32.Graphics.Direct3D12;
using static Vortice.GPU.D3D12.D3D12Utils;
using static Win32.Apis;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12CommandBuffer : CommandBuffer, IDisposable
{
    private D3D12CommandQueue _queue;
    private ID3D12CommandAllocator* _currentAllocator;
    private readonly ID3D12GraphicsCommandList4* _commandList;
    private readonly List<D3D12SwapChain> _presentSwapChains = new();

    public D3D12CommandBuffer(D3D12CommandQueue queue)
    {
        _queue = queue;
        var d3dListType = ToD3D12(queue.QueueType);

        ID3D12GraphicsCommandList4* commandList = default;
        HResult hr = ((D3D12Device)queue.Device).Handle->CreateCommandList1(
           0,
           d3dListType,
           CommandListFlags.None,
           __uuidof<ID3D12GraphicsCommandList4>(),
           (void**)&commandList
           );
        hr.ThrowIfFailed();

        _commandList = commandList;
    }

    public override CommandQueue Queue => _queue;

    public ID3D12CommandAllocator* CurrentAllocator => _currentAllocator;
    public ID3D12GraphicsCommandList4* D3D12CommandList => _commandList;

    public void Dispose()
    {
        _commandList->Release();
    }

    public void Reset(ID3D12CommandAllocator* allocator)
    {
        _currentAllocator = allocator;
        _commandList->Reset(allocator, null);
        ResetState();
    }

    public override void Commit()
    {
        // Present acquired SwapChains
        for (int i = 0, count = _presentSwapChains.Count; i < count; ++i)
        {
            D3D12SwapChain swapChain = _presentSwapChains[i];

            // Transition SwapChain textures to present 
            D3D12Texture texture = swapChain.CurrentBackbuffer;

           // _commandList->EndRenderPass();

            ResourceBarrier barrier = ResourceBarrier.InitTransition(texture.Handle, ResourceStates.RenderTarget, ResourceStates.Present);
            _commandList->ResourceBarrier(1, &barrier);
        }

        ulong fenceValue = _queue.Commit(this);
        _queue.DiscardAllocator(fenceValue, _currentAllocator);
        _currentAllocator = null;

        // Now present SwapChains
        HResult hr = HResult.Ok;
        for (int i = 0, count = _presentSwapChains.Count; i < count && hr.Success; ++i)
        {
            D3D12SwapChain swapChain = _presentSwapChains[i];
            hr = swapChain.Present();
        }

        _presentSwapChains.Clear();
    }

    protected override Texture? AcquireSwapchainTextureCore(SwapChain swapChain)
    {
        D3D12SwapChain d3d12SwapChain = (D3D12SwapChain)swapChain;

        // Check for window size changes and resize the swapchain if needed. 
        Size swapChainSize = d3d12SwapChain.Size;
        Size surfaceSize = swapChain.Surface.Size;

        if (swapChainSize != surfaceSize)
        {
            _queue.WaitIdle();

            if (!d3d12SwapChain.Resize())
            {
                return null;
            }
        }

        D3D12Texture swapChainTexture = d3d12SwapChain.CurrentBackbuffer;

        // Transition to RenderTarget state
        ResourceBarrier barrier = ResourceBarrier.InitTransition(swapChainTexture.Handle, ResourceStates.Present, ResourceStates.RenderTarget);
        _commandList->ResourceBarrier(1, &barrier);

        //RenderPassRenderTargetDescription RTV = new();
        //RTV.cpuDescriptor = swapChainTexture.RTV;
        //RTV.BeginningAccess.Type = RenderPassBeginningAccessType.Clear;
        //RTV.BeginningAccess.Clear.ClearValue.Color[0] = 1.0f;
        //RTV.BeginningAccess.Clear.ClearValue.Color[1] = 0.0f;
        //RTV.BeginningAccess.Clear.ClearValue.Color[2] = 0.0f;
        //RTV.BeginningAccess.Clear.ClearValue.Color[3] = 1.0f;
        //RTV.EndingAccess.Type = RenderPassEndingAccessType.Preserve;
        //_commandList->BeginRenderPass(1, &RTV, null, RenderPassFlags.AllowUavWrites);

        _presentSwapChains.Add(d3d12SwapChain);
        return swapChainTexture;
    }

    protected override void BeginRenderPassCore(in RenderPassColorAttachment colorAttachment)
    {
        D3D12Texture d3dTexture = (D3D12Texture)colorAttachment.Texture;
        var clearColor = colorAttachment.ClearColor;

        RenderPassRenderTargetDescription RTV = new();
        RTV.cpuDescriptor = d3dTexture.RTV;
        RTV.BeginningAccess.Type = RenderPassBeginningAccessType.Clear;
        RTV.BeginningAccess.Clear.ClearValue.Color[0] = clearColor.R / 255.0f;
        RTV.BeginningAccess.Clear.ClearValue.Color[1] = clearColor.G / 255.0f;
        RTV.BeginningAccess.Clear.ClearValue.Color[2] = clearColor.B / 255.0f;
        RTV.BeginningAccess.Clear.ClearValue.Color[3] = clearColor.A / 255.0f;
        RTV.EndingAccess.Type = RenderPassEndingAccessType.Preserve;
        _commandList->BeginRenderPass(1, &RTV, null, RenderPassFlags.AllowUavWrites);
    }

    protected override void EndRenderPassCore()
    {
        _commandList->EndRenderPass();
    }
}

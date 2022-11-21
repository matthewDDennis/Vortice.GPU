// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Vortice.Vulkan;
using Win32;
using Win32.Graphics.Direct3D12;
using Win32.Security;
using static Vortice.GPU.D3D12.D3D12Utils;
using static Win32.Apis;

namespace Vortice.GPU.D3D12;

internal unsafe readonly struct D3D12DescriptorAllocator
{
    public readonly ID3D12Device5* Device;
    public readonly uint DescriptorSize;
    private readonly object _locker = new();
    private readonly DescriptorHeapType _type;
    private readonly uint _numDescriptorsPerBlock;
    private readonly List<ComPtr<ID3D12DescriptorHeap>> _heaps = new();
    private readonly List<CpuDescriptorHandle> _freeList = new();

    public D3D12DescriptorAllocator(ID3D12Device5* device, DescriptorHeapType type, uint numDescriptorsPerBlock)
    {
        Device = device;
        DescriptorSize = device->GetDescriptorHandleIncrementSize(type);

        _type = type;
        _numDescriptorsPerBlock = numDescriptorsPerBlock;
    }

    public void Shutdown()
    {
        for(int i = 0; i < _heaps.Count; i++)
        {
            _heaps[i].Dispose();
        }

        _heaps.Clear();
    }

    public CpuDescriptorHandle Allocate()
    {
        lock (_locker)
        {
            if (_freeList.Count == 0)
            {
                //heaps.emplace_back();
                DescriptorHeapDescription desc = new()
                {
                    Type = _type,
                    NumDescriptors = _numDescriptorsPerBlock
                };

                ComPtr<ID3D12DescriptorHeap> heap = default;
                Device->CreateDescriptorHeap(&desc,
                    __uuidof<ID3D12DescriptorHeap>(),
                    heap.GetVoidAddressOf()
                    ).ThrowIfFailed();
                _heaps.Add(heap);

                CpuDescriptorHandle heapStart = heap.Get()->GetCPUDescriptorHandleForHeapStart();
                for (uint i = 0; i < desc.NumDescriptors; ++i)
                {
                    CpuDescriptorHandle handle = heapStart;
                    handle.ptr += i * DescriptorSize;
                    _freeList.Add(handle);
                }
            }

            Debug.Assert(_freeList.Count > 0);

            CpuDescriptorHandle result = _freeList[_freeList.Count - 1];
            _freeList.RemoveAt(_freeList.Count - 1);
            return result;
        }
    }

    public void Free(in CpuDescriptorHandle index)
    {
        lock (_locker)
        {
            _freeList.Add(index);
        }
    }
}

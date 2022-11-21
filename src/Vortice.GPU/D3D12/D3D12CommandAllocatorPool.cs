// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System;
using Win32;
using Win32.Graphics.Direct3D12;
using static Win32.Apis;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12CommandAllocatorPool : IDisposable
{
    private readonly ID3D12Device5* _device;
    private readonly CommandListType _type;
    private readonly object _allocatorMutex = new();

    private readonly List<D3D12CommandAllocatorBundle> _allocators = new();
    private readonly Queue<Tuple<ulong, D3D12CommandAllocatorBundle>> _readyAllocators = new();

    public D3D12CommandAllocatorPool(ID3D12Device5* device, CommandListType type)
    {
        _device = device;
        _type = type;
    }

    public void Dispose()
    {
        for (int i = 0; i < _allocators.Count; ++i)
        {
            _allocators[i].Allocator->Release();
        }

        _allocators.Clear();
    }

    public ID3D12CommandAllocator* RequestAllocator(ulong completedFenceValue)
    {
        lock (_allocatorMutex)
        {
            if (_readyAllocators.Count > 0)
            {
                Tuple<ulong, D3D12CommandAllocatorBundle> pair = _readyAllocators.First();

                if (pair.Item1 <= completedFenceValue)
                {
                    D3D12CommandAllocatorBundle bundle = pair.Item2;
                    bundle.Allocator->Reset().ThrowIfFailed();
                    _readyAllocators.Dequeue();
                    return bundle.Allocator;
                }
            }

            // If no allocator's were ready to be reused, create a new one
            ID3D12CommandAllocator* newAllocator = default;

            HResult hr = _device->CreateCommandAllocator(
                _type,
                __uuidof<ID3D12CommandAllocator>(),
                (void**)&newAllocator
                );
            hr.ThrowIfFailed();

            string allocatorName = $"CommandAllocator {_allocators.Count}";
            ((ID3D12Object*)newAllocator)->SetName(allocatorName);
            _allocators.Add(new D3D12CommandAllocatorBundle(newAllocator));

            return newAllocator;
        }
    }

    public void DiscardAllocator(ulong fenceValue, ID3D12CommandAllocator* Allocator)
    {
        lock (_allocatorMutex)
        {
            // That fence value indicates we are free to reset the allocator
            _readyAllocators.Enqueue(Tuple.Create(fenceValue, new D3D12CommandAllocatorBundle(Allocator)));
        }
    }

    private readonly struct D3D12CommandAllocatorBundle
    {
        /// <summary>
        /// The <see cref="ID3D12CommandAllocator"/> value for the current entry.
        /// </summary>
        public readonly ID3D12CommandAllocator* Allocator;

        public D3D12CommandAllocatorBundle(ID3D12CommandAllocator* allocator)
        {
            Allocator = allocator;
        }
    }
}

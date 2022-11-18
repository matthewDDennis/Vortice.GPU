// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12Buffer : GPUBuffer
{
    private readonly ComPtr<ID3D12Resource> _handle;

    public D3D12Buffer(D3D12Device device, in BufferDescription description, void* initialData)
        : base(device, description)
    {
        //device.Handle->CreateCommittedResource();
    }

    // <summary>
    /// Finalizes an instance of the <see cref="D3D12Buffer" /> class.
    /// </summary>
    ~D3D12Buffer() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _handle.Dispose();
        }
    }

    protected override void OnLabelChanged(string newLabel)
    {
        fixed (char* labelPtr = newLabel)
        {
            _handle.Get()->SetName((ushort*)labelPtr);
        }
    }
}

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using static Win32.Apis;
using Win32.Graphics.Direct3D12;
using static Win32.Graphics.Direct3D12.Apis;
using static Vortice.GPU.D3D12.D3D12Utils;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12Buffer : GPUBuffer
{
    private readonly ComPtr<ID3D12Resource> _handle;

    public D3D12Buffer(D3D12Device device, in BufferDescription description, void* initialData)
        : base(device, description)
    {
        ulong alignedSize = description.Size;
        if ((description.Usage & BufferUsage.Constant) != BufferUsage.None)
        {
            alignedSize = Utilities.AlignUp(alignedSize, D3D12_CONSTANT_BUFFER_DATA_PLACEMENT_ALIGNMENT);
        }

        HeapProperties heapProps = DefaultHeapProps;
        ResourceFlags resourceFlags = ResourceFlags.None;

        if ((description.Usage & BufferUsage.ShaderWrite) != BufferUsage.None)
        {
            resourceFlags |= ResourceFlags.AllowUnorderedAccess;
        }

        if (!((description.Usage & BufferUsage.ShaderRead) == BufferUsage.None) &&
            !((description.Usage & BufferUsage.RayTracing) == BufferUsage.None))
        {
            resourceFlags |= ResourceFlags.DenyShaderResource;
        }

        ResourceDescription desc = ResourceDescription.Buffer(alignedSize, resourceFlags);

        HResult hr = device.Handle->CreateCommittedResource(
            &heapProps,
            HeapFlags.None,
            &desc,
            ResourceStates.Common,
            null,
            __uuidof<ID3D12Resource>(),
            _handle.GetVoidAddressOf()
            );
        hr.ThrowIfFailed();
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

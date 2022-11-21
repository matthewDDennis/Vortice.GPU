// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi.Common;
using static Vortice.GPU.D3D12.D3D12Utils;
using static Vortice.GPU.D3DUtils;
using static Win32.Apis;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12Texture : Texture
{
    public readonly ID3D12Resource* Handle;
    public readonly CpuDescriptorHandle RTV;

    public D3D12Texture(D3D12Device device, in TextureDescription description, ID3D12Resource* handle)
        : base(device, description)
    {
        Handle = handle;

        RTV = device.AllocateDescriptor(DescriptorHeapType.Rtv);
        device.Handle->CreateRenderTargetView(Handle, null, RTV);
    }

    public D3D12Texture(D3D12Device device, in TextureDescription description, void* initialData)
        : base(device, description)
    {
        Format dxgiFormat = ToDXGI(description.Format);

        bool isDepthStencil = description.Format.IsDepthStencilFormat();

        HeapProperties heapProps = DefaultHeapProps;
        ResourceFlags resourceFlags = ResourceFlags.None;
        int sampleCount = (int)description.SampleCount;

        ResourceDescription desc;

        switch (description.Dimension)
        {
            case TextureDimension.Texture1D:
                desc = ResourceDescription.Tex1D(dxgiFormat,
                    (ulong)description.Width,
                    (ushort)description.DepthOrArrayLayers,
                    (ushort)description.MipLevels,
                    resourceFlags
                    );
                break;

            case TextureDimension.Texture2D:
                desc = ResourceDescription.Tex2D(dxgiFormat,
                    (ulong)description.Width,
                    (uint)description.Height,
                    (ushort)description.DepthOrArrayLayers,
                    (ushort)description.MipLevels,
                    (uint)sampleCount,
                    0,
                    resourceFlags
                    );
                break;

            case TextureDimension.Texture3D:
                desc = ResourceDescription.Tex3D(dxgiFormat,
                    (ulong)description.Width,
                    (uint)description.Height,
                    (ushort)description.DepthOrArrayLayers,
                    (ushort)description.MipLevels,
                    resourceFlags
                    );
                break;
        }

        ID3D12Resource* handle = default;
        HResult hr = device.Handle->CreateCommittedResource(
            &heapProps,
            HeapFlags.None,
            &desc,
            ResourceStates.Common,
            null,
            __uuidof<ID3D12Resource>(),
            (void**)&handle
            );
        hr.ThrowIfFailed();
        Handle = handle;
    }

    // <summary>
    /// Finalizes an instance of the <see cref="D3D12Texture" /> class.
    /// </summary>
    ~D3D12Texture() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Handle->Release();
        }
    }

    protected override void OnLabelChanged(string newLabel)
    {
        fixed (char* labelPtr = newLabel)
        {
            Handle->SetName((ushort*)labelPtr);
        }
    }
}

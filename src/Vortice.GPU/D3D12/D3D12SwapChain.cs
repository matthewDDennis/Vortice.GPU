// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using static Win32.Apis;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12SwapChain : D3DSwapChainBase
{
    private D3D12Texture[]? _backbufferTextures;
    private int _backBufferIndex;

    public D3D12SwapChain(D3D12Device device, in ISwapChainSurface surface, in SwapChainDescription description)
        : base((IDXGIFactory2*)device.Factory, device.IsTearingSupported, (IUnknown*)device.D3D12GraphicsQueue,
            device, surface, description)
    {
    }

    // <summary>
    /// Finalizes an instance of the <see cref="D3D12SwapChain" /> class.
    /// </summary>
    ~D3D12SwapChain() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_backbufferTextures != null)
            {
                for (int i = 0; i < _backbufferTextures.Length; i++)
                {
                    _backbufferTextures[i].Dispose();
                }
            }
        }

        base.Dispose(disposing);
    }

    protected override void AfterResize()
    {
        SwapChainDescription1 swapChainDesc;
        _handle.Get()->GetDesc1(&swapChainDesc).ThrowIfFailed();

        _backBufferIndex = 0;
        _backbufferTextures = new D3D12Texture[swapChainDesc.BufferCount];

        TextureDescription textureDesc = TextureDescription.Texture2D(
            ColorFormat,
            (int)swapChainDesc.Width,
            (int)swapChainDesc.Height,
            1,
            1,
            TextureUsage.RenderTarget);

        for (uint i = 0; i < swapChainDesc.BufferCount; ++i)
        {
            ID3D12Resource* d3dHandle = default;
            _handle.Get()->GetBuffer(i, __uuidof<ID3D12Resource>(), (void**)&d3dHandle);

            _backbufferTextures[i] = new D3D12Texture(Device, textureDesc, d3dHandle)
            {
                Label = $"BackBuffer texture {i}"
            };
        }
    }

    protected override void OnLabelChanged(string newLabel)
    {
        fixed (char* labelPtr = newLabel)
        {
            // _handle.Get()->SetName((ushort*)labelPtr);
        }
    }
}

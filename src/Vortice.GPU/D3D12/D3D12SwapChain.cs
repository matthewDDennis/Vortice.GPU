// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using static Win32.Apis;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using static Win32.Graphics.Dxgi.Apis;
using System.Diagnostics;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12SwapChain : D3DSwapChainBase
{
    private readonly ComPtr<IDXGISwapChain3> _handle3;
    private D3D12Texture[] _backbufferTextures = Array.Empty<D3D12Texture>();
    //private int _backBufferIndex;

    public D3D12SwapChain(D3D12Device device, in ISwapChainSurface surface, in SwapChainDescription description)
        : base((IDXGIFactory2*)device.Factory, device.IsTearingSupported, (IUnknown*)device.D3D12GraphicsQueue,
            device, surface, description)
    {
        _handle.CopyTo(_handle3.GetAddressOf()).ThrowIfFailed();
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
            for (int i = 0; i < _backbufferTextures.Length; i++)
            {
                _backbufferTextures[i].Dispose();
            }
        }

        _handle3.Dispose();
        base.Dispose(disposing);
    }

    public uint CurrentBackbufferIndex => _handle3.Get()->GetCurrentBackBufferIndex();
    public D3D12Texture CurrentBackbuffer => _backbufferTextures[CurrentBackbufferIndex];

    protected override void AfterResize()
    {
        //_backBufferIndex = 0;
        _backbufferTextures = new D3D12Texture[BackBufferCount];

        TextureDescription textureDesc = TextureDescription.Texture2D(
            ColorFormat,
            Size.Width,
            Size.Height,
            1,
            1,
            TextureUsage.RenderTarget);

        for (uint i = 0; i < BackBufferCount; ++i)
        {
            ID3D12Resource* d3dHandle = default;
            _handle.Get()->GetBuffer(i, __uuidof<ID3D12Resource>(), (void**)&d3dHandle);

            _backbufferTextures[i] = new D3D12Texture((D3D12Device)Device, textureDesc, d3dHandle)
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

    public bool Resize()
    {
        for (int i = 0; i < BackBufferCount; i++)
        {
            _backbufferTextures[i].Dispose();
        }

        HResult hr = _handle.Get()->ResizeBuffers(
            (uint)BackBufferCount,
            (uint)Surface.Size.Width,
            (uint)Surface.Size.Height,
            Win32.Graphics.Dxgi.Common.Format.Unknown, /* Keep the old format */
            _tearingSupported ? SwapChainFlags.AllowTearing : SwapChainFlags.None
        );

        if (hr == DXGI_ERROR_DEVICE_REMOVED || hr == DXGI_ERROR_DEVICE_RESET)
        {
#if DEBUG
            hr = (hr == DXGI_ERROR_DEVICE_REMOVED) ? ((D3D12Device)Device).Handle->GetDeviceRemovedReason() : hr;
            Debug.WriteLine($"Device Lost on ResizeBuffers: Reason code {hr}");
#endif
            // If the device was removed for any reason, a new device and swap chain will need to be created.
            ((D3D12Device)Device).HandleDeviceLost();

            // Everything is set up now. Do not continue execution of this method. HandleDeviceLost will reenter this method
            // and correctly set up the new device.
            return false;
        }
        else
        {
            hr.ThrowIfFailed();

            SwapChainDescription1 swapChainDesc;
            _handle.Get()->GetDesc1(&swapChainDesc).ThrowIfFailed();
            Size = new((int)swapChainDesc.Width, (int)swapChainDesc.Height);

            AfterResize();

            return true;
        }
    }
}

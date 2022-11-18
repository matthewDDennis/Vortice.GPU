// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Dxgi;
using Win32.Graphics.Dxgi.Common;
using static Vortice.GPU.D3DUtils;

namespace Vortice.GPU;

internal abstract unsafe class D3DSwapChainBase : SwapChain
{
    protected readonly ComPtr<IDXGISwapChain1> _handle;
    protected readonly bool _tearingSupported;
    protected readonly uint _syncInterval;

    protected D3DSwapChainBase(
        IDXGIFactory2* factory,
        bool tearingSupported,
        IUnknown* deviceOrCommandQueue,
        GPUDevice device, in ISwapChainSurface surface, in SwapChainDescription description)
        : base(device, surface, description)
    {
        _tearingSupported = tearingSupported;
        _syncInterval = PresentModeToSyncInterval(description.PresentMode);

        SwapChainDescription1 swapChainDesc = new()
        {
            Width = (uint)surface.Size.Width,
            Height = (uint)surface.Size.Height,
            Format = ToDXGISwapChainFormat(description.ColorFormat),
            Stereo = false,
            SampleDesc = new(1, 0),
            BufferUsage = Usage.RenderTargetOutput,
            BufferCount = PresentModeToBufferCount(description.PresentMode),
            Scaling = Scaling.Stretch,
            SwapEffect = SwapEffect.FlipDiscard,
            AlphaMode = AlphaMode.Ignore,
            Flags = tearingSupported ? SwapChainFlags.AllowTearing : SwapChainFlags.None
        };

        switch (surface.Kind)
        {
            case SwapChainSurfaceKind.Win32:
                SwapChainFullscreenDescription fsSwapChainDesc = new()
                {
                    Windowed = !surface.IsFullscreen
                };

                factory->CreateSwapChainForHwnd(
                    deviceOrCommandQueue,
                    surface.SurfaceHandle,
                    &swapChainDesc,
                    &fsSwapChainDesc,
                    null,
                    _handle.GetAddressOf()
                    ).ThrowIfFailed();

                // This class does not support exclusive full-screen mode and prevents DXGI from responding to the ALT+ENTER shortcut
                factory->MakeWindowAssociation(surface.SurfaceHandle, WindowAssociationFlags.NoAltEnter).ThrowIfFailed();
                break;
        }

        AfterResize();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _handle.Dispose();
        }
    }

    protected abstract void AfterResize();

    public void Present()
    {
        PresentFlags presentFlags = PresentFlags.None;
        if (_syncInterval == 0 &&
            !Surface.IsFullscreen
            && _tearingSupported)
        {
            presentFlags = PresentFlags.AllowTearing;
        }

        HResult hr = _handle.Get()->Present(_syncInterval, presentFlags);
    }
}

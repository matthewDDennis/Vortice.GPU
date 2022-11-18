// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D;
using Win32.Graphics.Direct3D12;
using Dxgi = Win32.Graphics.Dxgi;
using static Win32.Apis;
using static Win32.Graphics.Dxgi.Apis;
using static Win32.Graphics.Direct3D12.Apis;
using CommunityToolkit.Diagnostics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DxgiInfoQueueFilter = Win32.Graphics.Dxgi.InfoQueueFilter;
using static Vortice.GPU.D3DUtils;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12Device : GPUDevice
{
    private static readonly FeatureLevel s_minFeatureLevel = FeatureLevel.Level_11_0;
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);
    public static bool IsSupported() => s_isSupported.Value;

    private readonly ComPtr<Dxgi.IDXGIFactory4> _dxgiFactory;
    private readonly ComPtr<ID3D12Device5> _d3dDevice;
    private readonly D3D12CommandQueue[] _commandQueues = new D3D12CommandQueue[(int)CommandQueueType.Count];

    public D3D12Device(ValidationMode validationMode, GPUPowerPreference powerPreference)
        : base(GPUBackendType.D3D12)
    {
        Guard.IsTrue(IsSupported(), nameof(D3D12Device), "Direct3D12 is not supported");

        uint dxgiDebugFlags = 0;

        if (validationMode != ValidationMode.Disabled)
        {
            using ComPtr<ID3D12Debug1> d3d12Debug1 = default;
            if (D3D12GetDebugInterface(__uuidof<ID3D12Debug1>(), d3d12Debug1.GetVoidAddressOf()).Success)
            {
                d3d12Debug1.Get()->EnableDebugLayer();

                if (validationMode == ValidationMode.GPU)
                {
                    d3d12Debug1.Get()->SetEnableGPUBasedValidation(true);
                    d3d12Debug1.Get()->SetEnableSynchronizedCommandQueueValidation(true);

                    using ComPtr<ID3D12Debug2> d3d12Debug2 = default;
                    if (D3D12GetDebugInterface(__uuidof<ID3D12Debug2>(), d3d12Debug2.GetVoidAddressOf()).Success)
                    {
                        d3d12Debug2.Get()->SetGPUBasedValidationFlags(GpuBasedValidationFlags.None);
                    }
                }
            }
            else
            {
                Debug.WriteLine("WARNING: Direct3D Debug Device is not available\n");
            }

#if DEBUG
            using ComPtr<Dxgi.IDXGIInfoQueue> dxgiInfoQueue = default;
            if (DXGIGetDebugInterface1(0, __uuidof<Dxgi.IDXGIInfoQueue>(), dxgiInfoQueue.GetVoidAddressOf()).Success)
            {
                dxgiDebugFlags = DXGI_CREATE_FACTORY_DEBUG;

                dxgiInfoQueue.Get()->SetBreakOnSeverity(DXGI_DEBUG_ALL, Dxgi.InfoQueueMessageSeverity.Error, true);
                dxgiInfoQueue.Get()->SetBreakOnSeverity(DXGI_DEBUG_ALL, Dxgi.InfoQueueMessageSeverity.Corruption, true);

                int* hide = stackalloc int[1]
                {
                    80 /* IDXGISwapChain::GetContainingOutput: The swapchain's adapter does not control the output on which the swapchain's window resides. */,
                };

                DxgiInfoQueueFilter filter = new()
                {
                    DenyList = new()
                    {
                        NumIDs = 1u,
                        pIDList = hide
                    }
                };
                dxgiInfoQueue.Get()->AddStorageFilterEntries(DXGI_DEBUG_DXGI, &filter);
            }
#endif
        }

        CreateDXGIFactory2(dxgiDebugFlags, __uuidof<Dxgi.IDXGIFactory4>(), _dxgiFactory.GetVoidAddressOf()).ThrowIfFailed();

        // Determines whether tearing support is available for fullscreen borderless windows.
        HResult hr = HResult.Ok;
        {
            Bool32 allowTearing = false;

            using ComPtr<Dxgi.IDXGIFactory5> dxgiFactory5 = default;
            hr = _dxgiFactory.CopyTo(dxgiFactory5.GetAddressOf());
            if (hr.Success)
            {
                hr = dxgiFactory5.Get()->CheckFeatureSupport(Win32.Graphics.Dxgi.Feature.PresentAllowTearing, &allowTearing, sizeof(Bool32));
            }

            if (hr.Failure || !allowTearing)
            {
                IsTearingSupported = false;
                Debug.WriteLine("Direct3D11: Variable refresh rate displays not supported");
            }
            else
            {
                IsTearingSupported = true;
            }
        }

        using ComPtr<Dxgi.IDXGIAdapter1> adapter = default;
        GetAdapter(adapter.GetAddressOf());

        // Create the DX12 API device object.
        hr = D3D12CreateDevice(
           (IUnknown*)adapter.Get(),
           s_minFeatureLevel,
           __uuidof<ID3D12Device5>(),
           _d3dDevice.GetVoidAddressOf()
           );
        hr.ThrowIfFailed();

        for (int i = 0; i < (int)CommandQueueType.Count; i++)
        {
            _commandQueues[i] = new D3D12CommandQueue(this, (CommandQueueType)i);
        }

        Dxgi.AdapterDescription1 adapterDesc;
        adapter.Get()->GetDesc1(&adapterDesc).ThrowIfFailed();

        AdapterProperties = new AdapterProperties()
        {
            VendorId = adapterDesc.VendorId,
            DeviceId = adapterDesc.DeviceId,
            Name = new string((char*)adapterDesc.Description),
            DriverDescription = string.Empty,
            AdapterType = AdapterType.Other
        };
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12Device" /> class.
    /// </summary>
    ~D3D12Device() => Dispose(isDisposing: false);

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            for (int i = 0; i < (int)CommandQueueType.Count; i++)
            {
                _commandQueues[i].Dispose();
            }

#if DEBUG
            uint refCount = _d3dDevice.Get()->Release();
            if (refCount > 0)
            {
                Debug.WriteLine($"Direct3D12: There are {refCount} unreleased references left on the device");

                ID3D12DebugDevice* debugDevice = default;
                if (_d3dDevice.Get()->QueryInterface(__uuidof<ID3D12DebugDevice>(), (void**)&debugDevice).Success)
                {
                    debugDevice->ReportLiveDeviceObjects(ReportLiveDeviceObjectFlags.Detail | ReportLiveDeviceObjectFlags.IgnoreInternal);
                    debugDevice->Release();
                }
            }
#else
            _d3dDevice.Dispose();
#endif

            _dxgiFactory.Dispose();

#if DEBUG
            using ComPtr<Dxgi.IDXGIDebug1> dxgiDebug = default;
            if (DXGIGetDebugInterface1(0, __uuidof<Dxgi.IDXGIDebug1>(), dxgiDebug.GetVoidAddressOf()).Success)
            {
                dxgiDebug.Get()->ReportLiveObjects(DXGI_DEBUG_ALL, Dxgi.ReportLiveObjectFlags.Summary | Dxgi.ReportLiveObjectFlags.Detail | Dxgi.ReportLiveObjectFlags.IgnoreInternal);
            }
#endif
        }
    }

    public Dxgi.IDXGIFactory4* Factory => _dxgiFactory;
    public bool IsTearingSupported { get; }
    public ID3D12Device5* Handle => _d3dDevice;

    /// <inheritdoc />
    public override AdapterProperties AdapterProperties { get; }

    /// <inheritdoc />
    public override GPUDeviceLimits Limits { get; }

    /// <inheritdoc />
    public override CommandQueue GraphicsQueue => _commandQueues[(int)CommandQueueType.Graphics];

    public ID3D12CommandQueue* D3D12GraphicsQueue => _commandQueues[(int)CommandQueueType.Graphics].Handle;

    /// <inheritdoc />
    public override void WaitIdle()
    {
    }

    /// <inheritdoc />
    public override PixelFormatSupport QueryPixelFormatSupport(PixelFormat format)
    {
        FeatureDataFormatSupport featureData = new()
        {
            Format = ToDXGI(format)
        };

        _d3dDevice.Get()->CheckFeatureSupport(Feature.FormatSupport, &featureData, sizeof(FeatureDataFormatSupport));

        PixelFormatSupport result = PixelFormatSupport.None;
        if ((featureData.Support1 & FormatSupport1.ShaderLoad) != FormatSupport1.None)
        {
            result |= PixelFormatSupport.ShaderLoad;
        }

        if ((featureData.Support1 & FormatSupport1.ShaderSample) != FormatSupport1.None)
        {
            result |= PixelFormatSupport.ShaderSample;
        }

        if ((featureData.Support1 & FormatSupport1.RenderTarget) != FormatSupport1.None)
        {
            result |= PixelFormatSupport.RenderTarget;
        }

        if ((featureData.Support1 & FormatSupport1.DepthStencil) != FormatSupport1.None)
        {
            result |= PixelFormatSupport.DepthStencil;
        }

        if ((featureData.Support1 & FormatSupport1.Blendable) != FormatSupport1.None)
        {
            result |= PixelFormatSupport.Blendable;
        }

        return result;
    }

    protected override GPUBuffer CreateBufferCore(in BufferDescription description, void* initialData)
    {
        return new D3D12Buffer(this, description, initialData);
    }

    protected override Texture CreateTextureCore(in TextureDescription description, void* initialData)
    {
        return new D3D12Texture(this, description, initialData);
    }

    protected override SwapChain CreateSwapChainCore(in ISwapChainSurface surface, in SwapChainDescription description)
    {
        return new D3D12SwapChain(this, surface, description);
    }

    private void GetAdapter(Dxgi.IDXGIAdapter1** ppAdapter)
    {
        using ComPtr<Dxgi.IDXGIAdapter1> adapter = default;

        using ComPtr<Dxgi.IDXGIFactory6> factory6 = default;
        HResult hr = _dxgiFactory.CopyTo(factory6.GetAddressOf());
        if (hr.Success)
        {
            for (uint adapterIndex = 0;
                factory6.Get()->EnumAdapterByGpuPreference(
                    adapterIndex,
                    ToDXGI(GPUPowerPreference.HighPerformance),
                    __uuidof<Dxgi.IDXGIAdapter1>(),
                     (void**)adapter.ReleaseAndGetAddressOf()).Success;
                adapterIndex++)
            {
                Dxgi.AdapterDescription1 desc;
                adapter.Get()->GetDesc1(&desc).ThrowIfFailed();

                if ((desc.Flags & Dxgi.AdapterFlags.Software) != Dxgi.AdapterFlags.None)
                {
                    // Don't select the Basic Render Driver adapter.
                    continue;
                }

                // Check to see if the adapter supports Direct3D 12, but don't create the actual device yet.
                if (D3D12CreateDevice((IUnknown*)adapter.Get(), s_minFeatureLevel, __uuidof<ID3D12Device>(), null).Success)
                {
#if DEBUG
                    //Debug.WriteLine($"Direct3D Adapter ({adapterIndex}): VID:%04X, PID:%04X - %ls\n", adapterIndex, desc.VendorId, desc.DeviceId, desc.Description);
#endif
                    break;
                }
            }
        }

        if (adapter.Get() is null)
        {
            for (uint adapterIndex = 0;
                _dxgiFactory.Get()->EnumAdapters1(
                    adapterIndex,
                    adapter.ReleaseAndGetAddressOf()).Success;
                ++adapterIndex)
            {
                Dxgi.AdapterDescription1 desc;
                adapter.Get()->GetDesc1(&desc).ThrowIfFailed();

                if ((desc.Flags & Dxgi.AdapterFlags.Software) != Dxgi.AdapterFlags.None)
                {
                    // Don't select the Basic Render Driver adapter.
                    continue;
                }

                // Check to see if the adapter supports Direct3D 12, but don't create the actual device yet.
                if (D3D12CreateDevice((IUnknown*)adapter.Get(), s_minFeatureLevel, __uuidof<ID3D12Device>(), null).Success)
                {
#if DEBUG
                    //wchar_t buff[256] = { };
                    //swprintf_s(buff, L"Direct3D Adapter (%u): VID:%04X, PID:%04X - %ls\n", adapterIndex, desc.VendorId, desc.DeviceId, desc.Description);
                    //OutputDebugStringW(buff);
#endif
                    break;
                }
            }
        }

#if DEBUG
        if (adapter.Get() is null)
        {
            // Try WARP12 instead
            if (_dxgiFactory.Get()->EnumWarpAdapter(__uuidof<Dxgi.IDXGIAdapter1>(), (void**)adapter.ReleaseAndGetAddressOf()).Failure)
            {
                throw new GPUException("WARP12 not available. Enable the 'Graphics Tools' optional feature");
            }

            Debug.WriteLine("Direct3D Adapter - WARP12");
        }
#endif

        if (adapter.Get() is null)
        {
            throw new GPUException("No Direct3D 12 device found");
        }

        *ppAdapter = adapter.Detach();
    }

    private static bool CheckIsSupported()
    {
        try
        {
#if NET6_0_OR_GREATER
            if (!OperatingSystem.IsWindows())
#else
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
            {
                return false;
            }

            using ComPtr<Dxgi.IDXGIFactory4> dxgiFactory = default;
            if (CreateDXGIFactory2(0, __uuidof<Dxgi.IDXGIFactory4>(), dxgiFactory.GetVoidAddressOf()).Failure)
            {
                return false;
            }

            bool foundCompatibleDevice = false;
            using ComPtr<Dxgi.IDXGIAdapter1> adapter = default;
            for (uint index = 0;
                dxgiFactory.Get()->EnumAdapters1(index, adapter.ReleaseAndGetAddressOf()).Success; ++index)
            {
                Dxgi.AdapterDescription1 desc;
                adapter.Get()->GetDesc1(&desc).ThrowIfFailed();

                if ((desc.Flags & Dxgi.AdapterFlags.Software) != Dxgi.AdapterFlags.None)
                {
                    // Don't select the Basic Render Driver adapter.
                    continue;
                }

                if (D3D12CreateDevice((IUnknown*)adapter.Get(),
                    s_minFeatureLevel,
                    __uuidof<ID3D12Device5>(), null).Success)
                {
                    foundCompatibleDevice = true;
                    break;
                }
            }

            if (!foundCompatibleDevice)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}

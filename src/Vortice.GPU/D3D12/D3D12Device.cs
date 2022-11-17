// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi;
using static Win32.Apis;
using static Win32.Graphics.Dxgi.Apis;
using static Win32.Graphics.Direct3D12.Apis;
#if !NET6_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12Device : GPUDevice
{
    private static readonly FeatureLevel s_minFeatureLevel = FeatureLevel.Level_11_0;
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);
    public static bool IsSupported() => s_isSupported.Value;

    public D3D12Device()
        : base(GPUBackendType.D3D12)
    {

    }

    /// <summary>
    /// Finalizes an instance of the <see cref="D3D12Device" /> class.
    /// </summary>
    ~D3D12Device() => Dispose(isDisposing: false);

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
        }
    }

    /// <inheritdoc />
    public override void WaitIdle()
    {
    }

    private static bool CheckIsSupported()
    {
        try
        {
#if NET6_0_OR_GREATER
            if(!OperatingSystem.IsWindows())
#else
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
            {
                return false;
            }

            using ComPtr<IDXGIFactory4> dxgiFactory = default;
            if (CreateDXGIFactory2(0, __uuidof<IDXGIFactory4>(), dxgiFactory.GetVoidAddressOf()).Failure)
            {
                return false;
            }

            bool foundCompatibleDevice = false;
            using ComPtr<IDXGIAdapter1> adapter = default;
            for (uint index = 0;
                dxgiFactory.Get()->EnumAdapters1(index, adapter.ReleaseAndGetAddressOf()).Success; ++index)
            {
                AdapterDescription1 desc;
                adapter.Get()->GetDesc1(&desc).ThrowIfFailed();

                if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
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

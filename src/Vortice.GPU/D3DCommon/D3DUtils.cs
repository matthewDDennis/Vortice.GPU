// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using Win32.Graphics.Dxgi;
using Win32.Graphics.Dxgi.Common;

namespace Vortice.GPU;

internal static class D3DUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GpuPreference ToDXGI(GPUPowerPreference preference)
    {
        switch (preference)
        {
            case GPUPowerPreference.HighPerformance:
                return GpuPreference.HighPerformance;

            case GPUPowerPreference.LowPower:
                return GpuPreference.MinimumPower;

            case GPUPowerPreference.Undefined:
            default:
                return GpuPreference.Unspecified;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Format ToDXGI(PixelFormat format)
    {
        switch (format)
        {
            // 8-bit formats
            case PixelFormat.R8Unorm: return Format.R8Unorm;
            case PixelFormat.R8Snorm: return Format.R8Snorm;
            case PixelFormat.R8Uint: return Format.R8Uint;
            case PixelFormat.R8Sint: return Format.R8Sint;
            // 16-bit formats
            case PixelFormat.R16Unorm: return Format.R16Unorm;
            case PixelFormat.R16Snorm: return Format.R16Snorm;
            case PixelFormat.R16Uint: return Format.R16Uint;
            case PixelFormat.R16Sint: return Format.R16Sint;
            case PixelFormat.R16Float: return Format.R16Float;
            case PixelFormat.RG8Unorm: return Format.R8G8Unorm;
            case PixelFormat.RG8Snorm: return Format.R8G8Snorm;
            case PixelFormat.RG8Uint: return Format.R8G8Uint;
            case PixelFormat.RG8Sint: return Format.R8G8Sint;
            // Packed 16-Bit Pixel Formats
            case PixelFormat.BGRA4Unorm: return Format.B4G4R4A4Unorm;
            case PixelFormat.B5G6R5Unorm: return Format.B5G6R5Unorm;
            case PixelFormat.B5G5R5A1Unorm: return Format.B5G5R5A1Unorm;
            // 32-bit formats
            case PixelFormat.R32Uint: return Format.R32Uint;
            case PixelFormat.R32Sint: return Format.R32Sint;
            case PixelFormat.R32Float: return Format.R32Float;
            case PixelFormat.RG16Unorm: return Format.R16G16Unorm;
            case PixelFormat.RG16Snorm: return Format.R16G16Snorm;
            case PixelFormat.RG16Uint: return Format.R16G16Uint;
            case PixelFormat.RG16Sint: return Format.R16G16Sint;
            case PixelFormat.RG16Float: return Format.R16G16Float;
            case PixelFormat.RGBA8Unorm: return Format.R8G8B8A8Unorm;
            case PixelFormat.RGBA8UnormSrgb: return Format.R8G8B8A8UnormSrgb;
            case PixelFormat.RGBA8Snorm: return Format.R8G8B8A8Snorm;
            case PixelFormat.RGBA8Uint: return Format.R8G8B8A8Uint;
            case PixelFormat.RGBA8Sint: return Format.R8G8B8A8Sint;
            case PixelFormat.BGRA8Unorm: return Format.B8G8R8A8Unorm;
            case PixelFormat.BGRA8UnormSrgb: return Format.B8G8R8A8UnormSrgb;
            // Packed 32-Bit formats
            case PixelFormat.RGB9E5Ufloat:          return Format.R9G9B9E5SharedExp;
            case PixelFormat.RGB10A2Unorm:          return Format.R10G10B10A2Unorm;
            case PixelFormat.RGB10A2Uint:           return Format.R10G10B10A2Uint;
            case PixelFormat.RG11B10Float:          return Format.R11G11B10Float;
            // 64-Bit formats
            case PixelFormat.RG32Uint:              return Format.R32G32Uint;
            case PixelFormat.RG32Sint:              return Format.R32G32Sint;
            case PixelFormat.RG32Float:             return Format.R32G32Float;
            case PixelFormat.RGBA16Unorm:           return Format.R16G16B16A16Unorm;
            case PixelFormat.RGBA16Snorm:           return Format.R16G16B16A16Snorm;
            case PixelFormat.RGBA16Uint:            return Format.R16G16B16A16Uint;
            case PixelFormat.RGBA16Sint:            return Format.R16G16B16A16Sint;
            case PixelFormat.RGBA16Float:           return Format.R16G16B16A16Float;
            // 128-Bit formats
            case PixelFormat.RGBA32Uint:            return Format.R32G32B32A32Uint;
            case PixelFormat.RGBA32Sint:            return Format.R32G32B32A32Sint;
            case PixelFormat.RGBA32Float:           return Format.R32G32B32A32Float;
            // Depth-stencil formats
            case PixelFormat.Depth16Unorm:          return Format.D16Unorm;
            case PixelFormat.Depth32Float:          return Format.D32Float;
            case PixelFormat.Stencil8:              return Format.D24UnormS8Uint;
            case PixelFormat.Depth24UnormStencil8:  return Format.D24UnormS8Uint;
            case PixelFormat.Depth32FloatStencil8:  return Format.D32FloatS8X24Uint;
            // Compressed BC formats
            case PixelFormat.BC1RgbaUnorm:          return Format.BC1Unorm;
            case PixelFormat.BC1RgbaUnormSrgb:      return Format.BC1UnormSrgb;
            case PixelFormat.BC2RgbaUnorm:          return Format.BC2Unorm;
            case PixelFormat.BC2RgbaUnormSrgb:      return Format.BC2UnormSrgb;
            case PixelFormat.BC3RgbaUnorm:          return Format.BC3Unorm;
            case PixelFormat.BC3RgbaUnormSrgb:      return Format.BC3UnormSrgb;
            case PixelFormat.BC4RSnorm:             return Format.BC4Unorm;
            case PixelFormat.BC4RUnorm:             return Format.BC4Snorm;
            case PixelFormat.BC5RgUnorm:            return Format.BC5Unorm;
            case PixelFormat.BC5RgSnorm:            return Format.BC5Snorm;
            case PixelFormat.BC6HRgbUfloat:         return Format.BC6HUF16;
            case PixelFormat.BC6HRgbSfloat:         return Format.BC6HSF16;
            case PixelFormat.BC7RgbaUnorm:          return Format.BC7Unorm;
            case PixelFormat.BC7RgbaUnormSrgb:      return Format.BC7UnormSrgb;

            default:
                return Format.Unknown;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Format ToDXGISwapChainFormat(PixelFormat format)
    {
        // FLIP_DISCARD and FLIP_SEQEUNTIAL swapchain buffers only support these formats
        switch (format)
        {
            case PixelFormat.RGBA16Float:
                return Format.R16G16B16A16Float;

            case PixelFormat.BGRA8Unorm:
            case PixelFormat.BGRA8UnormSrgb:
                return Format.B8G8R8A8Unorm;

            case PixelFormat.RGBA8Unorm:
            case PixelFormat.RGBA8UnormSrgb:
                return Format.R8G8B8A8Unorm;

            case PixelFormat.RGB10A2Unorm:
                return Format.R10G10B10A2Unorm;

            default:
                return Format.B8G8R8A8Unorm;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint PresentModeToBufferCount(PresentMode mode)
    {
        switch (mode)
        {
            case PresentMode.Immediate:
            case PresentMode.Fifo:
                return 2;
            case PresentMode.Mailbox:
                return 3;
            default:
                return 2;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint PresentModeToSyncInterval(PresentMode mode)
    {
        switch (mode)
        {
            case PresentMode.Immediate:
            case PresentMode.Mailbox:
                return 0u;

            case PresentMode.Fifo:
            default:
                return 1u;
        }
    }
}

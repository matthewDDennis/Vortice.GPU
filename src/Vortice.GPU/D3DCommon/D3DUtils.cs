// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using Win32.Graphics.Dxgi.Common; 

namespace Vortice.GPU;

internal static class D3DUtils
{
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
}

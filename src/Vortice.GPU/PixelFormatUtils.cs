// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Vortice.GPU;

/// <summary>
/// Defines utility methods for <see cref="PixelFormat"/>.
/// </summary>
public static class PixelFormatUtils
{
    private static readonly PixelFormatInfo[] s_formatInfos = new PixelFormatInfo[]
    {
        new(PixelFormat.Invalid,                0, 0, 0, PixelFormatKind.Unorm),
        // 8-bit pixel formats
        new(PixelFormat.R8Unorm,                1, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.R8Snorm,                1, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.R8Uint,                 1, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.R8Sint,                 1, 1, 1, PixelFormatKind.Sint),
        // 16-bit pixel formats
        new(PixelFormat.R16Unorm,               2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.R16Snorm,               2, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.R16Uint,                2, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.R16Sint,                2, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.R16Float,               2, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.RG8Unorm,               2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.RG8Snorm,               2, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.RG8Uint,                2, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.RG8Sint,                2, 1, 1, PixelFormatKind.Sint),
        // Packed 16-Bit Pixel Formats
        new(PixelFormat.BGRA4Unorm,             2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.B5G6R5Unorm,            2, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.B5G5R5A1Unorm,          2, 1, 1, PixelFormatKind.Unorm),
        // 32-bit pixel formats
        new(PixelFormat.R32Uint,                4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.R32Sint,                4, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.R32Float,               4, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.RG16Unorm,              4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.RG16Snorm,              4, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.RG16Uint,               4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.RG16Sint,               4, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.RG16Float,              4, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.RGBA8Unorm,             4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.RGBA8UnormSrgb,         4, 1, 1, PixelFormatKind.UnormSrgb),
        new(PixelFormat.RGBA8Snorm,             4, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.RGBA8Uint,              4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.RGBA8Sint,              4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.BGRA8Unorm,             4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.BGRA8UnormSrgb,         4, 1, 1, PixelFormatKind.UnormSrgb),
        // Packed 32-Bit Pixel formats
        new(PixelFormat.RGB9E5Ufloat,           4, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.RGB10A2Unorm,           4, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.RGB10A2Uint,            4, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.RG11B10Float,           4, 1, 1, PixelFormatKind.Float),
        // 64-Bit Pixel Formats
        new(PixelFormat.RG32Uint,               8, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.RG32Sint,               8, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.RG32Float,              8, 1, 1, PixelFormatKind.Float),
        new(PixelFormat.RGBA16Unorm,            8, 1, 1, PixelFormatKind.Unorm),
        new(PixelFormat.RGBA16Snorm,            8, 1, 1, PixelFormatKind.Snorm),
        new(PixelFormat.RGBA16Uint,             8, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.RGBA16Sint,             8, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.RGBA16Float,            8, 1, 1, PixelFormatKind.Float),
        // 128-Bit Pixel Formats
        new(PixelFormat.RGBA32Uint,            16, 1, 1, PixelFormatKind.Uint),
        new(PixelFormat.RGBA32Sint,            16, 1, 1, PixelFormatKind.Sint),
        new(PixelFormat.RGBA32Float,           16, 1, 1, PixelFormatKind.Float),
        // Depth-stencil formats
        new (PixelFormat.Depth16Unorm,          2, 1, 1, PixelFormatKind.Unorm),
        new (PixelFormat.Depth32Float,          4, 1, 1, PixelFormatKind.Float),
        new (PixelFormat.Stencil8,              1, 1, 1, PixelFormatKind.Unorm),
        new (PixelFormat.Depth24UnormStencil8,  4, 1, 1, PixelFormatKind.Unorm),
        new (PixelFormat.Depth32FloatStencil8,  8, 1, 1, PixelFormatKind.Float),
        // BC compressed formats
        new (PixelFormat.BC1RgbaUnorm,          8, 4, 4,  PixelFormatKind.Unorm),
        new (PixelFormat.BC1RgbaUnormSrgb,      8, 4, 4,  PixelFormatKind.UnormSrgb),
        new (PixelFormat.BC2RgbaUnorm,          16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.BC2RgbaUnormSrgb,      16, 4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.BC3RgbaUnorm,          16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.BC3RgbaUnormSrgb,      16, 4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.BC4RUnorm,             8,  4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.BC4RSnorm,             8,  4, 4, PixelFormatKind.Snorm),
        new (PixelFormat.BC5RgUnorm,            16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.BC5RgSnorm,            16, 4, 4, PixelFormatKind.Snorm),
        new (PixelFormat.BC6HRgbUfloat,         16, 4, 4, PixelFormatKind.Float),
        new (PixelFormat.BC6HRgbSfloat,         16, 4, 4, PixelFormatKind.Float),
        new (PixelFormat.BC7RgbaUnorm,          16, 4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.BC7RgbaUnormSrgb,      16, 4, 4, PixelFormatKind.UnormSrgb),
        // ETC2/EAC compressed formats
        new (PixelFormat.Etc2Rgb8Unorm,        8,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Etc2Rgb8UnormSrgb,    8,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Etc2Rgb8A1Unorm,     16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Etc2Rgb8A1UnormSrgb, 16,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.Etc2Rgba8Unorm,      16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.Etc2Rgba8UnormSrgb,  16,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.EacR11Unorm,         8,    4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.EacR11Snorm,         8,    4, 4, PixelFormatKind.Snorm),
        new (PixelFormat.EacRg11Unorm,        16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.EacRg11Snorm,        16,   4, 4, PixelFormatKind.Snorm),

        // ASTC compressed formats
        new (PixelFormat.ASTC4x4Unorm,        16,   4, 4, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC4x4UnormSrgb,    16,   4, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC5x4Unorm,        16,   5, 4, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC5x4UnormSrgb,    16,   5, 4, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC5x5Unorm,        16,   5, 5, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC5x5UnormSrgb,    16,   5, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC6x5Unorm,        16,   6, 5, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC6x5UnormSrgb,    16,   6, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC6x6Unorm,        16,   6, 6, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC6x6UnormSrgb,    16,   6, 6, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC8x5Unorm,        16,   8, 5, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC8x5UnormSrgb,    16,   8, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC8x6Unorm,        16,   8, 6, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC8x6UnormSrgb,    16,   8, 6, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC8x8Unorm,        16,   8, 8, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC8x8UnormSrgb,    16,   8, 8, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC10x5Unorm,       16,   10, 5, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC10x5UnormSrgb,   16,   10, 5, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC10x6Unorm,       16,   10, 6, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC10x6UnormSrgb,   16,   10, 6, PixelFormatKind.UnormSrgb ),
        new (PixelFormat.ASTC10x8Unorm,       16,   10, 8, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC10x8UnormSrgb,   16,   10, 8, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC10x10Unorm,      16,   10, 10, PixelFormatKind.Unorm ),
        new (PixelFormat.ASTC10x10UnormSrgb,  16,   10, 10, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC12x10Unorm,      16,   12, 10, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC12x10UnormSrgb,  16,   12, 10, PixelFormatKind.UnormSrgb),
        new (PixelFormat.ASTC12x12Unorm,      16,   12, 12, PixelFormatKind.Unorm),
        new (PixelFormat.ASTC12x12UnormSrgb,  16,   12, 12, PixelFormatKind.UnormSrgb),
    };

    public static ref readonly PixelFormatInfo GetFormatInfo(this PixelFormat format)
    {
        if (format >= PixelFormat.Count)
        {
            return ref s_formatInfos[0]; // UNKNOWN
        }

        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return ref s_formatInfos[(int)format];
    }

    /// <summary>
    /// Get the number of bytes per format
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int GetFormatBytesPerBlock(PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(int)format].BytesPerBlock;
    }

    /// <summary>
    /// Check if the format has a depth component.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has depth component, false otherwise.</returns>
    public static bool IsDepthFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.Depth16Unorm:
            case PixelFormat.Depth32Float:
            case PixelFormat.Depth24UnormStencil8:
            case PixelFormat.Depth32FloatStencil8:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Check if the format has a stencil component.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has stencil component, false otherwise.</returns>
    public static bool IsStencilFormat(PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.Stencil8:
            case PixelFormat.Depth24UnormStencil8:
            case PixelFormat.Depth32FloatStencil8:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Check if the format has depth or stencil components.
    /// </summary>
    /// <param name="format">The <see cref="PixelFormat"/> to check.</param>
    /// <returns>True if format has depth or stencil component, false otherwise.</returns>
    public static bool IsDepthStencilFormat(this PixelFormat format)
    {
        switch (format)
        {
            case PixelFormat.Depth16Unorm:
            case PixelFormat.Depth32Float:
            case PixelFormat.Stencil8:
            case PixelFormat.Depth24UnormStencil8:
            case PixelFormat.Depth32FloatStencil8:
                return true;
            default:
                return false;
        }
    }

    public static bool IsSigned(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(int)format].Kind == PixelFormatKind.Sint;
    }

    public static bool IsSRGB(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(int)format].Kind == PixelFormatKind.UnormSrgb;
    }

    /// <summary>
    /// Check if the format is a compressed format.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static bool IsCompressedFormat(this PixelFormat format)
    {
        Guard.IsTrue(s_formatInfos[(int)format].Format == format);
        return s_formatInfos[(int)format].BlockWidth > 1;
    }
}

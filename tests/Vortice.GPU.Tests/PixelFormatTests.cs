// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using static Vortice.GPU.PixelFormatUtils;
using NUnit.Framework;

namespace Vortice.GPU.Tests;

[TestFixture(TestOf = typeof(PixelFormat))]
public class TextureFormatTests
{
    [TestCase]
    public void IsDepthFormatTests()
    {
        Assert.True(PixelFormat.Depth16Unorm.IsDepthFormat());
        Assert.True(PixelFormat.Depth32Float.IsDepthFormat());
        Assert.False(PixelFormat.Stencil8.IsDepthFormat());
        Assert.True(PixelFormat.Depth24UnormStencil8.IsDepthFormat());
        Assert.True(PixelFormat.Depth32FloatStencil8.IsDepthFormat());
    }

    [TestCase]
    public void IsStencilFormatTests()
    {
        Assert.False(IsStencilFormat(PixelFormat.Depth16Unorm));
        Assert.False(IsStencilFormat(PixelFormat.Depth32Float));
        Assert.True(IsStencilFormat(PixelFormat.Stencil8));
        Assert.True(IsStencilFormat(PixelFormat.Depth24UnormStencil8));
        Assert.True(IsStencilFormat(PixelFormat.Depth32FloatStencil8));
    }

    [TestCase]
    public void IsDepthStencilFormatTests()
    {
        Assert.False(PixelFormat.R8Unorm.IsDepthStencilFormat());
        Assert.True(PixelFormat.Depth16Unorm.IsDepthStencilFormat());
        Assert.True(PixelFormat.Depth32Float.IsDepthStencilFormat());
        Assert.True(PixelFormat.Stencil8.IsDepthStencilFormat());
        Assert.True(PixelFormat.Depth24UnormStencil8.IsDepthStencilFormat());
        Assert.True(PixelFormat.Depth32FloatStencil8.IsDepthStencilFormat());
    }


    [TestCase]
    public void IsCompressedFormatTests()
    {
        // Compressed BC formats
        Assert.True(PixelFormat.BC1RgbaUnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC1RgbaUnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.BC2RgbaUnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC2RgbaUnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.BC3RgbaUnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC3RgbaUnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.BC4RUnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC4RSnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC5RgUnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC5RgSnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC6HRgbUfloat.IsCompressedFormat());
        Assert.True(PixelFormat.BC6HRgbSfloat.IsCompressedFormat());
        Assert.True(PixelFormat.BC7RgbaUnorm.IsCompressedFormat());
        Assert.True(PixelFormat.BC7RgbaUnormSrgb.IsCompressedFormat());

        // Compressed EAC/ETC formats
        Assert.True(PixelFormat.Etc2Rgb8Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.Etc2Rgb8UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.Etc2Rgb8A1Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.Etc2Rgb8A1UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.Etc2Rgba8Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.Etc2Rgba8UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.EacR11Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.EacR11Snorm.IsCompressedFormat());
        Assert.True(PixelFormat.EacRg11Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.EacRg11Snorm.IsCompressedFormat());

        Assert.True(PixelFormat.ASTC4x4Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC4x4UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC5x4Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC5x4UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC5x5Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC5x5UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC6x5Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC6x5UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC6x6Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC6x6UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC8x5Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC8x5UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC8x6Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC8x6UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC8x8Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC8x8UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x5Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x5UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x6Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x6UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x8Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x8UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x10Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC10x10UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC12x10Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC12x10UnormSrgb.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC12x12Unorm.IsCompressedFormat());
        Assert.True(PixelFormat.ASTC12x12UnormSrgb.IsCompressedFormat());

        // Other formats
        Assert.False(PixelFormat.R8Unorm.IsCompressedFormat());
    }
}

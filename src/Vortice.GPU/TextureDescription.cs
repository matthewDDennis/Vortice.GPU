// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;

namespace Vortice.GPU;

/// <summary>
/// Structure that describes the <see cref="Texture"/>.
/// </summary>
public readonly record struct TextureDescription
{
    [SetsRequiredMembers]
    public TextureDescription(
        TextureDimension dimension,
        PixelFormat format,
        int width,
        int height,
        int depthOrArrayLayers,
        int mipLevels = 1,
        TextureUsage usage = TextureUsage.ShaderRead,
        TextureSampleCount sampleCount = TextureSampleCount.Count1,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        Guard.IsTrue(format != PixelFormat.Invalid);
        Guard.IsGreaterThanOrEqualTo(width, 1);
        Guard.IsGreaterThanOrEqualTo(height, 1);
        Guard.IsGreaterThanOrEqualTo(depthOrArrayLayers, 1);

        Dimension = dimension;
        Format = format;
        Width = width;
        Height = height;
        DepthOrArrayLayers = depthOrArrayLayers;
        MipLevels = mipLevels;
        Usage = usage;
        SampleCount = sampleCount;
        CpuAccess = access;
        Label = label;
    }

    /// <summary>
    /// Gets the dimension of <see cref="Texture"/>
    /// </summary>
    public required TextureDimension Dimension { get; init; }

    /// <summary>
    /// Gets the pixel format of <see cref="Texture"/>
    /// </summary>
    public required PixelFormat Format { get; init; }

    /// <summary>
    /// Gets the width of <see cref="Texture"/>
    /// </summary>
    public required int Width { get; init; }

    /// <summary>
    /// Gets the height of <see cref="Texture"/>
    /// </summary>
    public required int Height { get; init; }

    /// <summary>
    /// Gets the depth of <see cref="Texture"/>, if it is 3D, or the array layers if it is an array of 1D or 2D resources.
    /// </summary>
    public required int DepthOrArrayLayers { get; init; }

    /// <summary>
    /// Gets the number of MIP levels in the <see cref="Texture"/>
    /// </summary>
    public required int MipLevels { get; init; }

    /// <summary>
    /// Gets the <see cref="TextureUsage"/> of <see cref="Texture"/>.
    /// </summary>
    public TextureUsage Usage { get; init; } = TextureUsage.ShaderRead;

    /// <summary>
    /// Gets the texture sample count.
    /// </summary>
    public TextureSampleCount SampleCount { get; init; } = TextureSampleCount.Count1;

    /// <summary>
    /// CPU access of the <see cref="Texture"/>.
    /// </summary>
    public CpuAccessMode CpuAccess { get; init; } = CpuAccessMode.None;

    // <summary>
    /// Gets the label of <see cref="Texture"/>.
    /// </summary>
    public string? Label { get; init; }

    public static TextureDescription Texture2D(
        PixelFormat format,
        int width,
        int height,
        int mipLevels,
        int arrayLayers,
        TextureUsage usage = TextureUsage.ShaderRead)
    {
        return new TextureDescription(
            TextureDimension.Texture2D,
            format,
            width,
            height,
            arrayLayers,
            mipLevels,
            usage,
            TextureSampleCount.Count1);
    }
}

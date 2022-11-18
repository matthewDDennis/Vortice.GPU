// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Vortice.GPU;

/// <summary>
/// Structure that describes the <see cref="SwapChain"/>.
/// </summary>
public readonly record struct SwapChainDescription
{
    public SwapChainDescription()
    {
    }

    //public TextureUsage ColorUsage { get; init; } = TextureUsage.ShaderRead;
    public PixelFormat ColorFormat { get; init; } = PixelFormat.BGRA8UnormSrgb;
    public PresentMode PresentMode { get; init; } = PresentMode.Fifo;

    // <summary>
    /// Gets the label of <see cref="SwapChain"/>.
    /// </summary>
    public string? Label { get; init; }
}

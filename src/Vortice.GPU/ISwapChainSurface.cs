// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;

namespace Vortice.GPU;

public interface ISwapChainSurface : IDisposable
{
    /// <summary>Gets the surface kind.</summary>
    SwapChainSurfaceKind Kind { get; }

    /// <summary>
    /// Gets the size, in pixels, of the surface.
    /// </summary>
    Size Size { get; }

    /// <summary>
    /// Gets whether the surface is in fullscreen.
    /// </summary>
    bool IsFullscreen { get; }

    nint SurfaceDisplay { get; }
    nint SurfaceHandle { get; }
}

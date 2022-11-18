// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

/// <summary>
/// A bitmask indicating how a <see cref="Texture"/> is permitted to be used.
/// </summary>
[Flags]
public enum TextureUsage
{
    /// <summary>
    /// None usage.
    /// </summary>
    None = 0,
    /// <summary>
    /// Supports shader read access.
    /// </summary>
    ShaderRead = 1 << 0,
    /// <summary>
    /// Supports write read access.
    /// </summary>
    ShaderWrite = 1 << 1,
    RenderTarget = 1 << 2,
    ShadingRate = 1 << 3,
    /// <summary>
    /// Supports shader read and write access.
    /// </summary>
    ShaderReadWrite = ShaderRead | ShaderWrite,
}

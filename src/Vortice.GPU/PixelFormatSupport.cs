// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

/// <summary>
/// Define a <see cref="PixelFormat"/> supported usage.
/// </summary>
[Flags]
public enum PixelFormatSupport : uint
{
    None = 0,
    ShaderRead = 1 << 0,
    ShaderWrite = 1 << 1,
}

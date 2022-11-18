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
    ShaderLoad = 0x00000001,
    ShaderSample = 0x00000002,
    ShaderWrite = 0x00000004,
    RenderTarget = 0x00000008,
    DepthStencil = 0x00000010,
    Blendable = 0x00000020,
}

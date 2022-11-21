// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;

namespace Vortice.GPU;

public readonly record struct RenderPassColorAttachment
{
    public required Texture Texture { get; init; }
    public required Color ClearColor { get; init; }
}

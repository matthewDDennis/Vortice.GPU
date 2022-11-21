// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Numerics;
using Win32.Numerics;

namespace Vortice.GPU.VertexTypes;

public readonly struct VertexPositionColor
{
    public static readonly unsafe int SizeInBytes = sizeof(VertexPositionColor);

    public VertexPositionColor(in Vector3 position, in Color4 color)
    {
        Position = position;
        Color = color;
    }

    public readonly Vector3 Position;
    public readonly Color4 Color;
}

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public readonly record struct PixelFormatInfo(
    PixelFormat Format,
    int BytesPerBlock,
    int BlockWidth,
    int BlockHeight,
    PixelFormatKind Kind
    );

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public readonly record struct DispatchIndirectCommand
{
    public required uint ThreadGroupCountX { get; init; }
    public required uint ThreadGroupCountY { get; init; }
    public required uint ThreadGroupCountZ { get; init; }
}

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;
public readonly record struct DrawIndirectCommand
{
    public required uint VertexCount { get; init; }
    public required uint InstanceCount { get; init; }
    public required uint FirstVertex { get; init; }
    public required uint FirstInstance { get; init; }
}

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public readonly record struct DrawIndexedIndirectCommand
{
    public required uint IndexCount { get; init; }
    public required uint InstanceCount { get; init; }
    public required uint FirstIndex { get; init; }
    public required int BaseVertex { get; init; }
    public required uint FirstInstance { get; init; }
}

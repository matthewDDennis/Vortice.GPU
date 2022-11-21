// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

/// <summary>
/// Structure that describes the <see cref="Pipeline"/>.
/// </summary>
public readonly record struct RenderPipelineDescription
{
    // <summary>
    /// Gets the label of <see cref="Pipeline"/>.
    /// </summary>
    public string? Label { get; init; }
}

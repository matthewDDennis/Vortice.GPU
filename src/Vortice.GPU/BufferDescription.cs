// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Diagnostics.CodeAnalysis;

namespace Vortice.GPU;

/// <summary>
/// Structure that describes the <see cref="GPUBuffer"/>.
/// </summary>
public readonly record struct BufferDescription
{
    [SetsRequiredMembers]
    public BufferDescription(
        ulong size,
        BufferUsage usage = BufferUsage.ShaderReadWrite,
        CpuAccessMode access = CpuAccessMode.None,
        string? label = default)
    {
        Usage = usage;
        Size = size;
        CpuAccess = access;
        Label = label;
    }

    /// <summary>
    /// Gets the size in bytes of <see cref="GPUBuffer"/>
    /// </summary>
    public required ulong Size { get; init; }

    /// <summary>
    /// Gets the <see cref="BufferUsage"/> of <see cref="GPUBuffer"/>.
    /// </summary>
    public required BufferUsage Usage { get; init; }

    /// <summary>
    /// CPU access of the <see cref="GPUBuffer"/>.
    /// </summary>
    public required CpuAccessMode CpuAccess { get; init; } = CpuAccessMode.None;

    // <summary>
    /// Gets or sets the label of <see cref="GPUBuffer"/>.
    /// </summary>
    public string? Label { get; init; }
}

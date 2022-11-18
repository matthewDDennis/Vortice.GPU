// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;
using System.Runtime.InteropServices;

namespace Vortice.GPU;

public abstract class GPUBuffer : GPUResource
{
    protected GPUBuffer(GPUDevice device, in BufferDescription description)
        : base(device, description.Label)
    {
        Size = description.Size;
        Usage = description.Usage;
        CpuAccess = description.CpuAccess;
    }

    /// <summary>
    /// Gets the size in bytes of this buffer.
    /// </summary>
    public ulong Size { get; }

    /// <summary>
    /// Gets the <see cref="BufferUsage"/> of this buffer.
    /// </summary>
    public BufferUsage Usage { get; }

    /// <summary>
    /// CPU access of this buffer.
    /// </summary>
    public CpuAccessMode CpuAccess { get; }
}

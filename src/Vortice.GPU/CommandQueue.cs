// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Vortice.GPU;

public abstract class CommandQueue : GPUObject
{
    /// <summary>Initializes a new instance of the <see cref="CommandQueue" /> class.</summary>
    /// <param name="device">The device object that created the resource..</param>
    /// <param name="label">The label of the object or <c>null</c> to use <see cref="MemberInfo.Name" />.</param>
    protected CommandQueue(GPUDevice device, string? label = default)
        : base(label)
    {
        Guard.IsNotNull(device, nameof(device));

        Device = device;
    }

    /// <summary>
    /// Get the <see cref="GPUDevice"/> object that created the resource.
    /// </summary>
    public GPUDevice Device { get; }
}

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Vortice.GPU;

public abstract class CommandQueue : GPUObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandQueue" /> class.
    /// </summary>
    protected CommandQueue(GPUDevice device, CommandQueueType type, string? label = default)
        : base(label)
    {
        Guard.IsNotNull(device, nameof(device));

        Device = device;
        QueueType = type;
    }

    /// <summary>
    /// Get the <see cref="GPUDevice"/> object that created the queue.
    /// </summary>
    public GPUDevice Device { get; }

    /// <summary>
    /// Get the queue type.
    /// </summary>
    public CommandQueueType QueueType { get; }

    public abstract void WaitIdle();

    public abstract CommandBuffer BeginCommandBuffer();
}

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public abstract class Pipeline : GPUResource
{
    protected Pipeline(GPUDevice device, in PipelineType pipelineType, string? label = default)
        : base(device, label)
    {
        PipelineType = pipelineType;
    }

    public PipelineType PipelineType { get; }
}

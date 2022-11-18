// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public abstract class Texture : GPUResource
{
    protected Texture(GPUDevice device, in TextureDescription description)
        : base(device, description.Label)
    {
    }
}

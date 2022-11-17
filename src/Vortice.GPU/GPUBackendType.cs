// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public enum GPUBackendType
{
    Null,
    WebGPU,
    D3D11,
    D3D12,
    Metal,
    Vulkan,

    Count,
}

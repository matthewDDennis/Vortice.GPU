// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public abstract class SwapChain : GPUResource
{
    protected SwapChain(GPUDevice device, in ISwapChainSurface surface, in SwapChainDescription description)
        : base(device, description.Label)
    {
        Surface = surface;
        ColorFormat = description.ColorFormat;
    }

    public ISwapChainSurface Surface { get; }
    public PixelFormat ColorFormat { get; protected set; }
}

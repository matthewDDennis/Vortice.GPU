// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

namespace Vortice.GPU;

public abstract class CommandBuffer
{
    private bool _insideRenderPass;

    /// <summary>
    /// Get the <see cref="CommandQueue"/> object that created the resource.
    /// </summary>
    public abstract CommandQueue Queue { get; }

    /// <summary>
    /// Get the <see cref="GPUDevice"/> object that created the resource.
    /// </summary>
    public GPUDevice Device => Queue.Device;

    public abstract void Commit();

    public Texture? AcquireSwapchainTexture(SwapChain swapChain)
    {
        return AcquireSwapchainTextureCore(swapChain);
    }

    public void BeginRenderPass(in RenderPassColorAttachment colorAttachment)
    {
        Guard.IsFalse(_insideRenderPass);

        BeginRenderPassCore(colorAttachment);
        _insideRenderPass = true;
    }

    public void EndRenderPass()
    {
        Guard.IsTrue(_insideRenderPass);

        EndRenderPassCore();
        _insideRenderPass = false;
    }

    protected void ResetState()
    {
        _insideRenderPass = false;
    }

    protected abstract Texture? AcquireSwapchainTextureCore(SwapChain swapChain);
    protected abstract void BeginRenderPassCore(in RenderPassColorAttachment colorAttachment);
    protected abstract void EndRenderPassCore();
}

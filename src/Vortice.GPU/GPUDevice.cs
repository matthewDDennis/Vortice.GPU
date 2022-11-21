// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using CommunityToolkit.Diagnostics;
using Win32.Graphics.Dxgi;

namespace Vortice.GPU;

public abstract class GPUDevice : GPUObject
{
    protected GPUDevice(GPUBackendType backendType)
    {
        BackendType = backendType;
    }

    /// <summary>
    /// Get the device backend type.
    /// </summary>
    public GPUBackendType BackendType { get; }

    /// <summary>
    /// Gets the adapter (physical device) properties.
    /// </summary>
    public abstract AdapterProperties AdapterProperties { get; }

    /// <summary>
    /// Gets the device limits.
    /// </summary>
    public abstract GPUDeviceLimits Limits { get; }

    /// <summary>
    /// Gets the graphics command queue.
    /// </summary>
    public abstract CommandQueue GraphicsQueue { get; }

    /// <summary>
    /// Checks whether the given <see cref="GPUBackendType"/> is supported on this system.
    /// </summary>
    /// <param name="backend">The GraphicsBackend to check.</param>
    /// <returns>True if the GraphicsBackend is supported; false otherwise.</returns>
    public static bool IsBackendSupported(GPUBackendType backend)
    {
        switch (backend)
        {
            case GPUBackendType.D3D12:
#if !EXCLUDE_D3D12_BACKEND
                return D3D12.D3D12Device.IsSupported();
#else
                return false;
#endif

            case GPUBackendType.Vulkan:
#if !EXCLUDE_VULKAN_BACKEND
                return Vulkan.VulkanDevice.IsSupported();
#else
                return false;
#endif

            default:
                return ThrowHelper.ThrowArgumentException<bool>("Invalid GraphicsBackend value");
        }
    }

    public static GPUDevice CreateDefault(
        ValidationMode validationMode = ValidationMode.Disabled,
        GPUPowerPreference powerPreference = GPUPowerPreference.Undefined)
    {
#if !EXCLUDE_D3D12_BACKEND
        if (D3D12.D3D12Device.IsSupported())
        {
            return new D3D12.D3D12Device(validationMode, powerPreference);
        }
#endif

#if !EXCLUDE_VULKAN_BACKEND
        if (Vulkan.VulkanDevice.IsSupported())
        {
            return new Vulkan.VulkanDevice(validationMode, powerPreference);
        }
#endif

        throw new GPUException("No supported backend on current platform");
    }

    /// <summary>
    /// Wait for device to finish pending GPU operations.
    /// </summary>
    public abstract void WaitIdle();

    public abstract PixelFormatSupport QueryPixelFormatSupport(PixelFormat format);

    public unsafe GPUBuffer CreateBuffer(in BufferDescription description)
    {
        return CreateBuffer(description, null);
    }

    public unsafe GPUBuffer CreateBuffer(in BufferDescription description, IntPtr initialData)
    {
        return CreateBuffer(description, initialData.ToPointer());
    }

    public unsafe GPUBuffer CreateBuffer(in BufferDescription description, void* initialData)
    {
        Guard.IsGreaterThanOrEqualTo(description.Size, 4, nameof(BufferDescription.Size));

        return CreateBufferCore(description, initialData);
    }

    public unsafe GPUBuffer CreateBuffer<T>(in BufferDescription description, ref T initialData) where T : unmanaged
    {
        Guard.IsGreaterThanOrEqualTo(description.Size, 4, nameof(BufferDescription.Size));

        fixed (void* initialDataPtr = &initialData)
        {
            return CreateBuffer(description, initialDataPtr);
        }
    }

    public GPUBuffer CreateBuffer<T>(in BufferDescription description, T[] initialData) where T : unmanaged
    {
        ReadOnlySpan<T> initialDataSppan = initialData.AsSpan();

        return CreateBuffer(description, ref MemoryMarshal.GetReference(initialDataSppan));
    }

    public GPUBuffer CreateBuffer<T>(in BufferDescription description, ReadOnlySpan<T> initialData) where T : unmanaged
    {
        return CreateBuffer(description, ref MemoryMarshal.GetReference(initialData));
    }

    public unsafe GPUBuffer CreateBuffer<T>(
        ReadOnlySpan<T> initialData,
        BufferUsage usage = BufferUsage.ShaderReadWrite,
        CpuAccessMode cpuAccess = CpuAccessMode.None,
        string? label = default) where T : unmanaged
    {
        BufferDescription description = new(sizeof(T) * initialData.Length, usage, cpuAccess, label);
        return CreateBuffer(description, ref MemoryMarshal.GetReference(initialData));
    }

    public unsafe Texture CreateTexture<T>(in TextureDescription description, ref T initialData) where T : unmanaged
    {
        Guard.IsGreaterThanOrEqualTo(description.Width, 1, nameof(TextureDescription.Width));
        Guard.IsGreaterThanOrEqualTo(description.Height, 1, nameof(TextureDescription.Height));

        fixed (void* initialDataPtr = &initialData)
        {
            return CreateTextureCore(description, initialDataPtr);
        }
    }

    public Pipeline CreateRenderPipeline(in RenderPipelineDescription description)
    {
        return CreateRenderPipelineCore(description);
    }

    public SwapChain CreateSwapChain(in ISwapChainSurface surface, in SwapChainDescription description)
    {
        Guard.IsGreaterThanOrEqualTo(surface.Size.Width, 1, nameof(ISwapChainSurface.Size));
        Guard.IsGreaterThanOrEqualTo(surface.Size.Height, 1, nameof(ISwapChainSurface.Size));

        return CreateSwapChainCore(surface, description);
    }

    protected abstract unsafe GPUBuffer CreateBufferCore(in BufferDescription description, void* initialData);
    protected abstract unsafe Texture CreateTextureCore(in TextureDescription description, void* initialData);
    protected abstract SwapChain CreateSwapChainCore(in ISwapChainSurface surface, in SwapChainDescription description);
    protected abstract Pipeline CreateRenderPipelineCore(in RenderPipelineDescription description);
}


// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;

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
                return false;
            //return Vulkan.VulkanGraphicsDevice.IsSupported();
#else
                return false;
#endif

            default:
                return ThrowHelper.ThrowArgumentException<bool>("Invalid GraphicsBackend value");
        }
    }

    /// <summary>
    /// Wait for device to finish pending GPU operations.
    /// </summary>
    public abstract void WaitIdle();

    public PixelFormatSupport QueryPixelFormatSupport(PixelFormat format)
    {
        return PixelFormatSupport.None;
    }

    //public unsafe Texture CreateTexture<T>(in TextureDescription description, ref T initialData) where T : unmanaged
    //{
    //    Guard.IsGreaterThanOrEqualTo(description.Width, 1, nameof(TextureDescription.Width));
    //    Guard.IsGreaterThanOrEqualTo(description.Height, 1, nameof(TextureDescription.Height));

    //    fixed (void* initialDataPtr = &initialData)
    //    {
    //        return CreateTextureCore(description, initialDataPtr);
    //    }
    //}

    //private unsafe Texture CreateTextureCore(in TextureDescription description, void* initialData)
    //{
    //    throw new NotImplementedException();
    //}
}


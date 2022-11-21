// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using CommunityToolkit.Diagnostics;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;
using static Vortice.GPU.Vulkan.VulkanUtils;
using Win32;

namespace Vortice.GPU.Vulkan;

internal unsafe class VulkanDevice : GPUDevice
{
    private static readonly VkString s_EngineName = new("Alimer");
    private static readonly Lazy<bool> s_isSupported = new(CheckIsSupported);

    public static bool IsSupported() => s_isSupported.Value;

    private readonly VkInstance _instance;
    //private readonly VkDevice _device;

    public VulkanDevice(ValidationMode validationMode, GPUPowerPreference powerPreference)
        : base(GPUBackendType.Vulkan)
    {
        Guard.IsTrue(IsSupported(), nameof(VulkanDevice), "Vulkan is not supported");

        // Create instance first.
        {
            List<string> instanceExtensions = new();
            List<string> instanceLayers = new();

            using VkString name = "Alimer";
            var appInfo = new VkApplicationInfo
            {
                pApplicationName = name,
                applicationVersion = new VkVersion(1, 0, 0),
                pEngineName = s_EngineName,
                engineVersion = new VkVersion(1, 0, 0),
                apiVersion = VkVersion.Version_1_2
            };

            using VkStringArray vkLayerNames = new(instanceLayers);
            using VkStringArray vkInstanceExtensions = new(instanceExtensions);

            var instanceCreateInfo = new VkInstanceCreateInfo
            {
                sType = VkStructureType.InstanceCreateInfo,
                pApplicationInfo = &appInfo,
                enabledLayerCount = vkLayerNames.Length,
                ppEnabledLayerNames = vkLayerNames,
                enabledExtensionCount = vkInstanceExtensions.Length,
                ppEnabledExtensionNames = vkInstanceExtensions
            };

            vkCreateInstance(&instanceCreateInfo, null, out _instance);
        }
    }

    public VkInstance VkInstance => _instance;
    public VkPhysicalDevice PhysicalDevice { get; }
    //public VkDevice NativeDevice => _device;

    /// <summary>
    /// Finalizes an instance of the <see cref="VulkanDevice" /> class.
    /// </summary>
    ~VulkanDevice() => Dispose(isDisposing: false);

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            vkDestroyInstance(_instance);
        }
    }

    /// <inheritdoc />
    public override AdapterProperties AdapterProperties { get; }

    /// <inheritdoc />
    public override GPUDeviceLimits Limits { get; }

    /// <inheritdoc />
    public override CommandQueue GraphicsQueue => default; // _commandQueues[(int)CommandQueueType.Graphics];

    /// <inheritdoc />
    public override void WaitIdle()
    {
    }

    /// <inheritdoc />
    public override PixelFormatSupport QueryPixelFormatSupport(PixelFormat format)
    {
        VkFormat vulkanFormat = ToVulkan(format);

        vkGetPhysicalDeviceFormatProperties(PhysicalDevice, vulkanFormat, out VkFormatProperties props);

        PixelFormatSupport result = PixelFormatSupport.None;
        if ((props.optimalTilingFeatures & VkFormatFeatureFlags.SampledImage) != VkFormatFeatureFlags.None)
        {
            result |= PixelFormatSupport.ShaderLoad;
        }
        if ((props.optimalTilingFeatures & VkFormatFeatureFlags.SampledImageFilterLinear) != VkFormatFeatureFlags.None)
        {
            result |= PixelFormatSupport.ShaderSample;
        }
        if ((props.optimalTilingFeatures & VkFormatFeatureFlags.StorageImage) != VkFormatFeatureFlags.None)
        {
            result |= PixelFormatSupport.ShaderWrite;
        }
        if ((props.optimalTilingFeatures & VkFormatFeatureFlags.ColorAttachment) != VkFormatFeatureFlags.None)
        {
            result |= PixelFormatSupport.RenderTarget;
        }
        if ((props.optimalTilingFeatures & VkFormatFeatureFlags.DepthStencilAttachment) != VkFormatFeatureFlags.None)
        {
            result |= PixelFormatSupport.DepthStencil;
        }
        if ((props.optimalTilingFeatures & VkFormatFeatureFlags.ColorAttachmentBlend) != VkFormatFeatureFlags.None)
        {
            result |= PixelFormatSupport.Blendable;
        }

        return result;
    }


    private static bool CheckIsSupported()
    {
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected override GPUBuffer CreateBufferCore(in BufferDescription description, void* initialData)
    {
        throw new NotImplementedException();
    }

    protected override Texture CreateTextureCore(in TextureDescription description, void* initialData)
    {
        throw new NotImplementedException();
    }

    protected override SwapChain CreateSwapChainCore(in ISwapChainSurface surface, in SwapChainDescription description)
    {
        throw new NotImplementedException();
    }
}

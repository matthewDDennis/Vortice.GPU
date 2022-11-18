// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using Vortice.Vulkan;

namespace Vortice.GPU.Vulkan;

internal unsafe class VulkanUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VkFormat ToVulkan(PixelFormat format)
    {
        return VkFormat.Undefined;
    }
}

// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using Vortice.Vulkan;

namespace Vortice.GPU.Vulkan;

internal unsafe class VulkanUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VkFormat ToVulkan(PixelFormat format)
    {
        switch (format)
        {
            // 8-bit formats
            case PixelFormat.R8Unorm:               return VkFormat.R8UNorm;
            case PixelFormat.R8Snorm:               return VkFormat.R8SNorm;
            case PixelFormat.R8Uint:                return VkFormat.R8UInt;
            case PixelFormat.R8Sint:                return VkFormat.R8SInt;
            // 16-bit formats
            case PixelFormat.R16Uint:               return VkFormat.R16UInt;
            case PixelFormat.R16Sint:               return VkFormat.R16SInt;
            case PixelFormat.R16Unorm:              return VkFormat.R16UNorm;
            case PixelFormat.R16Snorm:              return VkFormat.R16SNorm;
            case PixelFormat.R16Float:              return VkFormat.R16SFloat;
            case PixelFormat.RG8Unorm:              return VkFormat.R8G8UNorm;
            case PixelFormat.RG8Snorm:              return VkFormat.R8G8SNorm;
            case PixelFormat.RG8Uint:               return VkFormat.R8G8UInt;
            case PixelFormat.RG8Sint:               return VkFormat.R8G8SInt;
            // Packed 16-Bit Pixel Formats
            case PixelFormat.BGRA4Unorm:            return VkFormat.B4G4R4A4UNormPack16;
            case PixelFormat.B5G6R5Unorm:           return VkFormat.B5G6R5UNormPack16;
            case PixelFormat.B5G5R5A1Unorm:         return VkFormat.B5G5R5A1UNormPack16;
            // 32-bit formats
            case PixelFormat.R32Uint:               return VkFormat.R32UInt;
            case PixelFormat.R32Sint:               return VkFormat.R32SInt;
            case PixelFormat.R32Float:              return VkFormat.R32SFloat;
            case PixelFormat.RG16Uint:              return VkFormat.R16G16UInt;
            case PixelFormat.RG16Sint:              return VkFormat.R16G16SInt;
            case PixelFormat.RG16Unorm:             return VkFormat.R16G16UNorm;
            case PixelFormat.RG16Snorm:             return VkFormat.R16G16SNorm;
            case PixelFormat.RG16Float:             return VkFormat.R16G16SFloat;
            case PixelFormat.RGBA8Unorm:            return VkFormat.R8G8B8A8UNorm;
            case PixelFormat.RGBA8UnormSrgb:        return VkFormat.R8G8B8A8SRgb;
            case PixelFormat.RGBA8Snorm:            return VkFormat.R8G8B8A8SNorm;
            case PixelFormat.RGBA8Uint:             return VkFormat.R8G8B8A8UInt;
            case PixelFormat.RGBA8Sint:             return VkFormat.R8G8B8A8SInt;
            case PixelFormat.BGRA8Unorm:            return VkFormat.B8G8R8A8UNorm;
            case PixelFormat.BGRA8UnormSrgb:        return VkFormat.B8G8R8A8SRgb;
            // Packed 32-Bit formats
            case PixelFormat.RGB9E5Ufloat:          return VkFormat.E5B9G9R9UFloatPack32;
            case PixelFormat.RGB10A2Unorm:          return VkFormat.A2B10G10R10UNormPack32;
            case PixelFormat.RGB10A2Uint:           return VkFormat.A2R10G10B10UIntPack32;
            case PixelFormat.RG11B10Float:          return VkFormat.B10G11R11UFloatPack32;
            // 64-Bit formats
            case PixelFormat.RG32Uint:              return VkFormat.R32G32UInt;
            case PixelFormat.RG32Sint:              return VkFormat.R32G32SInt;
            case PixelFormat.RG32Float:             return VkFormat.R32G32SFloat;
            case PixelFormat.RGBA16Uint:            return VkFormat.R16G16B16A16UInt;
            case PixelFormat.RGBA16Sint:            return VkFormat.R16G16B16A16SInt;
            case PixelFormat.RGBA16Unorm:           return VkFormat.R16G16B16A16UNorm;
            case PixelFormat.RGBA16Snorm:           return VkFormat.R16G16B16A16SNorm;
            case PixelFormat.RGBA16Float:           return VkFormat.R16G16B16A16SFloat;
            // 128-Bit formats
            case PixelFormat.RGBA32Uint:            return VkFormat.R32G32B32A32UInt;
            case PixelFormat.RGBA32Sint:            return VkFormat.R32G32B32A32SInt;
            case PixelFormat.RGBA32Float:           return VkFormat.R32G32B32A32SFloat;
            // Depth-stencil formats
            case PixelFormat.Depth16Unorm:          return VkFormat.D16UNorm;
            case PixelFormat.Depth32Float:          return VkFormat.D32SFloat;
            case PixelFormat.Stencil8:              return VkFormat.S8UInt;
            case PixelFormat.Depth24UnormStencil8:  return VkFormat.D24UNormS8UInt;
            case PixelFormat.Depth32FloatStencil8:  return VkFormat.D32SFloatS8UInt;
            // Compressed BC formats
            case PixelFormat.BC1RgbaUnorm:          return VkFormat.BC1RGBAUNormBlock;
            case PixelFormat.BC1RgbaUnormSrgb:      return VkFormat.BC1RGBASRgbBlock;
            case PixelFormat.BC2RgbaUnorm:          return VkFormat.BC2UNormBlock;
            case PixelFormat.BC2RgbaUnormSrgb:      return VkFormat.BC2SRgbBlock;
            case PixelFormat.BC3RgbaUnorm:          return VkFormat.BC3UNormBlock;
            case PixelFormat.BC3RgbaUnormSrgb:      return VkFormat.BC3SRgbBlock;
            case PixelFormat.BC4RUnorm:             return VkFormat.BC4UNormBlock;
            case PixelFormat.BC4RSnorm:             return VkFormat.BC4SNormBlock;
            case PixelFormat.BC5RgUnorm:            return VkFormat.BC5UNormBlock;
            case PixelFormat.BC5RgSnorm:            return VkFormat.BC5SNormBlock;
            case PixelFormat.BC6HRgbUfloat:         return VkFormat.BC6HUFloatBlock;
            case PixelFormat.BC6HRgbSfloat:         return VkFormat.BC6HSFloatBlock;
            case PixelFormat.BC7RgbaUnorm:          return VkFormat.BC7UNormBlock;
            case PixelFormat.BC7RgbaUnormSrgb:      return VkFormat.BC7SRgbBlock;
            // EAC/ETC compressed formats
            case PixelFormat.Etc2Rgb8Unorm:         return VkFormat.ETC2R8G8B8UNormBlock;
            case PixelFormat.Etc2Rgb8UnormSrgb:     return VkFormat.ETC2R8G8B8SRgbBlock;
            case PixelFormat.Etc2Rgb8A1Unorm:       return VkFormat.ETC2R8G8B8A1UNormBlock;
            case PixelFormat.Etc2Rgb8A1UnormSrgb:   return VkFormat.ETC2R8G8B8A1SRgbBlock;
            case PixelFormat.Etc2Rgba8Unorm:        return VkFormat.ETC2R8G8B8A8UNormBlock;
            case PixelFormat.Etc2Rgba8UnormSrgb:    return VkFormat.ETC2R8G8B8A8SRgbBlock;
            case PixelFormat.EacR11Unorm:           return VkFormat.EACR11UNormBlock;
            case PixelFormat.EacR11Snorm:           return VkFormat.EACR11SNormBlock;
            case PixelFormat.EacRg11Unorm:          return VkFormat.EACR11G11UNormBlock;
            case PixelFormat.EacRg11Snorm:          return VkFormat.EACR11G11SNormBlock;
            // ASTC compressed formats
            case PixelFormat.ASTC4x4Unorm:          return VkFormat.ASTC4x4UNormBlock;
            case PixelFormat.ASTC4x4UnormSrgb:      return VkFormat.ASTC4x4SRgbBlock;
            case PixelFormat.ASTC5x4Unorm:          return VkFormat.ASTC5x4UNormBlock;
            case PixelFormat.ASTC5x4UnormSrgb:      return VkFormat.ASTC5x4SRgbBlock;
            case PixelFormat.ASTC5x5Unorm:          return VkFormat.ASTC5x5UNormBlock;
            case PixelFormat.ASTC5x5UnormSrgb:      return VkFormat.ASTC5x5SRgbBlock;
            case PixelFormat.ASTC6x5Unorm:          return VkFormat.ASTC6x5UNormBlock;
            case PixelFormat.ASTC6x5UnormSrgb:      return VkFormat.ASTC6x5SRgbBlock;
            case PixelFormat.ASTC6x6Unorm:          return VkFormat.ASTC6x6UNormBlock;
            case PixelFormat.ASTC6x6UnormSrgb:      return VkFormat.ASTC6x6SRgbBlock;
            case PixelFormat.ASTC8x5Unorm:          return VkFormat.ASTC8x5UNormBlock;
            case PixelFormat.ASTC8x5UnormSrgb:      return VkFormat.ASTC8x5SRgbBlock;
            case PixelFormat.ASTC8x6Unorm:          return VkFormat.ASTC8x6UNormBlock;
            case PixelFormat.ASTC8x6UnormSrgb:      return VkFormat.ASTC8x6SRgbBlock;
            case PixelFormat.ASTC8x8Unorm:          return VkFormat.ASTC8x8UNormBlock;
            case PixelFormat.ASTC8x8UnormSrgb:      return VkFormat.ASTC8x8SRgbBlock;
            case PixelFormat.ASTC10x5Unorm:         return VkFormat.ASTC10x5UNormBlock;
            case PixelFormat.ASTC10x5UnormSrgb:     return VkFormat.ASTC10x5SRgbBlock;
            case PixelFormat.ASTC10x6Unorm:         return VkFormat.ASTC10x6UNormBlock;
            case PixelFormat.ASTC10x6UnormSrgb:     return VkFormat.ASTC10x6SRgbBlock;
            case PixelFormat.ASTC10x8Unorm:         return VkFormat.ASTC10x8UNormBlock;
            case PixelFormat.ASTC10x8UnormSrgb:     return VkFormat.ASTC10x8SRgbBlock;
            case PixelFormat.ASTC10x10Unorm:        return VkFormat.ASTC10x10UNormBlock;
            case PixelFormat.ASTC10x10UnormSrgb:    return VkFormat.ASTC10x10SRgbBlock;
            case PixelFormat.ASTC12x10Unorm:        return VkFormat.ASTC12x10UNormBlock;
            case PixelFormat.ASTC12x10UnormSrgb:    return VkFormat.ASTC12x10SRgbBlock;
            case PixelFormat.ASTC12x12Unorm:        return VkFormat.ASTC12x12UNormBlock;
            case PixelFormat.ASTC12x12UnormSrgb:    return VkFormat.ASTC12x12SRgbBlock;

            //case PixelFormat::R8BG8Biplanar420Unorm:      return VkFormat.G8_B8R8_2PLANE_420UNorm;

            default:
                return VkFormat.Undefined;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static VkFormat ToVkFormat(VertexFormat format)
    {
        switch (format)
        {
            case VertexFormat.UByte2:           return VkFormat.R8G8UInt;
            case VertexFormat.UByte4:           return VkFormat.R8G8B8A8UInt;
            case VertexFormat.Byte2:            return VkFormat.R8G8SInt;
            case VertexFormat.Byte4:            return VkFormat.R8G8B8A8SInt;
            case VertexFormat.UByte2Norm:       return VkFormat.R8G8UNorm;
            case VertexFormat.UByte4Norm:       return VkFormat.R8G8B8A8UNorm;
            case VertexFormat.Byte2Norm:        return VkFormat.R8G8SNorm;
            case VertexFormat.Byte4Norm:        return VkFormat.R8G8B8A8SNorm;
            case VertexFormat.UShort2:          return VkFormat.R16G16UInt;
            case VertexFormat.UShort4:          return VkFormat.R16G16B16A16UInt;
            case VertexFormat.Short2:           return VkFormat.R16G16SInt;
            case VertexFormat.Short4:           return VkFormat.R16G16B16A16SInt;
            case VertexFormat.UShort2Norm:      return VkFormat.R16G16UNorm;
            case VertexFormat.UShort4Norm:      return VkFormat.R16G16B16A16UNorm;
            case VertexFormat.Short2Norm:       return VkFormat.R16G16SNorm;
            case VertexFormat.Short4Norm:       return VkFormat.R16G16B16A16SNorm;
            case VertexFormat.Half2:            return VkFormat.R16G16SFloat;
            case VertexFormat.Half4:            return VkFormat.R16G16B16A16SFloat;
            case VertexFormat.Float:            return VkFormat.R32SFloat;
            case VertexFormat.Float2:           return VkFormat.R32G32SFloat;
            case VertexFormat.Float3:           return VkFormat.R32G32B32SFloat;
            case VertexFormat.Float4:           return VkFormat.R32G32B32A32SFloat;
            case VertexFormat.UInt:             return VkFormat.R32UInt;
            case VertexFormat.UInt2:            return VkFormat.R32G32UInt;
            case VertexFormat.UInt3:            return VkFormat.R32G32B32UInt;
            case VertexFormat.UInt4:            return VkFormat.R32G32B32A32UInt;
            case VertexFormat.Int:              return VkFormat.R32SInt;
            case VertexFormat.Int2:             return VkFormat.R32G32SInt;
            case VertexFormat.Int3:             return VkFormat.R32G32B32SInt;
            case VertexFormat.Int4:             return VkFormat.R32G32B32A32SInt;
            case VertexFormat.RGB10A2UNorm:     return VkFormat.A2B10G10R10UNormPack32;

            default:
                return ThrowHelper.ThrowArgumentException<VkFormat>("Invalid VertexFormat value");
        }
    }

}

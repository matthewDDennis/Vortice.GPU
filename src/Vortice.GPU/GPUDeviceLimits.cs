// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;

namespace Vortice.GPU;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct GPUDeviceLimits
{
    public readonly uint MaxTextureDimension1D;
    public readonly uint MaxTextureDimension2D;
    public readonly uint MaxTextureDimension3D;
    public readonly uint MaxTextureDimensionCube;
    public readonly uint MaxTextureArrayLayers;
    public readonly ulong MaxUniformBufferBindingSize;
    public readonly ulong MaxStorageBufferBindingSize;
    public readonly uint MinUniformBufferOffsetAlignment;
    public readonly uint MinStorageBufferOffsetAlignment;
    public readonly uint MaxVertexBuffers;
    public readonly uint MaxVertexAttributes;
    public readonly uint MaxVertexBufferArrayStride;
    public readonly uint MaxComputeWorkgroupStorageSize;
    public readonly uint MaxComputeInvocationsPerWorkGroup;
    public readonly uint MaxComputeWorkGroupSizeX;
    public readonly uint MaxComputeWorkGroupSizeY;
    public readonly uint MaxComputeWorkGroupSizeZ;
    public readonly uint MaxComputeWorkGroupsPerDimension;
    public readonly uint MaxViewports;
    public fixed uint MaxViewportDimensions[2];
    public readonly uint MaxColorAttachments;
}


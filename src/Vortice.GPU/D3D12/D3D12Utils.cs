// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using Win32.Graphics.Direct3D12;

namespace Vortice.GPU.D3D12;

internal static class D3D12Utils
{
    public static readonly HeapProperties DefaultHeapProps = new (HeapType.Default);
    public static readonly HeapProperties UploadHeapProps = new (HeapType.Upload);
    public static readonly HeapProperties ReadbackeapProps = new(HeapType.Readback);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CommandListType ToD3D12(CommandQueueType type)
    {
        switch (type)
        {
            case CommandQueueType.Compute:
                return CommandListType.Compute;

            case CommandQueueType.Copy:
                return CommandListType.Copy;

            case CommandQueueType.Graphics:
            default:
                return CommandListType.Direct;
        }
    }
}

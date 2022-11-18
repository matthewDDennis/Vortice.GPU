// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using static Win32.Apis;
using static Vortice.GPU.D3D12.D3D12Utils;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12CommandQueue : CommandQueue
{
    private readonly ComPtr<ID3D12CommandQueue> _handle;

    public D3D12CommandQueue(D3D12Device device, CommandQueueType type)
        : base(device)
    {
        CommandQueueDescription desc = new()
        {
            Type = ToD3D12(type),
            Priority = (int)CommandQueuePriority.Normal,
            Flags = CommandQueueFlags.None,
            NodeMask = 0
        };

        device.Handle->CreateCommandQueue(
            &desc,
            __uuidof<ID3D12CommandQueue>(),
            _handle.GetVoidAddressOf())
            .ThrowIfFailed();

        switch (type)
        {
            case CommandQueueType.Compute:
                _label = "Compute CommandQueue";
                break;

            case CommandQueueType.Copy:
                _label = "Copy CommandQueue";
                break;

            default:
                _label = "Graphics CommandQueue";
                break;
        }

        fixed (char* labelPtr = _label)
        {
            _handle.Get()->SetName((ushort*)labelPtr);
        }
    }

    // <summary>
    /// Finalizes an instance of the <see cref="D3D12Buffer" /> class.
    /// </summary>
    ~D3D12CommandQueue() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _handle.Dispose();
        }
    }

    public ID3D12CommandQueue* Handle => _handle;
}

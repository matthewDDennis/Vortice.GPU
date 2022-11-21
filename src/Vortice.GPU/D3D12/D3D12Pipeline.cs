// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Win32;
using Win32.Graphics.Direct3D12;
using Win32.Graphics.Dxgi.Common;
using static Vortice.GPU.D3D12.D3D12Utils;
using static Vortice.GPU.D3DUtils;
using static Win32.Apis;

namespace Vortice.GPU.D3D12;

internal unsafe class D3D12Pipeline : Pipeline
{
    public readonly ID3D12PipelineState* Handle;

    public D3D12Pipeline(D3D12Device device, in RenderPipelineDescription description)
        : base(device, PipelineType.Render, description.Label)
    {
        var desc = new GraphicsPipelineStateDescription()
        {
            
        };
        ID3D12PipelineState* newPipeline;
        HResult hr = device.Handle->CreateGraphicsPipelineState(&desc,
            __uuidof<ID3D12PipelineState>(),
            (void**)&newPipeline);
        hr.ThrowIfFailed();
        Handle = newPipeline;
    }

    // <summary>
    /// Finalizes an instance of the <see cref="D3D12Pipeline" /> class.
    /// </summary>
    ~D3D12Pipeline() => Dispose(disposing: false);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Handle->Release();
        }
    }

    protected override void OnLabelChanged(string newLabel)
    {
        fixed (char* labelPtr = newLabel)
        {
            Handle->SetName((ushort*)labelPtr);
        }
    }
}

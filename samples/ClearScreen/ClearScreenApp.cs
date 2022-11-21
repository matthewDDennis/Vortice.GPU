// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;
using System.Numerics;
using Vortice.GPU.VertexTypes;
using Win32.Numerics;

namespace Vortice.GPU;

public static class Program
{
    class ClearScreenApp : Application
    {
        private GPUBuffer? _vertexBuffer;
        private Pipeline? _renderPipeline;

        public ClearScreenApp()
            : base(nameof(ClearScreenApp))
        {
        }

        protected override void Initialize()
        {
            ReadOnlySpan<VertexPositionColor> triangleVertices = stackalloc VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0f, 0.5f, 0.0f), new Color4(1.0f, 0.0f, 0.0f, 1.0f)),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.0f), new Color4(0.0f, 1.0f, 0.0f, 1.0f)),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.0f), new Color4(0.0f, 0.0f, 1.0f, 1.0f))
            };

            _vertexBuffer = GPUDevice.CreateBuffer(triangleVertices, BufferUsage.Vertex);

            //RenderPipelineDescription pipelineDescription = new()
            //{
            //    Label = "Triangle"
            //};
            //_renderPipeline = GPUDevice.CreateRenderPipeline(pipelineDescription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vertexBuffer?.Dispose();
                _renderPipeline?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnTick()
        {
            CommandBuffer commandBuffer = GPUDevice.GraphicsQueue.BeginCommandBuffer();
            Texture? swapChainTexture = commandBuffer.AcquireSwapchainTexture(MainWindow.SwapChain!);
            if (swapChainTexture != null)
            {
                RenderPassColorAttachment colorAttachment = new()
                {
                    Texture = swapChainTexture,
                    ClearColor = Color.CornflowerBlue
                };

                commandBuffer.BeginRenderPass(colorAttachment);
                commandBuffer.EndRenderPass();
            }

            commandBuffer.Commit();
        }
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    //[STAThread]
    public static void Main()
    {
        using ClearScreenApp game = new();
        game.Run();
    }
}

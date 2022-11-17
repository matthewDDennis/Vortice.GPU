// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public static class Program
{
    class ClearScreenApp : Application
    {
        public ClearScreenApp()
            : base(nameof(ClearScreenApp))
        {
        }

        //protected override void Initialize()
        //{
        //    //using GraphicsDevice device = new(ValidationMode.Enabled);
        //    //using GraphicsBuffer vertexBuffer = GraphicsBuffer.CreateBuffer(device, new BufferDescription(64, BufferUsage.Vertex));
        //}
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

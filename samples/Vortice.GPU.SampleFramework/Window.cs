// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Drawing;
using static Vortice.GPU.GLFW;

namespace Vortice.GPU;

[Flags]
public enum WindowFlags
{
    None = 0,
    Fullscreen = 1 << 0,
    FullscreenDesktop = 1 << 1,
    Hidden = 1 << 2,
    Borderless = 1 << 3,
    Resizable = 1 << 4,
    Minimized = 1 << 5,
    Maximized = 1 << 6,
}

public sealed unsafe class Window : ISwapChainSurface
{
    private readonly GLFWwindow* _window;

    public Window(string title, int width, int height, WindowFlags flags = WindowFlags.None)
    {
        Title = title;

        bool fullscreen = false;
        GLFWmonitor* monitor = null;
        if ((flags & WindowFlags.Fullscreen) != WindowFlags.None)
        {
            monitor = glfwGetPrimaryMonitor();
            fullscreen = true;
        }

        if ((flags & WindowFlags.FullscreenDesktop) != WindowFlags.None)
        {
            monitor = glfwGetPrimaryMonitor();
            //auto mode = glfwGetVideoMode(monitor);
            //
            //glfwWindowHint(GLFW_RED_BITS, mode->redBits);
            //glfwWindowHint(GLFW_GREEN_BITS, mode->greenBits);
            //glfwWindowHint(GLFW_BLUE_BITS, mode->blueBits);
            //glfwWindowHint(GLFW_REFRESH_RATE, mode->refreshRate);

            glfwWindowHint(WindowHintBool.Decorated, false);
            fullscreen = true;
        }

        if (!fullscreen)
        {
            if ((flags & WindowFlags.Borderless) != WindowFlags.None)
            {
                glfwWindowHint(WindowHintBool.Decorated, false);
            }
            else
            {
                glfwWindowHint(WindowHintBool.Decorated, true);
            }

            if ((flags & WindowFlags.Resizable) != WindowFlags.None)
            {
                glfwWindowHint(WindowHintBool.Resizable, true);
            }

            if ((flags & WindowFlags.Hidden) != WindowFlags.None)
            {
                glfwWindowHint(WindowHintBool.Visible, false);
            }

            if ((flags & WindowFlags.Minimized) != WindowFlags.None)
            {
                glfwWindowHint(WindowHintBool.Iconified, true);
            }

            if ((flags & WindowFlags.Maximized) != WindowFlags.None)
            {
                glfwWindowHint(WindowHintBool.Maximized, true);
            }
        }

        _window = glfwCreateWindow(width, height, title, monitor);
        //Handle = hwnd;

        glfwGetWindowSize(_window, out width, out height);
        ClientSize = new(width, height);

        if (OperatingSystem.IsWindows())
        {
            Kind = SwapChainSurfaceKind.Win32;
            SurfaceHandle = glfwGetWin32Window(_window);
        }
    }

    public void Dispose()
    {
        SwapChain?.Dispose();

        glfwDestroyWindow(_window);
    }

    public string Title { get; }
    public Size ClientSize { get; }

    #region ISwapChainSurface Members
    public SwapChainSurfaceKind Kind { get; }

    Size ISwapChainSurface.Size => ClientSize;

    public nint SurfaceDisplay { get; }
    public nint SurfaceHandle { get; }

    public bool IsFullscreen { get; private set; }
    #endregion

    public bool ShoudClose => glfwWindowShouldClose(_window);
    public SwapChain? SwapChain { get; private set; }

    public void CreateSwapChain(GPUDevice device)
    {
        SwapChainDescription description = new();
        SwapChain = device.CreateSwapChain(this, description);
    }
}

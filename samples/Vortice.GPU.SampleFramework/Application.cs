// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vortice.Vulkan;
using Win32.Graphics.Direct3D;
using static Vortice.GPU.GLFW;

namespace Vortice.GPU;

public abstract class Application : IDisposable
{
#if NET6_0_OR_GREATER
    private volatile uint _isDisposed = 0;
#else
    private volatile int _isDisposed = 0;
#endif

    private static readonly glfwErrorCallback s_errorCallback = GlfwError;

    /// <summary>
    /// Initializes a new instance of the <see cref="GPUObject" /> class.
    /// </summary>
    protected Application(string name)
    {
        if (!glfwInit())
        {
            throw new PlatformNotSupportedException("GLFW is not supported");
        }

        glfwSetErrorCallback(s_errorCallback);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            glfwInitHint(InitHintBool.CocoaChDirResources, false);
        }

        glfwWindowHint((int)WindowHintClientApi.ClientApi, 0);

        ValidationMode validationMode = ValidationMode.Disabled;
#if DEBUG
        validationMode = ValidationMode.Enabled;
#endif
        GPUDevice = GPUDevice.CreateDefault(validationMode);

        // Create main window.
        Name = name;
        MainWindow = new Window(name, 1280, 720);
    }

    public string Name { get; }

    public GPUDevice GPUDevice { get; }

    public Window MainWindow { get; }

    /// <summary>
    /// Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.
    /// </summary>
    public bool IsDisposed => _isDisposed != 0;

    /// <inheritdoc />
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="disposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            GPUDevice.WaitIdle();
            MainWindow.Dispose();
            GPUDevice.Dispose();
            glfwTerminate();
        }
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void OnTick()
    {
    }


    public void Run()
    {
        InitializeBeforeRun();

        while (!MainWindow.ShoudClose)
        {
            OnTick();
            glfwPollEvents();
        }
    }

    /// <summary>Throws an exception if the object has been disposed.</summary>
    /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ThrowIfDisposed()
    {
        if (_isDisposed != 0)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    private void InitializeBeforeRun()
    {
        MainWindow.CreateSwapChain(GPUDevice);

        Initialize();
    }

    private static unsafe void GlfwError(int code, IntPtr message)
    {
        throw new Exception(Interop.GetString((byte*)message));
    }
}

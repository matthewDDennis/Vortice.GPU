// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Reflection;
using System.Runtime.InteropServices;

namespace Vortice.GPU;

internal static class LibraryLoader
{
    private static string GetOSPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "win";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "linux";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "osx";

        throw new ArgumentException("Unsupported OS platform.");
    }

    private static string GetArchitecture()
    {
        switch (RuntimeInformation.ProcessArchitecture)
        {
            case Architecture.X86: return "x86";
            case Architecture.X64: return "x64";
            case Architecture.Arm: return "arm";
            case Architecture.Arm64: return "arm64";
        }

        throw new ArgumentException("Unsupported architecture.");
    }

    public static nint LoadLibrary(string libraryName)
    {
        string libraryPath = GetNativeAssemblyPath(libraryName);

        IntPtr handle = LoadPlatformLibrary(libraryPath);
        if (handle == IntPtr.Zero)
            throw new DllNotFoundException($"Unable to load library '{libraryName}'.");

        return handle;

        static string GetNativeAssemblyPath(string libraryName)
        {
            string osPlatform = GetOSPlatform();
            string architecture = GetArchitecture();

            string assemblyLocation = Assembly.GetExecutingAssembly() != null ? Assembly.GetExecutingAssembly().Location : typeof(LibraryLoader).Assembly.Location;
            assemblyLocation = Path.GetDirectoryName(assemblyLocation);

            string[] paths = new[]
            {
                Path.Combine(assemblyLocation, libraryName),
                Path.Combine(assemblyLocation, "runtimes", osPlatform, "native", libraryName),
                Path.Combine(assemblyLocation, "runtimes", $"{osPlatform}-{architecture}", "native", libraryName),
                Path.Combine(assemblyLocation, "native", $"{osPlatform}-{architecture}", libraryName),
            };

            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return libraryName;
        }
    }

    public static T LoadFunction<T>(nint library, string name)
    {
        nint symbol = NativeLibrary.GetExport(library, name);

        return Marshal.GetDelegateForFunctionPointer<T>(symbol);
    }

    private static nint LoadPlatformLibrary(string libraryName)
    {
        return NativeLibrary.Load(libraryName);
    }

    public static nint GetSymbol(nint library, string symbolName)
    {
        return NativeLibrary.GetExport(library, symbolName);
    }
}

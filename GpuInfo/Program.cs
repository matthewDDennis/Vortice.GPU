// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using Vortice.GPU;


var gpuDevice = GPUDevice.CreateDefault();

Console.WriteLine("==== GPU Properties ====");
Console.WriteLine($"Name: {gpuDevice.AdapterProperties.Name}");
Console.WriteLine($"Description: {gpuDevice.AdapterProperties.DriverDescription}");
Console.WriteLine($"VendorId: {gpuDevice.AdapterProperties.VendorId}");
Console.WriteLine($"DeviceId: {gpuDevice.AdapterProperties.DeviceId}");
Console.WriteLine($"AdapterType: {gpuDevice.AdapterProperties.AdapterType}");

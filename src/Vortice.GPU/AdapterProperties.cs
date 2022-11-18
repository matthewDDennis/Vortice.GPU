// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

namespace Vortice.GPU;

public readonly struct AdapterProperties
{
    public required uint VendorId { get; init; }
    public required uint DeviceId { get; init; }
    public required string Name { get; init; }
    public required string DriverDescription { get; init; }
    public required AdapterType AdapterType { get; init; }
}


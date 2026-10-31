namespace HomeNet.Applications.BlazorApp.Devices;

public sealed class Device
{
    public required string Name { get; set; }
    public IReadOnlyCollection<DeviceState>? States { get; set; }
}

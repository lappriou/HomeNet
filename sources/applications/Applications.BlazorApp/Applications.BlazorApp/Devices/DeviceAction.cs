namespace HomeNet.Applications.BlazorApp.Devices;

public class DeviceAction
{
    public required string Name { get; set; }

    public required Device Device { get; set; }

    public required Action<Device> Action { get; set; }
}

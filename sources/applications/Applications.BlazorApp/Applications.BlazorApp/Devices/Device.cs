namespace HomeNet.Applications.BlazorApp.Devices;

public sealed class Device
{
    public required string Name { get; set; }
    public IReadOnlyCollection<State>? States { get; set; }
}

namespace HomeNet.Applications.BlazorApp.Devices;

public class State
{
    public required string Name { get; set; }

    public required IObservable<object> ValueObservable { get; init; }

    public object? Value { get; private set; }
}

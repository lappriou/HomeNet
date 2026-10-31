namespace HomeNet.Applications.BlazorApp.Devices;

public class State
{
    public required string Name { get; set; }

    public required Func<IObservable<object>> GetValueObservable { get; init; }

    public object? Value { get; private set; }
}

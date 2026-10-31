namespace HomeNet.Applications.BlazorApp.Devices;

public sealed class DeviceStateBuilder
{
    private IObservable<object>? ValueObservable{ get; set; }

    public DeviceStateBuilder WithValueObservable(IObservable<object> valueObservable)
    {
        ValueObservable = valueObservable;
        return this;
    }

    public DeviceState Build(string name)
    {
        var state = new DeviceState
        {
            Name = name,
        };

        ValueObservable?.Subscribe(state.ValueSubject);
        return state;
    }
}

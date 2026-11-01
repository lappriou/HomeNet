namespace HomeNet.Applications.BlazorApp.Devices;

public sealed class DeviceStateBuilder
{
    private IObservable<object?>? ValueObservable { get; set; }

    public object? ValueDefault { get; set; }

    public DeviceStateBuilder WithValueObservable(IObservable<object?> valueObservable)
    {
        ValueObservable = valueObservable;
        return this;
    }

    public DeviceStateBuilder WithValueDefault(object valueDefault)
    {
        ValueDefault = valueDefault;
        return this;
    }

    public DeviceState Build(string name)
    {
        var state = new DeviceState
        {
            Name = name,
        };

        if (ValueDefault != null)
        {
            state.Value = ValueDefault;
        }

        if (ValueObservable != null)
        {
            state.SubscribeToValue(ValueObservable);
        }

        return state;
    }
}

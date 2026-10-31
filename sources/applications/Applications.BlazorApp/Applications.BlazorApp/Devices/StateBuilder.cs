namespace HomeNet.Applications.BlazorApp.Devices;

public sealed class StateBuilder
{
    public State Build(string name, IObservable<object> valueObservable)
    {
        return new State
        {
            Name = name,
            ValueObservable = valueObservable
        };
    }
}

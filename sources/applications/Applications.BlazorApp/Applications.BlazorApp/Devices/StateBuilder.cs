namespace HomeNet.Applications.BlazorApp.Devices;

public sealed class StateBuilder
{
    public State Build(string name, IObservable<object> valueObservable)
    {
        var state = new State
        {
            Name = name,
        };

        valueObservable.Subscribe(state.ValueSubject);
        return state;
    }
}

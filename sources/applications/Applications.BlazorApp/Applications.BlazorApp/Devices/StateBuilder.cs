namespace HomeNet.Applications.BlazorApp.Devices;

public sealed class StateBuilder
{
    private IObservable<object>? ValueObservable{ get; set; }

    public StateBuilder WithValueObservable(IObservable<object> valueObservable)
    {
        ValueObservable = valueObservable;
        return this;
    }

    public State Build(string name)
    {
        var state = new State
        {
            Name = name,
        };

        ValueObservable?.Subscribe(state.ValueSubject);
        return state;
    }
}

using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HomeNet.Applications.BlazorApp.Devices;

public class DeviceState : IDisposable
{
    private readonly Subject<object?> _valueSubject = new();
    private readonly CompositeDisposable _subscriptions = new();

    public required string Name { get; set; }

    public IObservable<object?> ValueObservable => _valueSubject.AsObservable();

    public object? Value
    {
        get => field;
        set
        {
            field = value;
            _valueSubject.OnNext(value);
        }
    }

    internal void SubscribeToValue(IObservable<object?> valueObservable)
    {
        _subscriptions.Add(valueObservable.Subscribe(value => Value = value));
    }

    public void Dispose()
    {
        _subscriptions.Dispose();
        _valueSubject.Dispose();
    }
}

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HomeNet.Applications.BlazorApp.Devices;

public class DeviceState : IDisposable
{
    public required string Name { get; set; }

    public Subject<object> ValueSubject { get; } = new Subject<object>();
    public IObservable<object> ValueObservable => ValueSubject.AsObservable();

    public void Dispose()
    {
        ValueSubject.Dispose();
    }
}

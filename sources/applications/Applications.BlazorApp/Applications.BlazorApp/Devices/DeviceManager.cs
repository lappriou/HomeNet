using System.Reactive.Linq;

namespace HomeNet.Applications.BlazorApp.Devices;

public class DeviceManager
{
    public DeviceManager()
    {
        var devices = new List<Device>
        {
            new Device
            {
                Name = "Temperature sensor",
                States = new List<State>
                {
                    new State
                    {
                        Name = "Temperature",
                        GetValueObservable = () => Observable.Interval(TimeSpan.FromSeconds(1))
                            .Select(_ => (object)$"{new Random().NextInt64(19, 24)}°C")
                    },
                }
            },
        };
        Devices = devices;
    }
    public IReadOnlyCollection<Device> Devices { get; private set; } = new List<Device>();
}

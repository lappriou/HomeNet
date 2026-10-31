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
                            .Select(_ => $"{new Random().NextInt64(19, 24)}°C")
                    },
                }
            },
            new Device
            {
                Name = "Light",
                States = new List<State>
                {
                    new State
                    {
                        Name = "On/Off",
                        GetValueObservable = () => Observable.Interval(TimeSpan.FromSeconds(1))
                            .Select(_ => new Random().NextInt64(0,2) == 1 ? "On" : "Off")
                    },
                }
            },
        };
        Devices = devices;
    }
    public IReadOnlyCollection<Device> Devices { get; private set; } = new List<Device>();
}

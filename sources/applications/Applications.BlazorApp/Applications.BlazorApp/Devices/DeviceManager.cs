using System.Reactive.Linq;

namespace HomeNet.Applications.BlazorApp.Devices;

public class DeviceManager
{
    public DeviceManager()
    {
        var devices = new List<Device>
        {
            BuildDeviceSensorTemperature("Temperature sensor 1"),
            BuildDeviceLight("Light 1"),
        };
        Devices = devices;
    }
    public IReadOnlyCollection<Device> Devices { get; private set; } = new List<Device>();

    private Device BuildDeviceSensorTemperature(string name)
    {
        return new Device
        {
            Name = "Temperature sensor",
            States = new List<State>
                {
                    new State
                    {
                        Name = "Temperature",
                        ValueObservable = Observable.Interval(TimeSpan.FromSeconds(1))
                            .Select(_ => $"{new Random().NextInt64(19, 24)}°C")
                    },
                }
        };
    }

    private Device BuildDeviceLight(string name)
    {
        return new Device
        {
            Name = "Light",
            States = new List<State>
                {
                    new State
                    {
                        Name = "On/Off",
                        ValueObservable = Observable.Interval(TimeSpan.FromSeconds(1))
                            .Select(_ => new Random().NextInt64(0,2) == 1 ? "On" : "Off")
                    },
                }
        };
    }
}

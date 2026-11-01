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
            States = new List<DeviceState>
                {
                    new DeviceStateBuilder()
                    .WithValueObservable(Observable.Interval(TimeSpan.FromSeconds(1))
                            .Select(_ => $"{new Random().NextInt64(19, 24)}°C"))
                    .Build("Temperature")
                }
        };
    }

    private Device BuildDeviceLight(string name)
    {
        var device = new Device
        {
            Name = name,
            States = new List<DeviceState>
                {
                    new DeviceStateBuilder()
                        .Build(name: "On/Off"),
                }
        };

        device.Actions = new List<DeviceAction>
        {
            
            new DeviceAction
            {
                Name = "Turn On",
                Device = device,
                Action = (device) => 
                {
                    device.States?.FirstOrDefault(x => x.Name == "On/Off")?.ValueSubject.OnNext("On");
                }
            },
            new DeviceAction
            {
                Name = "Turn Off",
                Device = device,
                Action = (device) =>
                {
                    device.States?.FirstOrDefault(x => x.Name == "On/Off")?.ValueSubject.OnNext("Off");
                }
            }
        };

        return device;
    }
}

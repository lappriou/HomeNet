using System.Reactive.Linq;

namespace HomeNet.Cores.Devices;

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
        return new DeviceBuilder(name)
            .WithState(new DeviceStateBuilder()
                .WithValueObservable(Observable.Interval(TimeSpan.FromSeconds(20))
                    .Select(_ => $"{new Random().NextInt64(19, 24)}°C"))
                .WithValueDefault("20°C")
                .Build("Temperature"))
            .Build();
    }

    private Device BuildDeviceLight(string name)
    {
        return new DeviceBuilder(name)
            .WithState(new DeviceStateBuilder()
                .WithValueDefault("Off")
                .Build(name: "On/Off"))
            .WithAction("Turn On", (device) =>
            {
                var state = device.States?.FirstOrDefault(x => x.Name == "On/Off");
                if (state != null)
                {
                    state.Value = "On";
                }
            })
            .WithAction("Turn Off", (device) =>
            {
                var state = device.States?.FirstOrDefault(x => x.Name == "On/Off");
                if (state != null)
                {
                    state.Value = "Off";
                }
            })
            .Build();
    }
}

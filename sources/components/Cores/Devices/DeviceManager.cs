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
            .WithOnOff()
            .WithState(new DeviceStateBuilder()
                .WithValueDefault("0%")
                .Build(name: "Brightness"))
            .WithAction<SetBrightnessParameters>("Set brightness", (device, parameters) =>
            {
                if (parameters.Percentage is < 0 or > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(parameters), "Brightness must be between 0 and 100.");
                }

                var state = device.States?.FirstOrDefault(x => x.Name == "Brightness");
                if (state != null)
                {
                    state.Value = $"{parameters.Percentage}%";
                }
            })
            .Build();
    }
}

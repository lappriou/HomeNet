namespace HomeNet.Cores.Devices;

internal class DeviceBuilder
{
    private Device Device { get; set; }

    private List<DeviceState> States { get; set; } = new List<DeviceState>();
    private List<IDeviceAction> Actions { get; set; } = new List<IDeviceAction>();

    public DeviceBuilder(string name)
    {
        Device = new Device
        {
            Name = name,
        };
    }

    public DeviceBuilder WithState(DeviceState state)
    {
        States.Add(state);
        return this;
    }

    public DeviceBuilder WithAction(string name, Action<Device> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return WithAction(name, device =>
        {
            action(device);
            return Task.CompletedTask;
        });
    }

    public DeviceBuilder WithAction(string name, Func<Device, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        Actions.Add(DeviceAction<object?>.WithoutParameters(name, Device, action));
        return this;
    }

    public DeviceBuilder WithAction<TParameters>(string name, Action<Device, TParameters> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return WithAction<TParameters>(name, (device, parameters) =>
        {
            action(device, parameters);
            return Task.CompletedTask;
        });
    }

    public DeviceBuilder WithAction<TParameters>(string name, Func<Device, TParameters, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        Actions.Add(new DeviceAction<TParameters>(name, Device, action));
        return this;
    }

    public DeviceBuilder WithOnOff()
    {
        this.WithState(new DeviceStateBuilder()
                .WithValueDefault("Off")
                .Build(name: "On/Off"))
            .WithAction("Toggle", (device) =>
            {
                var state = device.States?.FirstOrDefault(x => x.Name == "On/Off");
                if (state != null)
                {
                    state.Value = (string?)state.Value == "On" ? "Off" : "On";
                }
            });

        return this;
    }

    public Device Build()
    {
        Device.States = States;
        Device.Actions = Actions;
        return Device;
    }
}

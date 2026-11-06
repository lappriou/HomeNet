namespace HomeNet.Cores.Devices;

internal class DeviceBuilder
{
    private Device Device { get; set; }

    private List<DeviceState> States { get; set; } = new List<DeviceState>();
    private List<DeviceAction> Actions { get; set; } = new List<DeviceAction>();
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
        Actions.Add(new DeviceAction
        {
            Name = name,
            Device = Device,
            Action = action
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

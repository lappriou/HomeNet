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
                        GetValue = () => "25.0 °C"
                    },
                }
            },
        };
        Devices = devices;
    }
    public IReadOnlyCollection<Device> Devices { get; private set; } = new List<Device>();
}

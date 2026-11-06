namespace HomeNet.Cores.Devices;

public interface IDeviceAction
{
    string Name { get; }
    Device Device { get; }
    Type? ParametersType { get; }
    bool RequiresParameters { get; }
    Task ExecuteAsync(object? parameters = null);
}

public sealed class DeviceAction<TParameters> : IDeviceAction
{
    private readonly Func<Device, TParameters, Task> _action;
    private readonly bool _requiresParameters;

    public string Name { get; }
    public Device Device { get; }
    public Type? ParametersType => _requiresParameters ? typeof(TParameters) : null;
    public bool RequiresParameters => _requiresParameters;

    public DeviceAction(string name, Device device, Action<Device, TParameters> action)
        : this(name, device, (device, parameters) =>
        {
            action(device, parameters);
            return Task.CompletedTask;
        })
    {
    }

    public DeviceAction(string name, Device device, Func<Device, TParameters, Task> action)
        : this(name, device, action, requiresParameters: true)
    {
    }

    private DeviceAction(string name, Device device, Func<Device, TParameters, Task> action, bool requiresParameters)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(device);
        ArgumentNullException.ThrowIfNull(action);

        Name = name;
        Device = device;
        _action = action;
        _requiresParameters = requiresParameters;
    }

    internal static DeviceAction<object?> WithoutParameters(string name, Device device, Func<Device, Task> action) =>
        new(name, device, (target, _) => action(target), requiresParameters: false);

    public async Task ExecuteAsync(object? parameters = null)
    {
        if (!_requiresParameters)
        {
            if (parameters is not null)
            {
                throw new ArgumentException("This action does not accept parameters.", nameof(parameters));
            }

            await _action(Device, default!).ConfigureAwait(false);
            return;
        }

        if (parameters is not TParameters typedParameters)
        {
            throw new ArgumentException(
                $"Expected parameters of type '{typeof(TParameters).Name}'.",
                nameof(parameters));
        }

        await _action(Device, typedParameters).ConfigureAwait(false);
    }
}

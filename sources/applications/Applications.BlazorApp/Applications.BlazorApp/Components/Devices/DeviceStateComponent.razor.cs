using HomeNet.Cores.Devices;

using Microsoft.AspNetCore.Components;

namespace HomeNet.Applications.BlazorApp.Components.Devices;

public partial class DeviceStateComponent : IDisposable
{
    [Parameter]
    public DeviceState State { get; set; } = default!;

    private object? Value { get; set; }
    private IDisposable? _subscription;

    protected override void OnInitialized()
    {
        Value = State.Value;
        _subscription = State.ValueObservable
            .Subscribe(value =>
            {
                Value = value;
                InvokeAsync(StateHasChanged);
            });
    }

    public void Dispose()
    {
        _subscription?.Dispose();
    }
}

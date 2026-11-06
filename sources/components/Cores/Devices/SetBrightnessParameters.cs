using System.ComponentModel.DataAnnotations;

namespace HomeNet.Cores.Devices;

public sealed class SetBrightnessParameters
{
    [Display(Name = "Brightness (%)")]
    [Range(0, 100)]
    public int Percentage { get; set; } = 50;
}

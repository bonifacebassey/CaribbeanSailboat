using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Components;

public enum IndicatorColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Orange,
    Default
}

public partial class Indicator
{
    [Parameter]
    public IndicatorColor Color { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public string? Width { get; set; }

    private string Activate => Color switch
    {
        IndicatorColor.Red => "led-red",
        IndicatorColor.Green => "led-green",
        IndicatorColor.Blue => "led-blue",
        IndicatorColor.Yellow => "led-yellow",
        IndicatorColor.Orange => "led-orange",
        _ => "",
    };
}

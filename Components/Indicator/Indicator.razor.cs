using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Components.Indicator;

public enum IndicatorColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Orange,
    Default
}

public enum Align
{
    Start,
    Center,
    End
}

public partial class Indicator
{
    [Parameter]
    public IndicatorColor Color { get; set; }

    [Parameter]
    public Align AlignItem { get; set; } = Align.Center;

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

    private string Alignment => AlignItem switch
    {
        Align.Start => "start",
        Align.Center => "center",
        Align.End => "end",
        _ => "",
    };
}

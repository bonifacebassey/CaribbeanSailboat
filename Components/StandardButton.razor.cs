using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Components;

public partial class StandardButton
{
    [Parameter]
    public EventCallback Click { get; set; }

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public bool IsEnabled { get; set; } = true;

    [Parameter]
    public string? CssStyle { get; set; }

    [Parameter]
    public string? Color { get; set; }

    [Parameter]
    public string? Background { get; set; }

    [Parameter]
    public string? Radius { get; set; }

    protected void OnClick()
    {
        if (Click.HasDelegate)
        {
            _ = Click.InvokeAsync();
        }
    }
}

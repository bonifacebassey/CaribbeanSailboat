using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Components;

public partial class Switch
{
    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public string OffText { get; set; } = "Off";

    [Parameter]
    public string OnText { get; set; } = "On";

    private bool? isChecked = false;

    private void OnValueChanged(ChangeEventArgs e)
    {
        isChecked = e.Value as bool?;

        if (ValueChanged.HasDelegate && isChecked.HasValue)
        {
            ValueChanged.InvokeAsync(isChecked.Value);
        }
    }
}

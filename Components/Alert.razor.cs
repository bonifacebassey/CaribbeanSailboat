using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Components;

public enum AlertType
{
    Error,
    Warning,
    Question,
    Success,
    Info
}

public partial class Alert
{
    [Parameter]
    public EventCallback Close { get; set; }

    [Parameter]
    public string? Message { get; set; }

    [Parameter]
    public AlertType AlertType { get; set; }

    private string AlertColor => AlertType switch
    {
        AlertType.Error => "var(--error)",
        AlertType.Warning => "var(--warning)",
        AlertType.Question => "var(--alert)",
        AlertType.Success => "var(--success)",
        AlertType.Info => "var(--secondary1)",
        _ => ""
    };

    private string AlertIcon => AlertType switch
    {
        AlertType.Error => "&#11198;",
        AlertType.Warning => "&#10007;",
        AlertType.Question => "&#63;",
        AlertType.Success => "&#10003;",
        AlertType.Info => "&#8505;",
        _ => ""
    };

    private void OnClose()
    {
        if (Close.HasDelegate)
        {
            _ = Close.InvokeAsync(this);
        }
    }
}

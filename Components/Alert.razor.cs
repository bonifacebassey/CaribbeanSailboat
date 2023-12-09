using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Components;

public enum AlertType
{
    Error,
    Warning,
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
        AlertType.Warning => "var(--alert)",
        AlertType.Success => "var(--success)",
        AlertType.Info => "var(--secondary1)",
        _ => ""
    };

    private string AlertIcon => AlertType switch
    {
        AlertType.Error => "svg/x-circle.svg",
        AlertType.Warning => "svg/question-circle.svg",
        AlertType.Success => "svg/check-circle.svg",
        AlertType.Info => "svg/exclamation-circle.svg",
        _ => ""
    };

    // private string AlertIcon => AlertType switch
    // {
    //     AlertType.Error => "&#11198;",
    //     AlertType.Warning => "&#xF505;",
    //     AlertType.Success => "&#xF26B;",
    //     AlertType.Info => "&#xF333;",
    //     _ => "wwwroot/images/icon.svg"
    // };

    private void OnClose()
    {
        if (Close.HasDelegate)
        {
            _ = Close.InvokeAsync(this);
        }
    }
}

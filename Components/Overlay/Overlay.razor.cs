using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Components.Overlay;

public partial class Overlay
{
    private bool showOverlay = false;
    private string cssClass = "";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public void Show()
    {
        showOverlay = true;
        cssClass = "overlay show";
        StateHasChanged();
    }

    public void Hide()
    {
        cssClass = "overlay hide";
        showOverlay = false;
        StateHasChanged();
    }
}

using CaribbeanSailboat.Components.Overlay;

namespace CaribbeanSailboat.Pages.Index;

public partial class Index
{
    private Overlay? overlay;


    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            overlay?.Show();
        }
    }
}
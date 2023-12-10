using CaribbeanSailboat.Components.Indicator;
using CaribbeanSailboat.Database;

namespace CaribbeanSailboat.Shared;

public partial class NavMenu
{
    private OracleDbContext? dbContext;
    Timer? timer;
    private IndicatorColor dbLEDColor = IndicatorColor.Red;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            dbContext = OracleDbContext.Get();
            timer = new Timer(async _ => await CheckDatabaseState(), null, 0, 5000);
        }
    }

    private async Task CheckDatabaseState()
    {
        bool isDbRunning = false;
        if (dbContext != null)
        {
            isDbRunning = await dbContext.IsDatabaseRunningAsync();
        }

        dbLEDColor = isDbRunning ? IndicatorColor.Green : IndicatorColor.Red;
        await InvokeAsync(StateHasChanged);
    }
}

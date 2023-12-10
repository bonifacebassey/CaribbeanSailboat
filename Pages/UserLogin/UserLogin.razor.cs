using CaribbeanSailboat.Components.Alert;
using CaribbeanSailboat.Components.Overlay;
using CaribbeanSailboat.Database;
using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Pages.UserLogin;

public partial class UserLogin
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    private Overlay? overlay;

    private string username = "";
    private string password = "";
    private bool showPassword = false;
    private string message = "";
    private AlertType alertType = AlertType.Info;

    private async Task OnLogin()
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            message = "Missing login details? Please provide both username and password to LOGIN!";
            alertType = AlertType.Warning;
            overlay?.Show();
            return;
        }

        if (CheckUserExists())
        {
            await Task.Delay(1000);

            // Navigate to welcome page
            NavigationManager?.NavigateTo("/welcome");
        }
    }


    private bool CheckUserExists()
    {
        var employeeModel = DbModel.Get().EmployeeModel();
        var dbEmployeeModel = employeeModel.CreateItem();

        if (dbEmployeeModel.Validate(username, password))
        {
            message = $"Welcome back {username}! Enjoy your day at Caribbean Sailboat Charters.";
            alertType = AlertType.Success;
            overlay?.Show();

            return true;
        }
        else
        {
            message = $"Login failed for {username}! Enusre your Username/Password are entered correctly OR contact C.S.Charter to register an account with us.";
            alertType = AlertType.Error;
            overlay?.Show();
            return false;
        }
    }
}

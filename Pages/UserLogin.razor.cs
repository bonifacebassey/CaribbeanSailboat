using CaribbeanSailboat.Components;
using CaribbeanSailboat.Database;
using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Pages;

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

    private void OnLogin()
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            message = "Missing login details? Please provide both username and password to LOGIN!";
            alertType = AlertType.Question;
            overlay?.Show();
            return;
        }

        ValidateUser();
        // TODO Query username and password: Navigate to mainpage on success
        // Condition to be replaced by result of username and password verification. Just for test
        //if (username.Equals(password))
        //{
        //    NavigateToMainPage();
        //}
    }

    private void ValidateUser()
    {
        var employeeModel = DbModel.Get().EmployeeModel();
        var dbEmployeeModel = employeeModel.CreateItem();
        var employee = dbEmployeeModel.FindBy("boniface");

        if (employee != null && employee.Username != null)
        {
            //if (user.Validate(username, password))
            if (employee.Username.Trim().ToLower() == username.Trim().ToLower())
            {
                message = $"Welcome back {username}! Enjoy your day at Caribbean Sailboat Charters.";
                alertType = AlertType.Success;
                overlay?.Show();
            }
            else
            {
                message = $"Login failed for {username}! Enusre your Username/Password are entered correctly OR contact C.S.Charter to register an account with us.";
                alertType = AlertType.Error;
                overlay?.Show();
            }
        }
    }

    private void NavigateToMainPage()
    {
        NavigationManager?.NavigateTo("/welcome");
    }
}

﻿using CaribbeanSailboat.Components;
using Microsoft.AspNetCore.Components;

namespace CaribbeanSailboat.Pages;

public partial class Login
{
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

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
            message = "Missing login details? Please provice both username and password to LOGIN!";
            alertType = AlertType.Question;
            overlay?.Show();
            return;
        }

        // TODO Query username and password: Navigate to mainpage on success
        // Condition to be replaced by result of username and password verification. Just for test
        if (username.Equals(password))
        {
            NavigateToMainPage();
        }
    }

    private void NavigateToMainPage()
    {
        NavigationManager.NavigateTo("/mainpage");
    }
}

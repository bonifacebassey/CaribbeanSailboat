using CaribbeanSailboat.Components;
using CaribbeanSailboat.Database;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using CaribbeanSailboat.Database.ModelImpl;

namespace CaribbeanSailboat.Pages;

public partial class Rental
{
    private Overlay? overlay;

    private List<IBoat>? boats;
    private ICustomer? customer;

    private bool toggleCustomer = true;

    private string selectedBoatName = "";
    private IBoat? selectedBoat = new BoatImpl(new Boat
    {
        Name = "",
        Size = ""
    });


    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            var boatModel = DbModel.Get().BoatModel();
            var dbBoatModel = boatModel.CreateItem();
            boats = dbBoatModel.GetAllBoats();
            selectedBoat = boats.FirstOrDefault();
            selectedBoatName = selectedBoat?.Name ?? string.Empty;

            StateHasChanged();
        }
    }

    private void FindOrAddCustomer()
    {

        var customerModel = DbModel.Get().CustomerModel();
        var dbCustomerModel = customerModel.CreateItem();
        customer = dbCustomerModel.GetCustomerByEmail("John.Doe@gmail.com");
    }


    private string message = "";
    private AlertType alertType = AlertType.Info;
    private void FindBoat()
    {
        var boat = boats?.FirstOrDefault(boat => boat.Name?.ToLower() == selectedBoatName.ToLower());
        if (boat != null)
        {
            selectedBoat = boat;
            StateHasChanged();
        }
        else
        {
            message = $"Please select from available Boats: {string.Join(", ", boats?.Select(boat => boat?.Name) ?? Array.Empty<string>())}";
            alertType = AlertType.Error;
            overlay?.Show();
        }
    }

    private void SubmitCharter()
    {

    }

}

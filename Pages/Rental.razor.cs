using CaribbeanSailboat.Components;
using CaribbeanSailboat.Database;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;

namespace CaribbeanSailboat.Pages;

public partial class Rental
{
    private Overlay? overlay;

    private Customer customer = new();
    private Boat boat = new();
    private int charterId = 0;
    private List<IBoat>? boats;
    private bool existingCustomer = true;
    private bool isReturnCharter = false;
    private DateTime charterStartDate = DateTime.Today;
    private DateTime charterEndDate = DateTime.Today;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            // First fetch all boats from the database
            var boatModel = DbModel.Get().BoatModel();
            var dbBoatModel = boatModel.CreateItem();

            boats = dbBoatModel.GetAllBoats();
            var selectedBoat = boats.FirstOrDefault();
            if (selectedBoat != null)
            {
                boat = new()
                {
                    BoatId = selectedBoat.BoatId,
                    OwnerId = selectedBoat.OwnerId,
                    Name = selectedBoat.Name,
                    Size = selectedBoat.Size,
                    RentalCost = selectedBoat.RentalCost
                };
            }
            StateHasChanged();
        }
    }

    private void ToggleCharter()
    {
        isReturnCharter = !isReturnCharter;
        StateHasChanged();
    }

    /* FIND or ADD Customer
    ***************************************************/
    private void FindOrAddCustomer()
    {
        var customerModel = DbModel.Get()
                                    .CustomerModel()
                                    .CreateItem();

        if (existingCustomer)
        {
            // This is an EXISTING customer
            FindCustomer(customerModel);
        }
        else
        {
            // This is a NEW customer
            AddCustomer(customerModel);
        }
    }

    /* FIND an existing Customer
    ***************************************************/
    private void FindCustomer(ICustomer model)
    {
        if (existingCustomer)
        {
            if (!string.IsNullOrEmpty(customer.Email))
            {
                // Check if customer exist in the database
                var dbCustomer = model.GetCustomerByEmail(customer?.Email ?? "");
                if (dbCustomer != null && !string.IsNullOrEmpty(dbCustomer.Email))
                {
                    customer = new()
                    {
                        CustomerId = dbCustomer.CustomerId,
                        FirstName = dbCustomer.FirstName,
                        LastName = dbCustomer.LastName,
                        Email = dbCustomer.Email,
                        Balance = dbCustomer.Balance
                    };
                    message = $"CUSTOMER found. Welcome Back [{customer.FirstName} {customer.LastName}]";
                    alertType = AlertType.Success;
                    overlay?.Show();
                    StateHasChanged();
                    return;
                }
            }
            message = $"Enter an existing CUSTOMER's email address or create new if not exist!";
            alertType = AlertType.Info;
            overlay?.Show();
        }
    }

    /* ADD a new Customer
    ***************************************************/
    private void AddCustomer(ICustomer model)
    {
        // This is a NEW customer
        if (!existingCustomer)
        {
            if (!string.IsNullOrEmpty(customer?.FirstName) &&
                !string.IsNullOrEmpty(customer?.LastName) &&
                !string.IsNullOrEmpty(customer?.Email))
            {
                // Add NEW customer to the database
                var customerId = model.AddCustomer(customer);
                if (customer.CustomerId != -1)
                {
                    customer.CustomerId = customerId;
                    message = $"New customer [{customer.FirstName} {customer.LastName}] added successfully";
                    alertType = AlertType.Success;
                    overlay?.Show();
                    StateHasChanged();
                    return;
                }
            }

            message = $"New CUSTOMER [{customer?.FirstName ?? ""} {customer?.LastName}] not created";
            alertType = AlertType.Info;
            overlay?.Show();
        }
    }

    /* Find BOAT
    ***************************************************/
    private string message = "";
    private AlertType alertType = AlertType.Info;
    private void FindBoat()
    {
        var dbBoat = boats?.FirstOrDefault(b => b.Name?.ToLower() == boat.Name?.ToLower());
        if (dbBoat != null)
        {
            boat = new()
            {
                BoatId = dbBoat.BoatId,
                OwnerId = dbBoat.OwnerId,
                Name = dbBoat.Name,
                Size = dbBoat.Size,
                RentalCost = dbBoat.RentalCost
            };
            StateHasChanged();

            message = $"Boat [{boat.Name}] is available";
            alertType = AlertType.Success;
            overlay?.Show();
        }
        else
        {
            message = $"Boat [{boat.Name}] is unavailable. CHOOSE either... {string.Join(", ", boats?.Select(boat => boat?.Name) ?? Array.Empty<string>())}";
            alertType = AlertType.Error;
            overlay?.Show();
        }
    }


    /* Submit request to ADD or RETURN Charter
    ***************************************************/
    private void SubmitRequest()
    {
        var charterModel = DbModel.Get()
                                    .CharterModel()
                                    .CreateItem();

        if (isReturnCharter)
        {
            ReturnCharter(charterModel);
        }
        else
        {
            AddCharter(charterModel);
        }
    }

    private void AddCharter(ICharter model)
    {
        var charterId = model.AddCharter(customer.CustomerId, boat.OwnerId, charterStartDate, charterEndDate);
        if (charterId != -1)
        {
            message = $"Add charter successful. Charter ID: {charterId}";
            alertType = AlertType.Success;
            overlay?.Show();
        }
        else
        {
            message = $"Add charter failed";
            alertType = AlertType.Error;
            overlay?.Show();
        }
    }

    private void ReturnCharter(ICharter model)
    {
        var result = model.ReturnCharter(charterId);
        if (result)
        {
            message = $"Return charter successful. Charter ID: {charterId}";
            alertType = AlertType.Success;
            overlay?.Show();
        }
        else
        {
            message = $"Return charter failed";
            alertType = AlertType.Error;
            overlay?.Show();
        }
    }
}

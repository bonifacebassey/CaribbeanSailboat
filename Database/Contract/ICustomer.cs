namespace CaribbeanSailboat.Database.Contract;

public interface ICustomer : ICrudObject<ICustomer>
{
    int CustomerId { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    string? Email { get; set; }
    double Balance { get; set; }

    ICustomer AddCustomer(ICustomer customer);
    ICustomer GetCustomerByEmail(string email);
}

namespace CaribbeanSailboat.Database.Contract;

public interface IEmployee : ICrudObject<IEmployee>
{
    int Id { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    string? Email { get; set; }
    string? Username { get; set; }
    string? Password { get; set; }
    DateTime JoinDate { get; set; }

    IEmployee FindBy(string usernameOrId);
    bool Validate(string username, string password);
}

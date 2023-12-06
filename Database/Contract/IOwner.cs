namespace CaribbeanSailboat.Database.Contract;

public interface IOwner : ICrudObject<IOwner>
{
    int OwnerId { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    string? Email { get; set; }
    DateTime JoinDate { get; set; }

    IOwner FindBy(int id);
}

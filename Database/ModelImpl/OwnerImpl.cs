using Oracle.ManagedDataAccess.Client;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using System.Data;

namespace CaribbeanSailboat.Database.ModelImpl;

public class OwnerImpl : IOwner
{
    public OwnerImpl(Owner owner)
    {
        Owner = owner;
    }

    public Owner Owner { get; set; }
    public int OwnerId { get => Owner.OwnerId; set => Owner.OwnerId = value; }
    public string? FirstName { get => Owner.FirstName; set => Owner.FirstName = value; }
    public string? LastName { get => Owner.LastName; set => Owner.LastName = value; }
    public string? Email { get => Owner.Email; set => Owner.Email = value; }
    public DateTime JoinDate { get => Owner.JoinDate; set => Owner.JoinDate = value; }

    public IOwner CreateItem()
    {
        return new OwnerImpl(new Owner());
    }

    public void AddItem(IOwner owner)
    {
        throw new NotImplementedException();
    }

    public void DeleteItem(IOwner owner)
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(IOwner owner)
    {
        throw new NotImplementedException();
    }

    public IOwner FindBy(int id)
    {
        throw new NotImplementedException();
    }
}

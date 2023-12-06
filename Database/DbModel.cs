using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using CaribbeanSailboat.Database.ModelImpl;


namespace CaribbeanSailboat.Database;

public class DbModel
{
    private static readonly Lazy<DbModel> lazy = new Lazy<DbModel>(() => new DbModel());

    private DbModel()
    {
    }

    public static DbModel Get()
    {
        return lazy.Value;
    }

    public IEmployee EmployeeModel()
    {
        return new EmployeeImpl(new Employee());
    }

    public IBoat BoatModel()
    {
        return new BoatImpl(new Boat());
    }

    public ICharter CharterModel()
    {
        return new CharterImpl(new Charter());
    }

    public ICustomer CustomerModel()
    {
        return new CustomerImpl(new Customer());
    }

    public IOwner OwnerModel()
    {
        return new OwnerImpl(new Owner());
    }
}

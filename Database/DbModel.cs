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
}

using Oracle.ManagedDataAccess.Client;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using System.Data;

namespace CaribbeanSailboat.Database.ModelImpl;

public class CharterImpl : ICharter
{
    public CharterImpl(Charter charter)
    {
        Charter = charter;
    }

    public Charter Charter { get; set; }
    public int CharterId { get => Charter.CharterId; set => Charter.CharterId = value; }
    public int CustomerId { get => Charter.CustomerId; set => Charter.CustomerId = value; }
    public DateTime StartDate { get => Charter.StartDate; set => Charter.StartDate = value; }
    public DateTime EndDate { get => Charter.EndDate; set => Charter.EndDate = value; }

    public ICharter CreateItem()
    {
        return new CharterImpl(new Charter());
    }

    public void AddItem(ICharter charter)
    {
        throw new NotImplementedException();
    }

    public void DeleteItem(ICharter charter)
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(ICharter charter)
    {
        throw new NotImplementedException();
    }
}

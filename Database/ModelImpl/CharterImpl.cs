using Oracle.ManagedDataAccess.Client;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using System.Data;
using Oracle.ManagedDataAccess.Types;

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

    public int AddCharter(int CustomerId, int BoatId, DateTime startDate, DateTime endDate)
    {
        try
        {
            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Add_Charter";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("IN_BOAT_ID", OracleDbType.Int32).Value = BoatId;
                    command.Parameters.Add("IN_StartDate", OracleDbType.Date).Value = startDate;
                    command.Parameters.Add("IN_EndDate", OracleDbType.Date).Value = endDate;
                    command.Parameters.Add("IN_CUST_ID", OracleDbType.Int32).Value = CustomerId;
                    command.Parameters.Add("OUT_RESULT", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    OracleDecimal outputValue = (OracleDecimal)command.Parameters["OUT_RESULT"].Value;
                    return outputValue.IsNull ? 0 : outputValue.ToInt32();
                }
            }
        }
        catch { }

        return -1;
    }

    public bool ReturnCharter(int charterId)
    {
        try
        {
            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Return_Charter";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("IN_CHARTER_ID", OracleDbType.Int32).Value = charterId;
                    command.Parameters.Add("OUT_RESULT", OracleDbType.Boolean).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    return ((OracleBoolean)command.Parameters["OUT_RESULT"].Value).Value;
                }
            }
        }
        catch { }

        return false;
    }
}

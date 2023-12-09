
using Oracle.ManagedDataAccess.Client;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using System.Data;
using Oracle.ManagedDataAccess.Types;

namespace CaribbeanSailboat.Database.ModelImpl;

public class CustomerImpl : ICustomer
{
    public CustomerImpl(Customer customer)
    {
        Customer = customer;
    }

    public Customer Customer { get; set; }
    public int CustomerId { get => Customer.CustomerId; set => Customer.CustomerId = value; }
    public string? FirstName { get => Customer.FirstName; set => Customer.FirstName = value; }
    public string? LastName { get => Customer.LastName; set => Customer.LastName = value; }
    public string? Email { get => Customer.Email; set => Customer.Email = value; }
    public double Balance { get => Customer.Balance; set => Customer.Balance = value; }

    public ICustomer CreateItem()
    {
        return new CustomerImpl(new Customer());
    }

    public void AddItem(ICustomer customer)
    {
        throw new NotImplementedException();
    }

    public void DeleteItem(ICustomer customer)
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(ICustomer customer)
    {
        throw new NotImplementedException();
    }

    public int AddCustomer(Customer customer)
    {
        int customerId = -1;
        try
        {
            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Add_Customer";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("IN_CUST_FNAME", OracleDbType.Varchar2).Value = customer.FirstName;
                    command.Parameters.Add("IN_CUST_LNAME", OracleDbType.Varchar2).Value = customer.LastName;
                    command.Parameters.Add("IN_CUST_EMAIL", OracleDbType.Varchar2).Value = customer.Email;
                    command.Parameters.Add("OUT_RESULT", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    OracleDecimal outputValue = (OracleDecimal)command.Parameters["OUT_RESULT"].Value;
                    customerId = outputValue.IsNull ? 0 : outputValue.ToInt32();
                }
            }
        }
        catch { }
        return customerId;
    }

    public ICustomer GetCustomerByEmail(string email)
    {
        Customer customer = new();
        try
        {
            using (var connection = OracleDbContext.Get().Connection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CUSTOMER WHERE CUST_EMAIL = :email";
                command.Parameters.Add(new OracleParameter(":email", OracleDbType.Varchar2, email, ParameterDirection.Input));

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CUST_ID")),
                            FirstName = reader["CUST_FNAME"] is DBNull ? null : reader.GetString(reader.GetOrdinal("CUST_FNAME")),
                            LastName = reader["CUST_LNAME"] is DBNull ? null : reader.GetString(reader.GetOrdinal("CUST_LNAME")),
                            Email = reader["CUST_EMAIL"] is DBNull ? null : reader.GetString(reader.GetOrdinal("CUST_EMAIL")),
                            Balance = reader.GetDouble(reader.GetOrdinal("CUST_BALANCE"))
                        };
                    }
                }
            }
        }
        catch { }

        return new CustomerImpl(customer);
    }
}

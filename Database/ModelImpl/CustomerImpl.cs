
using Dapper;
using Oracle.ManagedDataAccess.Client;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using System;
using System.Data;

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

    public ICustomer AddCustomer(ICustomer customer)
    {
        var newCustomer = new Customer();
        try
        {
            using (var connection = OracleDbContext.Get().Connection())
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                // Define the PL/SQL block
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "BEGIN :result := ADD_CUSTOMER(:firstname, :lastname, :email, :balance); END;";

                // Add input parameters
                command.Parameters.Add("firstname", OracleDbType.Varchar2).Value = customer.FirstName;
                command.Parameters.Add("lastname", OracleDbType.Varchar2).Value = customer.LastName;
                command.Parameters.Add("email", OracleDbType.Varchar2).Value = customer.Email;
                command.Parameters.Add("balance", OracleDbType.Double).Value = customer.Balance;

                // Add output parameter for the function result (CUSTOMER_ID)
                var outputParameter = new OracleParameter("result", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(outputParameter);

                // Execute the PL/SQL block
                command.ExecuteNonQuery();

                // Retrieve the function result from the output parameter
                int customerId = Convert.ToInt32(outputParameter.Value);

                // Create and return a new customer with the provided details and updated CustomerId
                newCustomer = new Customer
                {
                    CustomerId = customerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Balance = customer.Balance
                };
            }
        }
        catch { }
        return new CustomerImpl(newCustomer);
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

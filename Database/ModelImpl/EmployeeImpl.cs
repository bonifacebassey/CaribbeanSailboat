using Oracle.ManagedDataAccess.Client;
using CaribbeanSailboat.Database.Contract;
using CaribbeanSailboat.Database.Model;
using System.Data;

namespace CaribbeanSailboat.Database.ModelImpl;

public class EmployeeImpl : IEmployee, ICrudObject<IEmployee>
{
    public EmployeeImpl(Employee employee)
    {
        Employee = employee;
    }

    public Employee Employee { get; set; }
    public int Id { get => Employee.Id; set => Employee.Id = value; }
    public string? FirstName { get => Employee.FirstName ?? string.Empty; set => Employee.FirstName = value; }
    public string? LastName { get => Employee.LastName ?? string.Empty; set => Employee.LastName = value; }
    public string? Email { get => Employee.Email ?? string.Empty; set => Employee.Email = value; }
    public string? Username { get => Employee.Username ?? string.Empty; set => Employee.Username = value; }
    public string? Password { get => Employee.Password ?? string.Empty; set => Employee.Password = value; }
    public DateTime JoinDate { get => Employee.JoinDate; set => Employee.JoinDate = value; }

    public IEmployee CreateItem()
    {
        return new EmployeeImpl(new Employee());
    }

    public void AddItem(IEmployee user)
    {
        try
        {
            string insertSql = "INSERT INTO EMPLOYEE (" +
                                        "EMPLOYEE_FNAME," +
                                        "EMPLOYEE_LNAME," +
                                        "EMPLOYEE_EMAIL," +
                                        "EMPLOYEE_USERNAME," +
                                        "EMPLOYEE_PASSWORD," +
                                        "EMPLOYEE_JOIN_DATE) " +
                                "VALUES (:FirstName, :LastName, :Email, :Username, :Password, :JoinDate)";

            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand(insertSql, connection))
                {
                    // Add parameters using the properties of the User object
                    command.Parameters.Add(new OracleParameter(":Id", OracleDbType.Int32, user.Id, ParameterDirection.Input));
                    command.Parameters.Add(new OracleParameter(":FirstName", OracleDbType.Varchar2, user.FirstName, ParameterDirection.Input));
                    command.Parameters.Add(new OracleParameter(":LastName", OracleDbType.Varchar2, user.LastName, ParameterDirection.Input));
                    command.Parameters.Add(new OracleParameter(":Email", OracleDbType.Varchar2, user.Email, ParameterDirection.Input));
                    command.Parameters.Add(new OracleParameter(":Username", OracleDbType.Varchar2, user.Username, ParameterDirection.Input));
                    command.Parameters.Add(new OracleParameter(":Password", OracleDbType.Varchar2, user.Password, ParameterDirection.Input));
                    command.Parameters.Add(new OracleParameter(":JoinDate", OracleDbType.Date, user.JoinDate, ParameterDirection.Input));

                    // Execute the INSERT command
                    command.ExecuteNonQuery();

                    Console.WriteLine("User inserted successfully!");
                }
            }
        }
        catch { }
    }

    public void UpdateItem(IEmployee employee)
    {
        string updateSql = "UPDATE EMPLOYEE" +
                            "SET  EMPLOYEE_FNAME = :FirstName," +
                                    "EMPLOYEE_LNAME = :LastName," +
                                    "EMPLOYEE_EMAIL = :Email," +
                                    "EMPLOYEE_USERNAME = :Username," +
                                    "EMPLOYEE_PASSWORD = :Password," +
                                    "EMPLOYEE_JOIN_DATE = :JoinDate " +
                            "WHERE EMPLOYEE_ID = :Id";

        using (var connection = OracleDbContext.Get().Connection())
        {
            connection.Open();

            using (OracleCommand command = new OracleCommand(updateSql, connection))
            {
                // Add parameters using the properties of the User object
                command.Parameters.Add(new OracleParameter(":Id", OracleDbType.Int32, employee.Id, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(":FirstName", OracleDbType.Varchar2, employee.FirstName, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(":LastName", OracleDbType.Varchar2, employee.LastName, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(":Email", OracleDbType.Varchar2, employee.Email, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(":Username", OracleDbType.Varchar2, employee.Username, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(":Password", OracleDbType.Varchar2, employee.Password, ParameterDirection.Input));
                command.Parameters.Add(new OracleParameter(":JoinDate", OracleDbType.Date, employee.JoinDate, ParameterDirection.Input));

                // Execute the UPDATE command
                command.ExecuteNonQuery();

                Console.WriteLine("User updated successfully!");
            }
        }
    }

    public void DeleteItem(IEmployee employee)
    {
        try
        {
            string deleteSql = "DELETE FROM EMPLOYEE WHERE EMPLOYEE_ID = :Id";

            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand(deleteSql, connection))
                {
                    // Add parameter for UserId
                    command.Parameters.Add(new OracleParameter(":Id", OracleDbType.Int32, employee.Id, System.Data.ParameterDirection.Input));

                    // Execute the DELETE command
                    command.ExecuteNonQuery();

                    Console.WriteLine("User deleted successfully!");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions or log them if needed
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public IEmployee FindBy(string usernameOrId)
    {
        try
        {
            string selectSql = "SELECT * FROM EMPLOYEE WHERE EMPLOYEE_ID = :Id OR EMPLOYEE_USERNAME = :Username";

            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand(selectSql, connection))
                {
                    // Add parameters for UserId and Username
                    command.Parameters.Add(new OracleParameter(":Id", OracleDbType.Int32));
                    command.Parameters.Add(new OracleParameter(":Username", OracleDbType.Varchar2));

                    // Determine whether the input is a UserId or Username
                    if (int.TryParse(usernameOrId, out int userId))
                    {
                        command.Parameters[":Id"].Value = userId;
                        command.Parameters[":Username"].Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters[":Id"].Value = DBNull.Value;
                        command.Parameters[":Username"].Value = usernameOrId;
                    }

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Map the data from the database to a new User object
                            return new EmployeeImpl(new Employee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EMPLOYEE_ID")),
                                FirstName = reader.GetString(reader.GetOrdinal("EMPLOYEE_FNAME")),
                                LastName = reader.GetString(reader.GetOrdinal("EMPLOYEE_LNAME")),
                                Email = reader.GetString(reader.GetOrdinal("EMPLOYEE_EMAIL")),
                                Username = reader.GetString(reader.GetOrdinal("EMPLOYEE_USERNAME")),
                                Password = reader.GetString(reader.GetOrdinal("EMPLOYEE_PASSWORD")),
                                JoinDate = reader.GetDateTime(reader.GetOrdinal("EMPLOYEE_JOIN_DATE"))
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions or log them if needed
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return new EmployeeImpl(new Employee());
    }

    public bool Validate(string username, string password)
    {
        try
        {
            string functionSql = "BEGIN :result := CHECK_USER_EXIST(:username, :password); END;";

            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand(functionSql, connection))
                {
                    // Add parameters for the function call
                    command.Parameters.Add(new OracleParameter(":result", OracleDbType.Boolean, ParameterDirection.ReturnValue));
                    command.Parameters.Add(new OracleParameter(":username", OracleDbType.Varchar2, username, ParameterDirection.Input));
                    command.Parameters.Add(new OracleParameter(":password", OracleDbType.Varchar2, password, ParameterDirection.Input));

                    // Execute the function call
                    command.ExecuteNonQuery();

                    // Retrieve the result from the output parameter
                    return ((Oracle.ManagedDataAccess.Types.OracleBoolean)command.Parameters[":result"].Value).Value;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions or log them if needed
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return false;
    }

    public DataTable GetUsers()
    {
        DataTable table = new DataTable();
        try
        {
            using (var connection = OracleDbContext.Get().Connection())
            {
                connection.Open();

                string selectAllUsersQuery = "SELECT * FROM EMPLOYEE";

                using (OracleCommand cmd = new OracleCommand(selectAllUsersQuery, connection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions or log them if needed
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return table;
    }
}

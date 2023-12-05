using Oracle.ManagedDataAccess.Client;

namespace CaribbeanSailboat.Database;

public class OracleDbContext
{
    private string connectionString = "";
    public string ConnectionString => connectionString;

    private static readonly Lazy<OracleDbContext> lazy = new Lazy<OracleDbContext>(() => new OracleDbContext());

    public static OracleDbContext Get()
    {
        return lazy.Value;
    }

    private OracleDbContext()
    {
        string configFile = "ORAConfig.json";
        var config = ORAConfigReader.ReadConfig(configFile);

        // Build connection string
        connectionString = $"Data Source={config.Host}:1521/{config.ServiceName};User Id={config.UserId};Password={config.Password};";
        //connectionString = $"Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {config.Host})(PORT = {config.Port})) (CONNECT_DATA=(SERVICE_NAME={config.ServiceName})));User Id={config.UserId};Password={config.Password};";
    }


    public OracleConnection Connection()
    {
        return new OracleConnection(connectionString);
    }

    public void Execute(string queryString, string connectionString)
    {
        using (var connection = new OracleConnection(connectionString))
        {
            var command = new OracleCommand(queryString, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
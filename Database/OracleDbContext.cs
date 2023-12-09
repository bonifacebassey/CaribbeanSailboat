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
        string configFile = "dbConfig.json";
        var config = DBConfigReader.ReadConfig(configFile);

        // Build connection string
        connectionString = $"Data Source={config.Host}:1521/{config.ServiceName};User Id={config.UserId};Password={config.Password};";
        //connectionString = $"Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {config.Host})(PORT = {config.Port})) (CONNECT_DATA=(SERVICE_NAME={config.ServiceName})));User Id={config.UserId};Password={config.Password};";
    }


    public OracleConnection Connection()
    {
        return new OracleConnection(connectionString);
    }

    public async Task<bool> IsDatabaseRunningAsync()
    {
        try
        {
            using (OracleConnection connection = Connection())
            {
                await connection.OpenAsync();
                return connection.State == System.Data.ConnectionState.Open;
            }
        }
        catch
        {
            return false; // If an exception occurs, the database is not running
        }
    }
}
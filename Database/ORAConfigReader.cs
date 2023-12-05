using Newtonsoft.Json;

namespace CaribbeanSailboat.Database;

public class Config
{
    public string? Host { get; set; }
    public string? Port { get; set; }
    public string? ServiceName { get; set; }
    public string? UserId { get; set; }
    public string? Password { get; set; }
}

public class ORAConfigReader
{
    public static Config ReadConfig(string filename)
    {
        try
        {
            // Get the current directory
            string currentDirectory = Directory.GetCurrentDirectory();

            // Combine the current directory with the filename to get the full path
            string filePath = Path.Combine(currentDirectory, filename);

            // Read the content of the file
            string json = File.ReadAllText(filePath);

            // Deserialize JSON to a C# object
            return JsonConvert.DeserializeObject<Config>(json) ?? new Config();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the configuration: {ex.Message}");
            return new Config();
        }
    }
}

namespace Bookings.PostgresRepositories.Models.Settings;

public class PostgresDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
} 
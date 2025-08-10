using Npgsql;

namespace PaymentProcessor.Api.Infrastructure.Database;

public class DatabaseHealthCheck(IConfiguration config)
{
    private readonly string _connectionString = config.GetConnectionString("Postgres")!;

    public async Task<bool> IsDatabaseReady()
    {
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var cmd = new NpgsqlCommand("SELECT 1", connection);
            return (await cmd.ExecuteScalarAsync())?.Equals(1) ?? false;
        }
        catch
        {
            return false;
        }
    }
}

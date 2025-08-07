using Npgsql;

namespace PaymentProcessor.Api.Infrastructure.Config;

public sealed class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new ArgumentNullException("Postgres connection string missing");
    }

    public NpgsqlConnection CreateConnection() 
    { 
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}

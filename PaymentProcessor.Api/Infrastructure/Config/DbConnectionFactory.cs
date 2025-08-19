namespace PaymentProcessor.Api.Infrastructure.Config;

public sealed class DbConnectionFactory(IConfiguration configuration)
{
    private readonly string _connectionString = configuration
        .GetConnectionString("Postgres")
        ?? throw new ArgumentNullException("Postgres connection string missing");

    public NpgsqlConnection CreateConnection() 
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;  
    }
}
using Npgsql;
using PaymentProcessor.Api.Domain.DTOs.GET;
using PaymentProcessor.Api.Domain.Entities;
using PaymentProcessor.Api.Features.PaymentProcessor.Interfaces;
using PaymentProcessor.Api.Infrastructure.Enum;

namespace PaymentProcessor.Api.Features.PaymentProcessor;

public class PaymentRepository : IPaymentRepository
{
    //private readonly IConfiguration _configuration;
    //private readonly string _connectionString;
    private readonly NpgsqlDataSource _dataSource;

    public PaymentRepository(IConfiguration configuration, 
        NpgsqlDataSource dataSource)
    {
        //_configuration = configuration;
        //_connectionString = _configuration.GetConnectionString("Postgres")!;
        _dataSource = dataSource;
    }

    public async Task InsertPaymentAsync(Payment paymentEntity, CancellationToken cancellationToken = default)
    {
        var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await connection.OpenAsync(cancellationToken);

        const string sqlInsert = (
            @"
                INSERT INTO payments(
                    correlation_id, 
                    amount, 
                    processor_type,
                    processed_at)
                VALUE (
                    @correlationId, 
                    @amount, 
                    @processorType, 
                    @processedAt)
            "
        );

        try
        {
            await using (NpgsqlCommand cmd = _dataSource.CreateCommand(sqlInsert))
            {
                cmd.Parameters.AddWithValue("correlationId", paymentEntity.Correlation_Id);
                cmd.Parameters.AddWithValue("amount", paymentEntity.Correlation_Id);
                cmd.Parameters.AddWithValue("processorType", paymentEntity.Correlation_Id);
                cmd.Parameters.AddWithValue("processedAt", paymentEntity.Correlation_Id);

                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<PaymentsSummaryResponse> GetPaymentsSummaryAsync(DateTime? fromUtc, DateTime? toUtc, CancellationToken cancellationToken = default)
    {
        using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        var sqlSelect = (
            @"
            SELECT processor_type, 
                COUNT(*) AS total_requests, 
                COALESCE(SUM(amount),0) 
                    AS total_amount
            FROM payments
            WHERE (@from IS NULL OR processed_at >= @from)
              AND (@to IS NULL OR processed_at <= @to)
            GROUP BY processor_type"
        );

        await using var cmd = _dataSource.CreateCommand(sqlSelect);
        var result = cmd.Parameters.AddWithValue(sqlSelect, new
        {
            From = fromUtc,
            To = toUtc,
        });

        var summary = new PaymentsSummaryResponse();

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var type = reader.GetString(0);
            var count = reader.GetInt32(1);
            var amount = reader.GetDecimal(2);

            if (type == PaymentGateway.Default.ToString())
            {
                summary.Default.TotalRequests = count;
                summary.Default.TotalAmount = amount;
            }
            else if (type == PaymentGateway.Fallback.ToString())
            {
                summary.Fallback.TotalRequests = count;
                summary.Fallback.TotalAmount = amount;
            }
        }
        return summary;
    }

    public async Task<bool> ExistAsync(Guid correlationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

using PaymentProcessor.Api.Domain.DTOs.GET;
using PaymentProcessor.Api.Domain.Entities;
using PaymentProcessor.Api.Features.PaymentProcessor.Interfaces;
using PaymentProcessor.Api.Infrastructure.Enum;

namespace PaymentProcessor.Api.Features.PaymentProcessor;

public class PaymentRepository : IPaymentRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PaymentRepository(NpgsqlDataSource dataSource)
        => _dataSource = dataSource;

    public async Task InsertPaymentAsync(Payment paymentEntity, CancellationToken cancellationToken = default)
    {
        var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await connection.OpenAsync(cancellationToken);

        try
        {
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
                    @processedAt);
                "
            );
            await using NpgsqlCommand cmd = _dataSource.CreateCommand(sqlInsert);

            cmd.Parameters.AddWithValue("correlationId", paymentEntity.Correlation_Id);
            cmd.Parameters.AddWithValue("amount", paymentEntity.Correlation_Id);
            cmd.Parameters.AddWithValue("processorType", paymentEntity.Correlation_Id);
            cmd.Parameters.AddWithValue("processedAt", paymentEntity.Correlation_Id);

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            await connection.CloseAsync();
            await _dataSource.DisposeAsync();
        }
    }

    public async Task<PaymentsSummaryResponse> GetPaymentsSummaryAsync(DateTimeOffset? fromUtc, DateTimeOffset? toUtc, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await connection.OpenAsync(cancellationToken);

        try
        {
            var sqlSelect = (
                @"
                SELECT gateway, 
                    COUNT(*) AS TotalRequests, 
                    COALESCE(SUM(amount),0) 
                        AS TotalAmount
                FROM payments
                WHERE (@fromUtc IS NULL OR requested_at >= @fromUtc)
                  AND (@toUtc IS NULL OR requested_at <= @Utcto)
                GROUP BY gateway;
                "
            );

            PaymentSummaryDTO? defaultSummary = null;
            PaymentSummaryDTO? fallbackSummary = null;

            await using var cmd = _dataSource.CreateCommand(sqlSelect);

            cmd.Parameters.AddWithValue("fromUtc", fromUtc ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("toUtc", toUtc ?? (object)DBNull.Value);

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var type = reader.GetString(0);
                var count = reader.GetInt32(1);
                var amount = reader.GetDecimal(2);

                var dto = new PaymentSummaryDTO(
                    Gateway: type,
                    TotalRequests: count,
                    TotalAmount: amount,
                    TotalFee: 0, // Adjust if you have fee columns
                    FeePerTransaction: 0 // Adjust if you have fee columns
                );

                if (type.Equals(PaymentGateway.Default.ToString(), StringComparison.OrdinalIgnoreCase))
                    defaultSummary = dto;
                else if (type.Equals(PaymentGateway.Fallback.ToString(), StringComparison.OrdinalIgnoreCase))
                    fallbackSummary = dto;
            }

            defaultSummary ??= new PaymentSummaryDTO(
                PaymentGateway.Default.ToString(), 0, 0, 0, 0);
            fallbackSummary ??= new PaymentSummaryDTO(
                PaymentGateway.Fallback.ToString(), 0, 0, 0, 0);

            return new PaymentsSummaryResponse(defaultSummary, fallbackSummary);
        }
        finally
        {
            await connection.CloseAsync();
            await _dataSource.DisposeAsync();
        }
    }

    public async Task<bool> ExistAsync(Guid correlationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

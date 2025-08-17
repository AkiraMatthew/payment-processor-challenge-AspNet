namespace PaymentProcessor.Api.Domain.DTOs.GET;

public sealed record PaymentSummaryDTO(
    string? Gateway,
    long TotalRequests, 
    decimal TotalAmount,
    decimal TotalFee,
    decimal FeePerTransaction);
//{
//    public PaymentSummaryDTO() : this(
//        string.Empty,
//        default,
//        default,
//        default,
//        default)
//    { }
//}

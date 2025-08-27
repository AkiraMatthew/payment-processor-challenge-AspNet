using PaymentProcessor.Api.Domain.DTOs.GET;
using PaymentProcessor.Api.Domain.DTOs.POST;
using PaymentProcessor.Api.Domain.Entities;
using PaymentProcessor.Api.Infrastructure.Enum;
using System.Text.Json.Serialization;

namespace PaymentProcessor.Api.Infrastructure.Config;

[JsonSerializable(typeof(Payment))]
[JsonSerializable(typeof(PaymentRequest))]
[JsonSerializable(typeof(SummaryResponse))]
[JsonSerializable(typeof(PaymentGateway))]
[JsonSerializable(typeof(DateTimeOffset?))]
public sealed partial class AppJsonSerializerContext : JsonSerializerContext;

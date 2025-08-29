using System.Text.Json.Serialization;

namespace PaymentProcessor.Api.Domain.DTOs.GET;

public sealed class HealthResponse
{
    private HealthResponse(bool failing, int minResponseTime)
    {
        Failing = failing;
        MinResponseTime = minResponseTime;
    }

    [JsonConstructor]
    public HealthResponse()
    { }

    [JsonPropertyName("failing")]
    public bool Failing { get; set; }

    [JsonPropertyName("minResponseTime")]
    public int MinResponseTime { get; set; }

    [JsonIgnore]
    public bool IsHealthy => !Failing;

    [JsonIgnore]
    public static HealthResponse Default = new HealthResponse(false, 0);
}
using System.Text.Json.Serialization;

namespace BusinessLogicAPI.Models;

public record RulesEngineValidationRequest
{
    [JsonPropertyName("carrierId")]
    public required int CarrierId { get; init; }

    [JsonPropertyName("featureName")]
    public required string FeatureName { get; init; } = string.Empty;

    [JsonPropertyName("requestDate")]
    public required DateTime RequestDate { get; init; }

    [JsonPropertyName("drugId")]
    public int? DrugId { get; init; }

    [JsonPropertyName("planId")]
    public int? PlanId { get; init; }
}

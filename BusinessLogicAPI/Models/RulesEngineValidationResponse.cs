using System.Text.Json.Serialization;

namespace BusinessLogicAPI.Models;

public record RulesEngineValidationResponse
{
    [JsonPropertyName("isValid")]
    public bool IsValid { get; init; }

    [JsonPropertyName("overallResult")]
    public string OverallResult { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("details")]
    public List<RuleDetail> Details { get; init; } = new();

    [JsonPropertyName("input")]
    public RequestInput Input { get; init; } = new();
}

public record RuleDetail
{
    [JsonPropertyName("rule")]
    public string Rule { get; init; } = string.Empty;

    [JsonPropertyName("passed")]
    public bool Passed { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("error")]
    public string? Error { get; init; }
}

public record RequestInput
{
    [JsonPropertyName("carrierId")]
    public int CarrierId { get; init; }

    [JsonPropertyName("featureName")]
    public string FeatureName { get; init; } = string.Empty;

    [JsonPropertyName("requestDate")]
    public string? RequestDate { get; init; }

    [JsonPropertyName("drugId")]
    public int? DrugId { get; init; }

    [JsonPropertyName("planId")]
    public int? PlanId { get; init; }
}

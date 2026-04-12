namespace BusinessLogicAPI.Models;

public record RulesRecord
{
    public int CarrierId { get; init; }
    public string FeatureName { get; init; } = string.Empty;
    public DateTime RequestDate { get; init; }
    public string Json { get; init; } = string.Empty;
}

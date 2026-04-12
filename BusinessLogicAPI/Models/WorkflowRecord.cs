namespace BusinessLogicAPI.Models;

public record WorkflowRecord
{
    public int CarrierId { get; init; }
    public string FeatureName { get; init; } = string.Empty;
    public DateTime RequestDate { get; init; }
    public string WorkflowJson { get; init; } = string.Empty;
}

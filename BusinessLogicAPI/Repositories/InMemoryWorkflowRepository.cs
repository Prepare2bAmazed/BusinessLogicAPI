using BusinessLogicAPI.Models;

namespace BusinessLogicAPI.Repositories;

public class InMemoryWorkflowRepository : IWorkflowRepository
{
    private readonly List<WorkflowRecord> _workflows;

    public InMemoryWorkflowRepository()
    {
        // TODO: Load from configuration
        // This will be populated from appsettings.json
        _workflows = new List<WorkflowRecord>();
    }

    public InMemoryWorkflowRepository(List<WorkflowRecord> workflows)
    {
        _workflows = workflows;
    }

    public Task<string?> GetWorkflowJsonAsync(int carrierId, string featureName, DateTime requestDate)
    {
        // Find the most recent workflow that matches criteria and is not in the future
        var workflow = _workflows
            .Where(w => w.CarrierId == carrierId 
                     && w.FeatureName.Equals(featureName, StringComparison.OrdinalIgnoreCase)
                     && w.RequestDate <= requestDate)
            .OrderByDescending(w => w.RequestDate)
            .FirstOrDefault();

        return Task.FromResult(workflow?.WorkflowJson);
    }
}

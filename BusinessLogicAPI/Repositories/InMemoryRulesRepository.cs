using BusinessLogicAPI.Models;

namespace BusinessLogicAPI.Repositories;

public class InMemoryRulesRepository : IRulesRepository
{
    private readonly List<RulesRecord> _rules;

    public InMemoryRulesRepository()
    {
        _rules = new List<RulesRecord>();
    }

    public InMemoryRulesRepository(List<RulesRecord> rules)
    {
        _rules = rules;
    }

    public Task<string?> GetRulesJsonAsync(int carrierId, string featureName, DateTime requestDate)
    {
        // Find the most recent rules that match criteria and are not in the future
        var rulesRecord = _rules
            .Where(r => r.CarrierId == carrierId 
                     && r.FeatureName.Equals(featureName, StringComparison.OrdinalIgnoreCase)
                     && r.RequestDate <= requestDate)
            .OrderByDescending(r => r.RequestDate)
            .FirstOrDefault();

        return Task.FromResult(rulesRecord?.Json);
    }
}

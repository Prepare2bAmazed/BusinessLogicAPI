namespace BusinessLogicAPI.Repositories;

public interface IRulesRepository
{
    Task<string?> GetRulesJsonAsync(int carrierId, string featureName, DateTime requestDate);
}

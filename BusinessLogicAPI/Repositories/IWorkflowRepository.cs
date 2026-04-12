using BusinessLogicAPI.Models;

namespace BusinessLogicAPI.Repositories;

public interface IWorkflowRepository
{
    Task<string?> GetWorkflowJsonAsync(int carrierId, string featureName, DateTime requestDate);
}

using BusinessLogicAPI.Models;
using BusinessLogicAPI.Repositories;
using BusinessLogicAPI.Utils;
using RulesEngine.Models;
using System.Text.Json;

namespace BusinessLogicAPI.Services;

public class RulesEngineValidationService
{
    private readonly IWorkflowRepository _workflowRepository;

    public RulesEngineValidationService(IWorkflowRepository workflowRepository)
    {
        _workflowRepository = workflowRepository;
    }

    public async Task<RulesEngineValidationResponse> ValidateRequestAsync(RulesEngineValidationRequest request)
    {
        var workflows = await LoadWorkflowsAsync(request.CarrierId, request.FeatureName, request.RequestDate);
        var rulesEngine = CreateRulesEngine(workflows);
        var results = await ExecuteRulesAsync(rulesEngine, workflows[0].WorkflowName, request);

        return BuildResponse(results, request);
    }

    private async Task<Workflow[]> LoadWorkflowsAsync(int carrierId, string featureName, DateTime requestDate)
    {
        var workflowJson = await _workflowRepository.GetWorkflowJsonAsync(carrierId, featureName, requestDate);

        if (string.IsNullOrWhiteSpace(workflowJson))
        {
            throw new InvalidOperationException(
                $"No workflow found for CarrierId={carrierId}, FeatureName={featureName}, RequestDate={requestDate:yyyy-MM-dd}");
        }

        var workflows = JsonSerializer.Deserialize<Workflow[]>(workflowJson);

        if (workflows == null || workflows.Length == 0)
        {
            throw new InvalidOperationException("Failed to deserialize workflow configuration");
        }

        return workflows;
    }

    private RulesEngine.RulesEngine CreateRulesEngine(Workflow[] workflows)
    {
        var reSettings = new ReSettings
        {
            CustomTypes = new[] { typeof(DateTime), typeof(RulesEngineUtils) }
        };

        return new RulesEngine.RulesEngine(workflows, reSettings);
    }

    private async Task<List<RuleResultTree>> ExecuteRulesAsync(
        RulesEngine.RulesEngine rulesEngine,
        string workflowName,
        RulesEngineValidationRequest request)
    {
        var input = new
        {
            CarrierId = request.CarrierId,
            FeatureName = request.FeatureName,
            RequestDate = request.RequestDate,
            DrugId = request.DrugId,
            PlanId = request.PlanId
        };

        var ruleParameters = new[]
        {
            new RuleParameter("input", input)
        };

        return await rulesEngine.ExecuteAllRulesAsync(workflowName, ruleParameters);
    }

    private RulesEngineValidationResponse BuildResponse(
        List<RuleResultTree> results, 
        RulesEngineValidationRequest request)
    {
        var mainResult = results.First();
        var isValid = mainResult.IsSuccess;

        return new RulesEngineValidationResponse
        {
            IsValid = isValid,
            OverallResult = isValid ? "APPROVED" : "REJECTED",
            Message = isValid
                ? "All validations passed - Request approved"
                : "Request rejected - One or more validations failed",
            Details = mainResult.ChildResults?.Select(cr => new RuleDetail
            {
                Rule = cr.Rule.RuleName,
                Passed = cr.IsSuccess,
                Message = cr.IsSuccess
                    ? $"{cr.Rule.RuleName} passed"
                    : $"{cr.Rule.RuleName} failed",
                Error = cr.ExceptionMessage
            }).ToList() ?? new List<RuleDetail>(),
            Input = new RequestInput
            {
                CarrierId = request.CarrierId,
                FeatureName = request.FeatureName,
                RequestDate = request.RequestDate.ToString("yyyy-MM-dd"),
                DrugId = request.DrugId,
                PlanId = request.PlanId
            }
        };
    }
}

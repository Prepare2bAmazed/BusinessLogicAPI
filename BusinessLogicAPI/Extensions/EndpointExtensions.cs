using BusinessLogicAPI.Models;
using BusinessLogicAPI.Services;

namespace BusinessLogicAPI.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        app.MapPost("/rules/validate", async (RulesEngineValidationRequest request, RulesEngineValidationService validationService) =>
        {
            try
            {
                var response = await validationService.ValidateRequestAsync(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Error executing rules: {ex.Message}");
            }
        })
        .WithName("ValidateRequest")
        .WithDescription("Validates a request against configured business rules");

        return app;
    }
}

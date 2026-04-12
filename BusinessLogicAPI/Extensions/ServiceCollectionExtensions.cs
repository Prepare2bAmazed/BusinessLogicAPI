using BusinessLogicAPI.Repositories;
using BusinessLogicAPI.Services;

namespace BusinessLogicAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkflowServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Read configuration values directly
        var dataSource = configuration["RulesEngine:DataSource"] ?? "InMemory";
        var connectionString = configuration["RulesEngine:ConnectionString"];

        // Register workflow repository based on configuration
        if (dataSource == "Database")
        {
            services.AddScoped<IWorkflowRepository>(_ =>
                new DatabaseWorkflowRepository(connectionString ?? string.Empty));
        }
        else // Default to InMemory
        {
            services.AddSingleton<IWorkflowRepository>(_ =>
                new InMemoryWorkflowRepository(InMemoryWorkflowData.GetTestWorkflows()));
        }

        services.AddScoped<RulesEngineValidationService>();

        return services;
    }
}

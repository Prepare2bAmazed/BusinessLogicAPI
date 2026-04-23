using BusinessLogicAPI.Data;
using BusinessLogicAPI.Repositories;
using BusinessLogicAPI.Services;

namespace BusinessLogicAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRulesEngineServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Read configuration values directly
        var dataSource = configuration["RulesEngine:DataSource"] ?? "InMemory";
        var connectionString = configuration["RulesEngine:ConnectionString"];

        // Register rules repository based on configuration
        if (dataSource == "Database")
        {
            services.AddScoped<IRulesRepository>(_ =>
                new DatabaseRulesRepository(connectionString ?? string.Empty));
        }
        else // Default to InMemory
        {
            services.AddSingleton<IRulesRepository>(_ =>
                new InMemoryRulesRepository(TestRulesData.GetTestRules()));
        }

        services.AddScoped<RulesEngineValidationService>();

        return services;
    }
}

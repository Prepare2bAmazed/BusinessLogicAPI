using BusinessLogicAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogicAPI.Repositories;

public class DatabaseWorkflowRepository : IWorkflowRepository
{
    private readonly string _connectionString;

    public DatabaseWorkflowRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string?> GetWorkflowJsonAsync(int carrierId, string featureName, DateTime requestDate)
    {
        // TODO: Replace with actual database call when ready
        // PLACEHOLDER CODE - Not connected to real database

        /* 
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("usp_GetWorkflowJson", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddWithValue("@CarrierId", carrierId);
        command.Parameters.AddWithValue("@FeatureName", featureName);
        command.Parameters.AddWithValue("@RequestDate", requestDate);

        await connection.OpenAsync();
        
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return reader["WorkflowJson"] as string;
        }

        return null;
        */

        // For now, throw exception to remind you this is not implemented
        throw new NotImplementedException(
            $"Database repository not yet implemented. " +
            $"Attempted to fetch workflow for CarrierId={carrierId}, " +
            $"FeatureName={featureName}, RequestDate={requestDate:yyyy-MM-dd}");
    }
}

/*
SQL Server Stored Procedure Example:

CREATE PROCEDURE usp_GetWorkflowJson
    @CarrierId INT,
    @FeatureName NVARCHAR(100),
    @RequestDate DATETIME
AS
BEGIN
    SELECT TOP 1 
        CarrierId,
        FeatureName,
        RequestDate,
        WorkflowJson
    FROM WorkflowRules
    WHERE CarrierId = @CarrierId
      AND FeatureName = @FeatureName
      AND RequestDate <= @RequestDate
    ORDER BY RequestDate DESC
END

Table Schema Example:

CREATE TABLE WorkflowRules (
    CarrierId INT NOT NULL,
    FeatureName NVARCHAR(100) NOT NULL,
    RequestDate DATETIME NOT NULL,
    WorkflowJson NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (CarrierId, FeatureName, RequestDate)
)
*/

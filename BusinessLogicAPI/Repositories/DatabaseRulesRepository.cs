using BusinessLogicAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogicAPI.Repositories;

public class DatabaseRulesRepository : IRulesRepository
{
    private readonly string _connectionString;

    public DatabaseRulesRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<string?> GetRulesJsonAsync(int carrierId, string featureName, DateTime requestDate)
    {
        // TODO: Replace with actual database call when ready
        // PLACEHOLDER CODE - Not connected to real database

        /* 
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand("usp_GetRulesJson", connection)
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
            return reader["Json"] as string;
        }

        return null;
        */

        // For now, throw exception to remind you this is not implemented
        throw new NotImplementedException(
            $"Database repository not yet implemented. " +
            $"Attempted to fetch rules for CarrierId={carrierId}, " +
            $"FeatureName={featureName}, RequestDate={requestDate:yyyy-MM-dd}");
    }
}

/*
SQL Server Stored Procedure Example:

CREATE PROCEDURE usp_GetRulesJson
    @CarrierId INT,
    @FeatureName NVARCHAR(100),
    @RequestDate DATETIME
AS
BEGIN
    SELECT TOP 1 
        CarrierId,
        FeatureName,
        RequestDate,
        Json
    FROM Rules
    WHERE CarrierId = @CarrierId
      AND FeatureName = @FeatureName
      AND RequestDate <= @RequestDate
    ORDER BY RequestDate DESC
END

Table Schema Example:

CREATE TABLE Rules (
    CarrierId INT NOT NULL,
    FeatureName NVARCHAR(100) NOT NULL,
    RequestDate DATETIME NOT NULL,
    Json NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (CarrierId, FeatureName, RequestDate)
)
*/

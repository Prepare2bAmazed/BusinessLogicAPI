# Workflow Repository Configuration Guide

## Overview
The API supports two data sources for workflows:
1. **InMemory** (for testing) - Uses test data from `appsettings.json`
2. **Database** (for production) - Calls stored procedure to fetch workflow JSON

## Configuration

### appsettings.json
```json
{
  "WorkflowSettings": {
    "DataSource": "InMemory",  // Change to "Database" for production
    "ConnectionString": null,   // Set when using Database
    "TestWorkflows": [          // Test data for InMemory mode
      {
        "CarrierId": 1,
        "FeatureName": "DrugValidation",
        "RequestDate": "2025-01-01T00:00:00",
        "WorkflowJson": "[{...workflow JSON...}]"
      }
    ]
  }
}
```

## Switching Between Modes

### For Testing (Current Setup)
```json
"DataSource": "InMemory"
```
- Uses test workflows from `appsettings.json`
- No database connection needed
- Perfect for development and testing

### For Production
```json
"DataSource": "Database",
"ConnectionString": "Server=your-server;Database=your-db;User Id=user;Password=pass;"
```
- Calls stored procedure `usp_GetWorkflowJson`
- Requires database with `WorkflowRules` table

## Database Setup (When Ready)

### Table Schema
```sql
CREATE TABLE WorkflowRules (
    CarrierId INT NOT NULL,
    FeatureName NVARCHAR(100) NOT NULL,
    RequestDate DATETIME NOT NULL,
    WorkflowJson NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (CarrierId, FeatureName, RequestDate)
)
```

### Stored Procedure
```sql
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
```

### Implementation Location
- See `DatabaseWorkflowRepository.cs` for the placeholder code
- Uncomment the database code when ready to connect

## Adding Test Workflows

Add to `appsettings.json`:
```json
"TestWorkflows": [
  {
    "CarrierId": 1,
    "FeatureName": "DrugValidation",
    "RequestDate": "2025-01-01T00:00:00",
    "WorkflowJson": "[{...}]"
  },
  {
    "CarrierId": 2,
    "FeatureName": "PlanValidation",
    "RequestDate": "2025-02-01T00:00:00",
    "WorkflowJson": "[{...}]"
  }
]
```

## API Request Format

```json
{
  "carrierId": 1,           // REQUIRED
  "featureName": "DrugValidation",  // REQUIRED
  "requestDate": "2025-01-15",      // REQUIRED
  "drugId": 123,            // Optional
  "planId": 456             // Optional
}
```

## How It Works

1. **Request comes in** with `carrierId`, `featureName`, `requestDate`
2. **Repository fetches workflow**:
   - **InMemory**: Searches test workflows from config
   - **Database**: Calls stored procedure
3. **Service validates** the request against the workflow rules
4. **Response returned** with validation results

## Benefits

✅ **Easy Testing**: No database needed for development  
✅ **Flexible**: Switch between test and production data with one config change  
✅ **Multiple Carriers**: Support different rules per carrier  
✅ **Multiple Features**: Different validation logic per feature  
✅ **Time-based Rules**: Rules can change over time (versioning by RequestDate)

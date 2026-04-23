## License
This project is licensed under the terms of the CC0 1.0 Universal (True Public Domain). You can use it however you like—no credit or attribution required!

# Rules Repository Configuration Guide

## Overview
The API supports two data sources for rules:
1. **InMemory** (for testing) - Uses test data from `Data/TestRulesData.cs`
2. **Database** (for production) - Calls stored procedure to fetch rules JSON

## Configuration

### appsettings.json
```json
{
  "RulesEngine": {
    "DataSource": "InMemory",       // Change to "Database" for production
    "ConnectionString": null        // Set when using Database
  }
}
```

## Switching Between Modes

### For Testing (Current Setup)
```json
"DataSource": "InMemory"
```
- Uses test rules from `Data/TestRulesData.cs`
- No database connection needed
- Perfect for development and testing

### For Production
```json
"DataSource": "Database",
"ConnectionString": "Server=your-server;Database=your-db;User Id=user;Password=pass;"
```
- Calls stored procedure `usp_GetRulesJson`
- Requires database with `Rules` table

## Database Setup (When Ready)

### Table Schema
```sql
CREATE TABLE Rules (
    CarrierId INT NOT NULL,
    FeatureName NVARCHAR(100) NOT NULL,
    RequestDate DATETIME NOT NULL,
    Json NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (CarrierId, FeatureName, RequestDate)
)
```

### Stored Procedure
```sql
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
```

### Implementation Location
- See `DatabaseRulesRepository.cs` for the placeholder code
- Uncomment the database code when ready to connect

## Adding Test Rules

Edit `Data/TestRulesData.cs` and add to the list:
```csharp
new RulesRecord
{
    CarrierId = 3,
    FeatureName = "PlanValidation",
    RequestDate = new DateTime(2025, 2, 1),
    Json = """
    [
      {
        "WorkflowName": "RulesEngineWorkflow",
        "GlobalParams": [...],
        "Rules": [...]
      }
    ]
    """
}
```

## API Request Format

```json
{
  "carrierId": 1,                   // REQUIRED
  "featureName": "DrugValidation",  // REQUIRED
  "requestDate": "2025-01-15",      // REQUIRED
  "drugId": 123,                    // Optional
  "planId": 456                     // Optional
}
```

## How to Run Locally

```powershell
dotnet run
```

Default local URLs from launch settings:
- `http://localhost:5062`
- `https://localhost:7080`

## API Documentation & UI (Scalar + OpenAPI)

In Development environment, the app exposes:
- OpenAPI JSON: `/openapi/v1.json`
- Scalar API Reference UI: `/scalar`

Examples:
- `http://localhost:5062/openapi/v1.json`
- `http://localhost:5062/scalar`

## OpenAPI Calls (API Self-Description)

Use these calls to have the API describe itself in OpenAPI format.

### cURL (HTTP)
```bash
curl "http://localhost:5062/openapi/v1.json"
```

### cURL (HTTPS, local dev cert)
```bash
curl -k "https://localhost:7080/openapi/v1.json"
```

### PowerShell
```powershell
Invoke-RestMethod -Uri "http://localhost:5062/openapi/v1.json" -Method Get
```

### Save the OpenAPI document to a file
```powershell
Invoke-WebRequest -Uri "http://localhost:5062/openapi/v1.json" -OutFile "openapi.json"
```

## Multiple Ways to Test the API

Use any of these approaches.

### 1) Scalar UI (browser)
1. Run the API with `dotnet run`.
2. Open `http://localhost:5062/scalar`.
3. Find `POST /rules/validate`.
4. Use this sample body:

```json
{
  "carrierId": 1,
  "featureName": "DrugValidation",
  "requestDate": "2025-01-15",
  "drugId": 123,
  "planId": 456
}
```

### 2) cURL
```bash
curl -X POST "http://localhost:5062/rules/validate" \
  -H "Content-Type: application/json" \
  -d '{
    "carrierId": 1,
    "featureName": "DrugValidation",
    "requestDate": "2025-01-15",
    "drugId": 123,
    "planId": 456
  }'
```

### 3) PowerShell
```powershell
$body = @{
  carrierId = 1
  featureName = "DrugValidation"
  requestDate = "2025-01-15"
  drugId = 123
  planId = 456
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5062/rules/validate" -Method Post -ContentType "application/json" -Body $body
```

### 4) Postman or Bruno
- Method: `POST`
- URL: `http://localhost:5062/rules/validate`
- Header: `Content-Type: application/json`
- Body: raw JSON (same sample payload above)

## Example Response

```json
{
  "isValid": false,
  "overallResult": "REJECTED",
  "message": "Request rejected - One or more validations failed",
  "details": [],
  "input": {
    "carrierId": 1,
    "featureName": "DrugValidation",
    "requestDate": "2025-01-15",
    "drugId": 123,
    "planId": 456
  }
}
```

## How It Works

1. **Request comes in** with `carrierId`, `featureName`, `requestDate`
2. **Repository fetches rules**:
   - **InMemory**: Searches test rules from `Data/TestRulesData.cs`
   - **Database**: Calls stored procedure
3. **Service validates** the request against the rules
4. **Response returned** with validation results

## Benefits

✅ **Easy Testing**: No database needed for development  
✅ **Flexible**: Switch between test and production data with one config change  
✅ **Multiple Carriers**: Support different rules per carrier  
✅ **Multiple Features**: Different validation logic per feature  
✅ **Time-based Rules**: Rules can change over time (versioning by RequestDate)

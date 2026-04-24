using BusinessLogicAPI.Models;

namespace BusinessLogicAPI.Data;

public static class TestRulesData
{
    public static List<RulesRecord> GetTestRules()
    {
        return new List<RulesRecord>
        {
            // Test Rules 1: Drug and Plan Validation for Carrier 1
            new RulesRecord
            {
                CarrierId = 1,
                FeatureName = "DrugValidation",
                ActiveDate = new DateTime(2025, 1, 1),
                Json = """
                [
                  {
                    "WorkflowName": "RulesEngineWorkflow",
                    "GlobalParams": [
                      {
                        "Name": "MasterDrugList",
                        "Expression": "new[] { new (123 as ID, \"2025-01-01\" as start, \"2025-02-01\" as end) }"
                      },
                      {
                        "Name": "MasterPlanList",
                        "Expression": "new[] { new (456 as ID, \"2025-01-01\" as start, \"2025-12-31\" as end) }"
                      }
                    ],
                    "Rules": [
                      {
                        "RuleName": "OverallValidation",
                        "Operator": "And",
                        "Rules": [
                          {
                            "RuleName": "DrugCheck",
                            "Expression": "input.DrugId != null && (RulesEngineUtils.IsValid(MasterDrugList, input.DrugId, input.RequestDate, input.RequestDate) || input.DrugId == 888)",
                            "Actions": {
                              "OnSuccess": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Drug check passed\""
                                }
                              },
                              "OnFailure": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Drug check failed - Drug not found in master list or outside valid date range\""
                                }
                              }
                            }
                          },
                          {
                            "RuleName": "PlanCheck",
                            "Expression": "input.PlanId != null && RulesEngineUtils.IsValid(MasterPlanList, input.PlanId, input.RequestDate, input.RequestDate)",
                            "Actions": {
                              "OnSuccess": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Plan check passed\""
                                }
                              },
                              "OnFailure": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Plan check failed - Plan not found in master list or outside valid date range\""
                                }
                              }
                            }
                          }
                        ],
                        "Actions": {
                          "OnSuccess": {
                            "Name": "OutputExpression",
                            "Context": {
                              "Expression": "\"All validations passed - Request approved\""
                            }
                          },
                          "OnFailure": {
                            "Name": "OutputExpression",
                            "Context": {
                              "Expression": "\"Request rejected - One or more validations failed\""
                            }
                          }
                        }
                      }
                    ]
                  }
                ]
                """
            },

            // Test Rules 2: Different rules for Carrier 2
            new RulesRecord
            {
                CarrierId = 2,
                FeatureName = "DrugValidation",
                ActiveDate = new DateTime(2025, 1, 1),
                Json = """
                [
                  {
                    "WorkflowName": "RulesEngineWorkflow",
                    "GlobalParams": [
                      {
                        "Name": "MasterDrugList",
                        "Expression": "new[] { new (999 as ID, \"2025-01-01\" as start, \"2025-12-31\" as end) }"
                      },
                      {
                        "Name": "MasterPlanList",
                        "Expression": "new[] { new (777 as ID, \"2025-01-01\" as start, \"2025-06-30\" as end) }"
                      }
                    ],
                    "Rules": [
                      {
                        "RuleName": "OverallValidation",
                        "Operator": "And",
                        "Rules": [
                          {
                            "RuleName": "DrugCheck",
                            "Expression": "input.DrugId != null && RulesEngineUtils.IsValid(MasterDrugList, input.DrugId, input.RequestDate, input.RequestDate)",
                            "Actions": {
                              "OnSuccess": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Drug check passed for Carrier 2\""
                                }
                              },
                              "OnFailure": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Drug check failed for Carrier 2\""
                                }
                              }
                            }
                          },
                          {
                            "RuleName": "PlanCheck",
                            "Expression": "input.PlanId != null && RulesEngineUtils.IsValid(MasterPlanList, input.PlanId, input.RequestDate, input.RequestDate)",
                            "Actions": {
                              "OnSuccess": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Plan check passed for Carrier 2\""
                                }
                              },
                              "OnFailure": {
                                "Name": "OutputExpression",
                                "Context": {
                                  "Expression": "\"Plan check failed for Carrier 2\""
                                }
                              }
                            }
                          }
                        ],
                        "Actions": {
                          "OnSuccess": {
                            "Name": "OutputExpression",
                            "Context": {
                              "Expression": "\"All validations passed - Carrier 2 approved\""
                            }
                          },
                          "OnFailure": {
                            "Name": "OutputExpression",
                            "Context": {
                              "Expression": "\"Request rejected - Carrier 2 validation failed\""
                            }
                          }
                        }
                      }
                    ]
                  }
                ]
                """
            }
        };
    }
}




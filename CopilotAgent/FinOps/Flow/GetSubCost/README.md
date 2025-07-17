<h3>Azure UpdateManager Copilot Studio Flow - CheckPermission</h3>

Start with the "When an agent calls the flow" setting 2 input variable, "Fromdate" and "Todate":

<img src="./images/1 start.png" alt="1" width="800" >

Leave the value empty.

The second step is to add with the "+" button an "HTTP Request" block configuring it following the paramenter below:

<img src="./images/2 http.png" alt="2" width="800" >
<img src="./images/3 http.png" alt="3" width="800" >
<img src="./images/4 http.png" alt="4" width="800" >
<img src="./images/5 http.png" alt="5" width="800" >

You can also follow the configuration JSON below:

```code
{
  "type": "Http",
  "inputs": {
    "uri": "https://management.azure.com/subscriptions/subscriptionid/providers/Microsoft.CostManagement/query?api-version=2023-03-01",
    "method": "POST",
    "headers": {
      "Content-Type": "application/json"
    },
    "body": {
      "dataSet": {
        "aggregation": {
          "totalCost": {
            "function": "Sum",
            "name": "Cost"
          }
        },
        "granularity": "None",
        "grouping": [
          {
            "name": "ServiceName",
            "type": "Dimension"
          }
        ],
        "sorting": [
          {
            "direction": "descending",
            "name": "Cost"
          }
        ]
      },
      "timePeriod": {
        "from": "@{concat(triggerBody()?['text'], 'T00:00:00.000Z')}",
        "to": "@{concat(triggerBody()?['text_1'], 'T23:59:59.000Z')}"
      },
      "timeframe": "Custom",
      "type": "ActualCost"
    },
    "authentication": {
      "type": "ActiveDirectoryOAuth",
      "tenant": "tenantid",
      "audience": "https://management.azure.com",
      "clientId": "clientid",
      "secret": "secretid"
    }
  },
  "runAfter": {},
  "runtimeConfiguration": {
    "contentTransfer": {
      "transferMode": "Chunked"
    }
  }
}
```

For all the POST Operation is important to have a Service Principal on <a href="https://learn.microsoft.com/en-us/entra/identity-platform/howto-create-service-principal-portal">Azure with the required permition for the operations</a>.

After that pass the Body of the block to a new "Parse JSON" block below the "HTTP Request" block. 

<img src="./images/6 Parse Json.png" alt="6" width="800" >

Paste the schema below:

```code
{
    "type": "object",
    "properties": {
        "statusCode": {
            "type": "integer"
        },
        "headers": {
            "type": "object",
            "properties": {
                "Cache-Control": {
                    "type": "string"
                },
                "Pragma": {
                    "type": "string"
                },
                "Vary": {
                    "type": "string"
                },
                "session-id": {
                    "type": "string"
                },
                "x-ms-request-id": {
                    "type": "string"
                },
                "x-ms-correlation-request-id": {
                    "type": "string"
                },
                "x-ms-client-request-id": {
                    "type": "string"
                },
                "X-Powered-By": {
                    "type": "string"
                },
                "x-ms-operation-identifier": {
                    "type": "string"
                },
                "x-ms-ratelimit-remaining-subscription-resource-requests": {
                    "type": "string"
                },
                "x-ms-routing-request-id": {
                    "type": "string"
                },
                "Strict-Transport-Security": {
                    "type": "string"
                },
                "X-Content-Type-Options": {
                    "type": "string"
                },
                "X-Cache": {
                    "type": "string"
                },
                "X-MSEdge-Ref": {
                    "type": "string"
                },
                "Date": {
                    "type": "string"
                },
                "Content-Length": {
                    "type": "string"
                },
                "Content-Type": {
                    "type": "string"
                },
                "Expires": {
                    "type": "string"
                }
            }
        },
        "body": {
            "type": "object",
            "properties": {
                "id": {
                    "type": "string"
                },
                "name": {
                    "type": "string"
                },
                "type": {
                    "type": "string"
                },
                "location": {},
                "sku": {},
                "eTag": {},
                "properties": {
                    "type": "object",
                    "properties": {
                        "nextLink": {},
                        "columns": {
                            "type": "array",
                            "items": {
                                "type": "object",
                                "properties": {
                                    "name": {
                                        "type": "string"
                                    },
                                    "type": {
                                        "type": "string"
                                    }
                                },
                                "required": [
                                    "name",
                                    "type"
                                ]
                            }
                        },
                        "rows": {
                            "type": "array",
                            "items": {
                                "type": "array"
                            }
                        }
                    }
                }
            }
        }
    }
}
```

Now Inizialize a new variable, call it "CostOutput" and set the type as a String:

<img src="./images/7 variable.png" alt="7" width="800" >

In the Value you can set it as follow:

```code
@{triggerBody()?['text']} - @{triggerBody()?['text_1']} Costs: @{body('Parse_JSON')?['properties']?['rows']}
```

And on the "Respond to the agent" block set the Output Variable name as "CostOutput" and fill it with the value of "CostOutput" Variable:

<img src="./images/8 end.png" alt="8" width="800" >

Go back to continue [Configuration Link](../../README.md#finopsflowtwo)
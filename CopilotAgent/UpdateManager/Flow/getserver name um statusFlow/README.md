<h3>Azure UpdateManager Copilot Studio Flow - Get Server Name UM Status</h3>
 
<img src="./images/1 input.png" alt="1" width="800" >

Click on "+" and create "Initialize Variable":

<img src="./images/2 inizialize variable.png" alt="2" width="800" >

Call the variable "querygraph", set it as a String and past the following query as a value:

```querygraph
patchassessmentresources
| extend ServerName= extract(@'/machines/([^/]+)/', 1, id)
| extend ServerNameAZ = extract(@'/virtualMachines/([^/]+)/', 1, id)
| where  ServerName =~ "@{triggerBody()?['text']}" or ServerNameAZ =~"@{triggerBody()?['text']}"
```

Click again on "+" and select again "Initialize Variable", call it "apiversion" and place in the value the required api version (ex. 2021-03-01):

<img src="./images/3 inizialize variable.png" alt="3" width="800" >

After the variables creation place an "HTTP request" block and fill all the required information:

<img src="./images/4 HTTP request.png" alt="HTTPRequest" width="800" >

<img src="./images/4-1 HTTP request.png" alt="HTTPRequest4-1" width="800" >

After HTTP request creation place and Parse JSON block and use the "body" output as a content.

<img src="./images/5 parse json.png" alt="parseJSON" width="800" >

Copy and past the schema:

```schema
{
    "properties": {
        "extendedLocation": {},
        "id": {
            "type": "string"
        },
        "identity": {},
        "kind": {
            "type": "string"
        },
        "location": {
            "type": "string"
        },
        "managedBy": {
            "type": "string"
        },
        "name": {
            "type": "string"
        },
        "plan": {},
        "properties": {
            "properties": {
                "classifications": {
                    "items": {
                        "type": "string"
                    },
                    "type": "array"
                },
                "kbId": {
                    "type": "string"
                },
                "lastModifiedDateTime": {
                    "type": "string"
                },
                "patchId": {
                    "type": "string"
                },
                "patchName": {
                    "type": "string"
                },
                "publishedDateTime": {
                    "type": "string"
                },
                "rebootBehavior": {
                    "type": "string"
                }
            },
            "type": "object"
        },
        "resourceGroup": {
            "type": "string"
        },
        "sku": {},
        "subscriptionId": {
            "type": "string"
        },
        "tags": {},
        "tenantId": {
            "type": "string"
        },
        "type": {
            "type": "string"
        },
        "zones": {}
    },
    "type": "object"
}
```

Now we need to create a "machineid" variable, paste the code below inside the value :

<img src="./images/6 inizialize variable.png" alt="6" width="800" >

```function
body('Parse_JSON')?['data']?[0]?['id']
```

Create another "HTTP Request" for OpenAI service and fill all the required box following the examples below:

<img src="./images/7 OpenAI.png" alt="7" width="800" >
<img src="./images/7-1 OpenAI.png" alt="7" width="800" >

After HTTP request for OpenAI creation, place an "Parse JSON" block and use the "body" output of HTTP request OpenAI as a content.

<img src="./images/8 parse json.png" alt="parseJSON" width="800" >

Copy and past the schema (Schema for OpenAI 3.5 model.If you use other model schema may change):

```schema
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
                "Access-Control-Allow-Origin": {
                    "type": "string"
                },
                "apim-request-id": {
                    "type": "string"
                },
                "Strict-Transport-Security": {
                    "type": "string"
                },
                "X-Content-Type-Options": {
                    "type": "string"
                },
                "x-ms-region": {
                    "type": "string"
                },
                "x-ratelimit-remaining-requests": {
                    "type": "string"
                },
                "x-ratelimit-limit-requests": {
                    "type": "string"
                },
                "x-ratelimit-remaining-tokens": {
                    "type": "string"
                },
                "x-ratelimit-limit-tokens": {
                    "type": "string"
                },
                "x-accel-buffering": {
                    "type": "string"
                },
                "x-ms-rai-invoked": {
                    "type": "string"
                },
                "X-Request-ID": {
                    "type": "string"
                },
                "x-ms-client-request-id": {
                    "type": "string"
                },
                "azureml-model-session": {
                    "type": "string"
                },
                "x-ms-deployment-name": {
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
                }
            }
        },
        "body": {
            "type": "object",
            "properties": {
                "choices": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "content_filter_results": {
                                "type": "object",
                                "properties": {
                                    "hate": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    },
                                    "protected_material_code": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "detected": {
                                                "type": "boolean"
                                            }
                                        }
                                    },
                                    "protected_material_text": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "detected": {
                                                "type": "boolean"
                                            }
                                        }
                                    },
                                    "self_harm": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    },
                                    "sexual": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    },
                                    "violence": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    }
                                }
                            },
                            "finish_reason": {
                                "type": "string"
                            },
                            "index": {
                                "type": "integer"
                            },
                            "message": {
                                "type": "object",
                                "properties": {
                                    "content": {
                                        "type": "string"
                                    },
                                    "role": {
                                        "type": "string"
                                    }
                                }
                            }
                        },
                        "required": [
                            "content_filter_results",
                            "finish_reason",
                            "index",
                            "message"
                        ]
                    }
                },
                "created": {
                    "type": "integer"
                },
                "id": {
                    "type": "string"
                },
                "model": {
                    "type": "string"
                },
                "object": {
                    "type": "string"
                },
                "prompt_filter_results": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "prompt_index": {
                                "type": "integer"
                            },
                            "content_filter_results": {
                                "type": "object",
                                "properties": {
                                    "hate": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    },
                                    "jailbreak": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "detected": {
                                                "type": "boolean"
                                            }
                                        }
                                    },
                                    "self_harm": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    },
                                    "sexual": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    },
                                    "violence": {
                                        "type": "object",
                                        "properties": {
                                            "filtered": {
                                                "type": "boolean"
                                            },
                                            "severity": {
                                                "type": "string"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        "required": [
                            "prompt_index",
                            "content_filter_results"
                        ]
                    }
                },
                "system_fingerprint": {
                    "type": "string"
                },
                "usage": {
                    "type": "object",
                    "properties": {
                        "completion_tokens": {
                            "type": "integer"
                        },
                        "prompt_tokens": {
                            "type": "integer"
                        },
                        "total_tokens": {
                            "type": "integer"
                        }
                    }
                }
            }
        }
    }
}
```

Click again on "+" and select "Initialize Variable" and call variable "respond":

<img src="./images//9 inizialize variable.png" alt="AUMOverview" width="800" >

Now create an "Apply To Each" block, and ensure that the variable follow the images below:

<img src="./images/10 For Each.png" alt="Applytoeach" width="800" >

```code
@{body ('Parse_JSON_OpenAI')?['choices']}
```

Inside the "Apply to each" cycle, set a "set variable" and choose the "respond" variable. Set the value following the example below:

<img src="./images/11 set variable.png" alt="11" width="800" >

```code
@{items('Apply_to_each')?['message']?['content']}
```

Finally we can parse the output to Copilot Agent:

<img src="./images/12 end.png" alt="12" width="800" >

As a final step, save and publish the flow. The entire logic flow must appear like that:

<img src="./images/12 flow.png" alt="flow" width="600" >

Go back to continue [Configuration Link](../../README.md#flow2)


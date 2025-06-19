<h3>Azure UpdateManager Copilot Studio Flow - OneTimeUpdate</h3>
 
<img src="./images/1 start.png" alt="1" width="800" >

Click on "+" and create "Initialize Variable" :

<img src="./images/2 query graph.png" alt="2" width="800" >

Call the variable "querygraph", set it as a String and past the following query as a value:

```querygraph
resources
| where name =~ "@{triggerBody()?['text']}"
```

Click again on "+" and select again "Initialize Variable", call it "apiversion" :

<img src="./images/3 create api version varaible.png" alt="3" width="800" >

After the variables creation place an "HTTP request" block and fill all the required information:

<img src="./images/4 HTTP Request.png" alt="HTTPRequest" width="800" >

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

<img src="./images/6 machineid.png" alt="6" width="800" >

```function
body('Parse_JSON')?['data']?[0]?['id']
```

Create a condition block:

<img src="./images/7 condition.png" alt="7" width="800" >

In the false section set the variable "api version azmv" following the example:

<img src="./images/7 false.png" alt="7" width="800" >

In the True section set the variable "api version ARC" following the example:

<img src="./images/7 true.png" alt="7" width="800" >

Create invoke resource operation and fill the required parameters following the example:

<img src="./images/8 invoke resource.png" alt="8" width="800" >

```code
Subscritpion: body('Parse_JSON')?['data']?[0]?['subscriptionId']
Resource Group: body('Parse_JSON')?['data']?[0]?['resourceGroup']
Resource Provider: first(split(body('Parse_JSON')?['data']?[0]?['type'], '/'))
Short Resource ID: machines/@{triggerBody()?['text']}
Client Api Version: @{variables('apiversion')}
Action name: installPatches

Body:
{
  "maximumDuration": "PT120M",
  "rebootSetting": "IfRequired",
  "windowsParameters": {
    "classificationsToInclude": [
      "Security",
      "UpdateRollup",
      "FeaturePack",
      "ServicePack",
      "Critical",
      "Definition",
      "Tools",
      "Updates"
    ]
  }
}
```
Now we need to create a "StatusCode" variable :

<img src="./images/9 status code.png" alt="9" width="800" >

Create a condition block:

<img src="./images/10 condition.png" alt="7" width="800" >

In the false section set the variable "StatusCode" following the example:

<img src="./images/10 false.png" alt="10" width="800" >

In the True section set the variable "StatusCode" following the example:

<img src="./images/10 true.png" alt="10" width="800" >

Finally we can paste the output to Copilot Agent:

<img src="./images/11 end.png" alt="11" width="800" >

As a final step, save and publish the flow. The entire logic flow must appear like that:

<img src="./images/flow3.png" alt="flow" width="600" >

Go back to continue [Configuration Link](../../README.md#flow3)


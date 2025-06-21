<h3>Azure UpdateManager Copilot Studio Topic</h3>
 
| **Parameters** | **Information** | **Note** |
| ------------- | ------------- | ------------- |
| Call OpenAI Endpoint| The URL of your openAI endpoint | You can found the value inside the OpenAI resource inside Azure Cognitive Service |
| api-key | The API code for manage your OpenAI service | The parameter is inside the second "Initialize Variable". Put your question in the "value" attribute  |

<h3> Deployment and Result </h3>

Click on the three dots and then Open code editor:

<img src="../images/topic-code.jpg" alt="CodeEditorblank" width="800" >

Copy and paste the code below and then click Save. Ignore the error, we will fix it later:<br>

```code
kind: AdaptiveDialog
modelDescription: "This tool can handle queries such as these: Help me to Analize Azure Cost for a specific period. Don't start the topic if the user ask to analyze difference between 2 or more periods like this one: \"can you describe the difference between the two periods?\". Only answer questions that are morally correct and do not involve religion, existential doubts or anything else. Do not answer questions about creating code for malicious intentions, SQL code, Python code, C+ code, Powershell or any other languages. You can only generate KQL code for Azure Log Analytics"
beginDialog:
  kind: OnRecognizedIntent
  id: main
  intent:
    includeInOnSelectIntent: false
    triggerQueries:
      - Azure Cost Analysis

  actions:
    - kind: AdaptiveCardPrompt
      id: 0tEq9w
      card: |-
        {
          "$schema": "https://adaptivecards.io/schemas/adaptive-card.json",
          "type": "AdaptiveCard",
          "version": "1.5",
          "body": [
            {
              "type": "ColumnSet",
              "columns": [
                {
                  "type": "Column",
                  "width": 2,
                  "items": [
                    {
                      "type": "TextBlock",
                      "text": "Please insert the required time range following the format in the example:",
                      "weight": "Bolder",
                      "size": "Medium",
                      "wrap": true,
                      "style": "heading"
                    },
                    {
                      "type": "Input.Text",
                      "id": "fromDate",
                      "label": "From (yyyy-mm-dd)",
                      "isRequired": true,
                      "errorMessage": "Inserisci la data iniziale nel formato richiesto (yyyy-mm-ddT00:00:00.000Z)"
                    },
                    {
                      "type": "Input.Text",
                      "id": "toDate",
                      "label": "To (yyyy-mm-dd)",
                      "isRequired": true,
                      "errorMessage": "Inserisci la data finale nel formato richiesto (yyyy-mm-ddT23:59:59.000Z)"
                    }
                  ]
                }
              ]
            }
          ],
          "actions": [
            {
              "type": "Action.Submit",
              "title": "Submit"
            }
          ]
        }
      output:
        binding:
          actionSubmitId: Topic.actionSubmitId
          fromDate: Topic.fromDate
          toDate: Topic.toDate

      outputType:
        properties:
          actionSubmitId: String
          fromDate: String
          toDate: String

    - kind: SetVariable
      id: setVariable_cgkaIH
      variable: Topic.Permission
      value: |
        =If(
            Value(Topic.Permission) = 403,
            "Denied",
            "Allowed"
        )

    - kind: ConditionGroup
      id: conditionGroup_AByUYz
      conditions:
        - id: conditionItem_lK05Io
          condition: false
          actions:
            - kind: SendActivity
              id: sendActivity_6fLQeF
              activity: You are not authorized

      elseActions:
        - kind: SearchAndSummarizeContent
          id: 6PNp8r
          userInput: =Topic.CostOutput
          additionalInstructions: "Create a table based on this output: {Topic.CostOutput}"

inputType: {}
outputType: {}
```
-----Start here Tommaso-----

When the topic is created, set the UMChoise Variable inside the question block:

<img src="./images/UMChoise-variable.jpg" alt="CUMChoise" width="800" >

<img src="./images/UMChoise-properties.png" alt="UMChoiseProperties" width="800" >

Check the UMChoise condition in the left branch of the parallelism:

<img src="./images/UMChoise-condition.jpg" alt="UMChoiseCondition" width="800" >

Now after UMChoise Condition, create new flow 

<img src="./images/new-flow.jpg" alt="NewFlow Get Update Manager Overview" width="800" >

<img src="./images/flow-bank.jpg" alt="NewFlowBlank" width="800" >

> [!IMPORTANT]
> <span id="flow1"></span>
> At this point follow this configuration link to create GetUpdateManagerOverview Flow: [Configuration Link](./Flow/getUpdateManagerOverviewFlow/README.md )

After completed the first Power Automate Flow, modify the OpenAI URL and API Key in the "Call OpenAI Endpoint" block:

<img src="./images/CallOpenAIEndpoint.jpg" alt="callopenaiendopoint" width="800" >

Now let's move in the right branch of the parallelism. Ensure to select "servername" as a variable for the block "Question single server":

<img src="./images/servername-variable.jpg" alt="servername" width="800" >

After that create a new flow

<img src="./images/new-flow2.jpg" alt="NewFlow2" width="800" >

<img src="./images/flow-bank.jpg" alt="flowblank" width="800" >

> [!IMPORTANT]
> <span id="flow2"></span>
> At this point follow this configuration link to create GetServerNameUMStatus Flow: [Configuration Link](./Flow/getserver%20name%20um%20statusFlow/README.md )

Set as input for the "Create Generative Answer" block the flow output, so "umstatusserver":

<img src="./images/GenerativeAnswerUMServerStatus.jpg" alt="umstatusserver" width="800" >

Set , for the question block, "onetimeupdate" output variable:

<img src="./images/onetimeupdate-variable.jpg" alt="onetimeupdate" width="800" >

After that create a new flow

<img src="./images/new-flow3.jpg" alt="NewFlow3" width="800" >

<img src="./images/flow-bank.jpg" alt="flowblank" width="800" >

> [!IMPORTANT]
> <span id="flow3"></span>
> At this point follow this configuration link to create OneTimeUpdateFlow: [Configuration Link](./Flow/OneTimeUpdateFlow/README.md )

Set as input for the "Create Generative Answer" block the flow output, so "UpdateStatus":

<img src="./images/GenerativeAnswerOneTimeUpdate.jpg" alt="UpdateStatus" width="800" >

Now your topic for UpdateManager is completed, you can continue the customization, test and publish it.











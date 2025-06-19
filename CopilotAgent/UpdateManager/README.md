<h3>Azure UpdateManager Copilot Studio Topic</h3>
 
| **Parameters** | **Information** | **Note** |
| ------------- | ------------- | ------------- |
| Call OpenAI Endpoint| The URL of your openAI endpoint | You can found the value inside the OpenAI resource inside Azure Cognitive Service |
| api-key | The API code for manage your OpenAI service | The parameter is inside the second "Initialize Variable". Put your question in the "value" attribute  |

<h3> Deployment and Result </h3>

Click on the three dots and then Open code editor:

<img src="../images/topic-code.jpg" alt="CodeEditorblank" width="800" >

Copy and paste the code below and then click Save:<br>
```code
kind: AdaptiveDialog
modelDescription: Help me with Azure Update Manager, or orchestrate patching and AUM
beginDialog:
  kind: OnRecognizedIntent
  id: main
  intent:
    triggerQueries:
      - AUM
      - Azure Update Manager
 
  actions:
    - kind: Question
      id: question_YTqKbG
      interruptionPolicy:
        allowInterruption: true
 
      variable: init:Topic.UMChoise
      prompt: Do you want a general overview or view a specific server update status?
      entity:
        kind: EmbeddedEntity
        definition:
          kind: ClosedListEntity
          items:
            - id: General Overview
              displayName: General Overview
 
            - id: Single Server
              displayName: Single Server
 
    - kind: ConditionGroup
      id: conditionGroup_uvZHXF
      conditions:
        - id: conditionItem_tbxfED
          condition: =Topic.UMChoise = 'crc7c_agent.topic.AIforOPSAUM.main.question_YTqKbG'.'General Overview'
          actions:
            - kind: HttpRequestAction
              id: Qpgspk
              displayName: Call OpenAI Endpoint
              method: Post
              url: set-OpenAI-Endpoint
              headers:
                api-key: replace
                Content-Type: application/json
 
              body:
                kind: JsonRequestContent
                content: |
                  ={
                    messages: [
                      {
                        content: "You are a Azure Update Manager Administrator.Respond with a list",
                        role: "user"
                      },
                      {
                        content: Topic.graphresult,
                        role: "user"
                      }
                    ],
                    max_completion_tokens: 2000,
                    model: "o3-mini"
                  }
 
              response: Global.OpenAIOutput
              responseSchema: Any
 
            - kind: SendActivity
              id: sendActivity_HliD4h
              activity: "{Text(Index(Global.OpenAIOutput.choices, 1).message.content)}"
 
      elseActions:
        - kind: Question
          id: question_zoIoxC
          displayName: Question single server
          interruptionPolicy:
            allowInterruption: true
 
          alwaysPrompt: false
          variable: init:Topic.servername
          prompt: What is the server Name?
          entity: StringPrebuiltEntity
 
        - kind: SearchAndSummarizeContent
          id: nTBuGR
          userInput: =Topic.umstatusserver
          additionalInstructions: |-
            Describe the pending updates using this output:
            {Topic.umstatusserver}
 
        - kind: Question
          id: question_kbxpDC
          interruptionPolicy:
            allowInterruption: true
 
          variable: init:Topic.onetimeupdate
          prompt: How do you wont to procede?
          entity:
            kind: EmbeddedEntity
            definition:
              kind: ClosedListEntity
              items:
                - id: OneTimeUpdate
                  displayName: OneTimeUpdate
 
                - id: No Action
                  displayName: No Action
 
        - kind: ConditionGroup
          id: conditionGroup_WAEKbe
          conditions:
            - id: conditionItem_BIHv5a
              condition: =Topic.onetimeupdate = 'crc7c_agent.topic.AIforOPSAUM.main.question_kbxpDC'.OneTimeUpdate
              actions:
                - kind: SearchAndSummarizeContent
                  id: IHw6za
                  userInput: =Topic.UpdateStatus
                  additionalInstructions: |-
                    {Topic.UpdateStatus} = 200 means update in Running, else an error occurred with the update. If the value is 200 remember to customer to check on Azure Update Manager, else ask to customer to check user permissions only on Azure. Avoid any other information relatad WSUS and so on. Follow the example below:
                    - {Topic.UpdateStatus} = 200 : "The Update is in Progress."
                    - {Topic.UpdateStatus} = 403 : "Action denied. Check User Permissions."
 
          elseActions:
            - kind: SendActivity
              id: sendActivity_DXGYYd
              activity: OK no action
 
    - kind: Question
      id: question_CYQnuk
      interruptionPolicy:
        allowInterruption: true
 
      variable: init:Topic.EndConversation
      prompt: Do you want further help?
      entity:
        kind: EmbeddedEntity
        definition:
          kind: ClosedListEntity
          items:
            - id: Yes
              displayName: Yes
 
            - id: No
              displayName: No
 
    - kind: ConditionGroup
      id: conditionGroup_Z0kVKo
      conditions:
        - id: conditionItem_dcSLGm
          condition: =Topic.EndConversation = 'crc7c_agent.topic.AIforOPSAUM.main.question_CYQnuk'.Yes
          actions:
            - kind: SendActivity
              id: sendActivity_qH5K0e
              activity: OK
 
            - kind: GotoAction
              id: Q7f71L
              actionId: question_YTqKbG
 
      elseActions:
        - kind: BeginDialog
          id: sErToI
          dialog: crc7c_agent.topic.EndofConversation
 
inputType: {}
outputType: {}
```

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











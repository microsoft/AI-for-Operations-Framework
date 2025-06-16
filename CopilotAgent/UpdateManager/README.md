<h3>Azure UpdateManager Copilot Studio Topic</h3>
 
| **Parameters** | **Information** | **Note** |
| ------------- | ------------- | ------------- |
| Call OpenAI Endpoint| The URL of your openAI endpoint | You can found the value inside the OpenAI resource inside Azure Cognitive Service |
| repreplacewithRG | Connection setting during deployment | Replace with the selected RG Name for the deployment |
| api-key | The API code for manage your OpenAI service | The parameter is inside the second "Initialize Variable". Put your question in the "value" attribute  |
| changemodelname | Insert the model name | You can found the value inside the OpenAI resource inside Azure Cognitive Service |

<h3> Deployment and Result </h3>

Click on the three dots and then Open code editor:

![CodeEditorank](../images/topic-code.jpg )


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






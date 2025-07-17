<h3>Microsoft Azure AI for Operation Framework Copilot Agent</h3>


 <h2>Prerequisites</h2>
 

Required licenses:
 - Copilot Studio Developer Licenses (Microsoft 365 Copilot Studio) | 1 * Developer
 - Power Automate Premium | 1 * User
 - Microsoft Teams | 1* User

Required Solution:
- Copilot Studio Agent

<h2>Copilot Studio creation</h2>
 
Before proceeding with Copilot Studio Agent creation, ensure to have assigned the required Copilot Studio Developer Licenses in <a href="https://admin.microsoft.com/#/homepage">"Microsoft 365 Admin Center"</a>. After that continue with the agent creation.

Open <a href="https://copilotstudio.microsoft.com/">"Microsoft Copilot Studio Portal"</a>. Click on New Agent: 

![New Agent](./images/new-agent.jpg )

In the top right corner, click on Skip to configure:

![skip](./images/skip.jpg )

In this page fill all required sections and then click on create:

![Create agent](./images/agent-name.jpg )

Disable the GenAI orchestration clicking on Settings and set No:

![Setting](./images/Setting.jpg )

![Response](./images/Setting-response.jpg )


At this point the new agent has been created. On the headers click on Topic, and then add topic :

![Topic](./images/add-topic.jpg )

From blank

![FromBlank](./images/from-blank.jpg )

Follow the istructions below:


**Solution Name** | **Information** | **Configuration** |
| ------------- | ------------- | ------------- |
| Copilot topic Update Manager | Topic used to integrate Update Manager with Copilot Studio | [Configuration Link](./UpdateManager/README.md) |
| Copilot topic FinOps | Topic used to integrate FinOps to Copilot Studio | [Configuration Link](./FinOps/README.md) |



 

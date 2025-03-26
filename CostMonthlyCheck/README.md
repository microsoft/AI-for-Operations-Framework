<h3>Azure Cost Monthly Check OpenAI integration: Configuration</h3>
 
| **Parameters** | **Information** | **Note** |
| ------------- | ------------- | ------------- |
| replacewithsubid | Connection setting during deployment | Replace with your Subscription ID |
| repreplacewithRG | Connection setting during deployment | Replace with the selected RG Name for the deployment |
| api-key | The API code for manage your OpenAI service | The parameter is inside the second "Initialize Variable". Put your question in the "value" attribute  |
| changeendpointname | Insert the OpenAI endpoint name | You can found the value inside the OpenAI resource inside Azure Cognitive Service |
| changemodelname | Insert the model name | You can found the value inside the OpenAI resource inside Azure Cognitive Service |

<h3> Important </h3>
 This LogApp and the following changes are an example of Cost Monitor check with OpenAI send via Email . 
 
 
<h3>Required Identity</h3>
<h4>Managed Identity</h4>

Now configure the HTTP request to the Graph Explorer enabling the authentication via System Assigned Managed Identity. Please remind that the Managed Identity need to have the righ permission on the subscription for read the resources:


![identity](./images/identity.jpg)


<h3> Deployment and Result </h3>
 
After deployment completed, please follow the documentation:


As first step please configure the required recurrence:

![recurrence](./images/Recurrence.jpg)

Configure the Api Key with the value inside your OpenAI Service:

![Api Key](./images/ApiKey.jpg)

Modify the required subscription id in each the HTTP connector in order to obtain the required information:

![Api Key](./images/Sub-Id.jpeg)

At this point we need to configure the Ask to OpenAI module replacing the required parameters:

![OpenAI](./images/OpenAI.jpg)

Last step is to add a notification section.
In the example below we have a "Send Email V2" connector for send the final report to the required people or to a Teams channel . If you want to follow the same approach configure the module following the same example adding them at the end of the Logic App. Make sure to use the correct variable in the body of the email in order to have them correctly formatted:

![example](./images/example-notification.jpg)
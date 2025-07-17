<h3>Azure UpdateManager Copilot Studio Flow - CheckPermission</h3>

Click on "+" and select "Read a subscription" block:

<img src="./images/1 read subscription.png" alt="1" width="800" >

Fill the "Subscription" parameter using the SubID you whant to check. YOu can dynamicalli populate it passing the SubID from the Copilot Topic inside a String variable.

After that pass the block "Status code" to the "Respond to the agent" block and call the variable "Permission":

<img src="./images/2 status code.png" alt="2" width="800" >

```code
@{outputs('Read_a_subscription')?['statusCode']}
```

Remember to change the Settings for the "Respond to agent" block in order to run the block in all the cases:

<img src="./images/3 setting.png" alt="3" width="800" >

Go back to continue [Configuration Link](../../README.md#finopsflowone)
# Configuring NinjaOne

### Oauth config
In the lefthand panel browse to:
	<i class="fas fa-cog" aria-hidden="true"></i>Administration {: #cog}
	-> **Apps**
	-> API

Go to the second option (**Client App IDs**) and press `Add` in the top right.

For the new app select the following options:

Application platform: API Service (Machine-to-machine)  
Name: Anything descriptive so you know what this API key is for later, this is not visible in Teams.  
Redirect Urls: HTTPS:// + (The URL of your bot) + /api/ninjaouath  
    for example https://kabutobot.tailspintoys.com/api/ninjaoauth  
    **NOTE** This **MUST** match the value of `NinjaOauthRedirectUrl` in your  
    appsettings.json/enviroment (depending on if you are using the application as a docker image or a daemon)  

Scopes: Select only `Monitoring` and `Management` 
Allowed Grant Types:  
    `Authorization Code` - So an admin can grant the bot access.  
    `Refresh Token` - So that the bot can stay connected and re-connect after the application restarts.
    You can revoke the bots access by deleting the token under the `OAuth Tokens` tab.


Next, go to:  
    Administration
    -> **Apps**
    -> Notification Channels

Press `Add` in the top right  

* Make sure `Enabled` is toggled  
Name: "Teams message" (Or anything, use a descriptive name if you have multiple webhooks configured)  
Description: "Integration with microsoft Teams"  
Webhook URL: HTTPS:// + (The URL of your bot) + api/ninjawebhook  
    for example https://kabutobot.tailspintoys.com/api/ninjawebhook  
    *This is where ninjaOne will send the messages that will end up as cards in your teams channel.*

*If your bot is offline or a problem occours with the webhook NinjaOne disables it automatically.*  
*Just go back to the webhooks page, click your previously created webhook and enable it with the toggle again.*

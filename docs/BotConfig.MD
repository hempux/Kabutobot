### Configuring the teams bot

the bot requires multiple configuration parameters to run (and will throw errors in the console for parts of the config that you should doublecheck if something is missing)

A typical appsettings.json looks like this:

```` 
{
  "applicationUrl": "http://localhost:3978",
  "MicrosoftAppId": "4c8fde15-6aa2-41fa-95f1-de93434a24be",
  "MicrosoftAppPassword": "S9VaaUWpy~5YDEYJsmTtKBcmlxVh2ST17WXM~UQd0vHI8jJFCM0IsCPG",
  "MicrosoftBotendpoint": "https://kabutobot.tailwindtoys.tld/api/messages",
  "MicrosoftTeamsServiceURL": "https://smba.trafficmanager.net/emea/",
  "MicrosoftTeamsChannel": "19:6gg35fa86a643ee1a49b4dgbg32eee@thread.skype",
  "NinjaClientId": "U8A57uXG64l68i9HlkI2LN4Vg6v",
  "NinjaClientSecret": "yhBBmB9CSfJ62GKhHU8A57uXG64l68i9HlkI2LN4Vg6vQ8uGov1bj1",
  "NinjaEndpointUrl": "https://eu-api.ninjarmm.com",
  "NinjaOauthRedirectUrl": "https://kabutobot.tailwindtoys.tld/api/ninjaoauth"
  ...
}
````
If you want the database file to have a different name, add a value named `SqliteDatabase`
````
{
...
"SqliteDatabase": "kabutobot.sqlite",
....
}
````
### Azure keyvault
To access to your NinjaOne instance the bot needs an ouath refresh token(code), that is stored in the sqlite database,  
If someone unauthorized got access to your appsettings.json and sqlite file it is possible they could create their own authorization codes if your app is imroperly configured.  
And then have access to your NinjaOne instance with the user credentials of the person that authorized the bot.

A safer way [(and cheap, a few cents per month)](https://azure.microsoft.com/en-us/pricing/details/key-vault/) to store your refresh token is to use azure keyvault, to do this just add the following to your appsettings.json:  

UseAzureKeyVault: true // *Ommiting this and just leaving the below values filled causes the vault feature not to be loaded.*  
AZURE_VAULT_URL: <adress to your keyvault>  
AZURE_CLIENT_ID: (optional, if you leave this blank MicrosoftAppId will be used but you will need to configure your vault so that the app that is created in your AzureAD tenant by the teams developer portal have "Set / Get secret" permissions.) 
AZURE_CLIENT_SECRET - required with AZURE_CLIENT_ID, otherwise omit this as well.
```` 
{
  ...
"UseAzureKeyVault": "true",
"AZURE_VAULT_URL": "https://tailspoinbot.vault.azure.net/",
"AZURE_CLIENT_ID": "3856e071-49b1-4aaf-a956-60d7b13c9d77",
"AZURE_CLIENT_SECRET": "a2xsWuFm85U2Q~w3IkNYqc69NpcgdQIopumnYsncHC0L~9YRHMwUskjOn2Az9N",
  ...
}
````

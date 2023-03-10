# Kabuto aka NinjaRmm webhook-bridge
![Screenshot of sample card](docs/Sample_adaptivecard.png?raw=true)

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 6.0

  ```bash
  # determine dotnet version
  dotnet --version
  ```

## To run the bot

- Run the bot from a terminal or from Visual Studio/VS Code:

  ```bash
  # run the bot
  dotnet run
  ```


## Testing the bot using Bot Framework Emulator

[Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) is a desktop application that allows bot developers to test and debug their bots on localhost or running remotely through a tunnel.

- Install the latest Bot Framework Emulator from [here](https://github.com/Microsoft/BotFramework-Emulator/releases)

### Connect to the bot using Bot Framework Emulator

**For this to work you will either have to leave `MicrosfotBotAppId` and `MicrosoftAppPassword` blank, or supply those values while connecting with the emulator, otherwise you will recieve a "401 Unauthorized" error.**


- Launch Bot Framework Emulator
- File -> Open Bot
- Enter a Bot URL of `http://localhost:3978/api/messages` (Or port `5080` if you are using docker-compose)

### Using curl/powershell to test incomming messages

- Send a get request to `http://localhost:3978/api/ninjawebhook`  to simulate notifucations that the NinjaOne RMM service would send.

   ```bash
    curl -X POST --data-binary ./examples/WebhookDeviceMessage.json http://localhost:3978/api/ninjawebhook
   ```

   ```powershell
   Invoke-RestMethod -Uri http://localhost:3978/api/ninjawebhook -Body (Get-Content .\examples\WebhookDeviceMessage.json) -Method Post -ContentType "application/json"
   ```

- Using the Bot Framework Emulator, notice a message was sent to the user from the bot.

### Building

Run the following, replacing `<rid>` with the platforming you want to run it on.  
Use `win-x64` for windows and `debian-x64` for debian-based systems like ubuntu.
``` 
dotnet publish .\Kabutobot.csproj -r <rid> -o ..\build
```
This will create a self-contained executable for that platform in the top folder (next to `src`)

## Setting up the bot.

Visit the [Bot Framework developer portal](https://dev.botframework.com/bots/new) to register a new bot so that teams knows how to communicate with your bot.  
To set up the bot and select what channel the ninjaRMM messages will be sent to you'll have to configure `MicrosoftTeamsChannel` and `MicrosoftServiceURL`

## Configururing the application
The bot has many configuration parameterers that needs to be set for everything to work, depending on how you want to use it you will either have to use `appsettings.json` or enviroment variables with docker.

### With docker 
see the [Docker section](docs/Dockersupport.md) page for details about configuring the docker image and docker-compose.yml.  

### As a local app
see the [Bot settings](docs/BotConfig.MD) page for details about `appsettings.json`.  



## Post-configuration - teams channel-config.
1. Make sure that the variable `ASPNET_ENVIRONMENT` is set to `development`
2. Start the bot and @Mention it with the message 'channelinfo' ( like `@kabutobot channelinfo` ) 
to get a list of all the valid channels and their MicrosoftTeamsChannel

![channelinfo command screenshot](docs/channelinfo_command.png?raw=true)  
4. Update appsettings.json / docker-compose.yml with the channelinfo the bot outputs, after this all you have to do is to select what acitivites in ninja that should send notifications to to the bot.


# Configuring Ninja

Go to the dedicated [Configuring Ninja section](docs/Ninjasetup.MD)


# Troubleshooting
`AADSTS700016`  Application with identifier '\<your Micrcrosoft App ID>\' was not found in the directory 'botframework.com'
* Sign into the azure portal and go to the [App registrations](https://portal.azure.com/#blade/Microsoft_AAD_RegisteredApps/ApplicationsListBlade) and click `Authentication` in the left side panel, verify that the `Supported account types` is set to `Accounts in any organizational directory (Any Azure AD directory - Multitenant)` so that the bot framework can authenticate your bot.

`AADSTS7000215`
* Check that `appsettings.json` has the correct `MicrosoftAppPassword`

## Extending the bots functionality


Visit the [APIdocs](https://app.ninjarmm.com/apidocs/?links.active=core) page to see all the current API endpoint that NinjaOne supports

There is a [dedicated section](docs/Apicall.MD) in the docs on how a ninja API call works from this application that should give you an idea of how it works and how to implement more features.

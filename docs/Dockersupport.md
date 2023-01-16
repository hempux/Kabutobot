# Running with docker

You can run and install/configure the application directly through docker without installing dotnet.

To create the local docker image run the following command
```
docker build . -t kabutobot:latest 
```
`-t kabutobot:latest` can be changed into whatever image/tag you want to use.  

Next, configure your `docker-compose.yml` file and docker will use the `image` you built previously to launch.   
The `volumes` section configutes what local folder will be used to store the files the bot needs to write to disk in the format (local folder):(folder inside the container)  

`ASPNETCORE_ENVIRONMENT` only needs to be set to `Development` if you want verbose output.  
`published` port is the external port you want to proxy the app to.
```
version: '3.4'

services:
  kabutobot:
    image: kabutobot:latest
    environment:
       - ASPNETCORE_ENVIRONMENT=Development
       - SqliteDatabase=
       - MicrosoftAppId=
       - MicrosoftAppPassword=
       - MicrosoftBotendpoint=
       - MicrosoftTeamsServiceURL=
       - MicrosoftTeamsChannel=
       - NinjaClientId=
       - NinjaClientSecret=
       - NinjaEndpointUrl=
       - NinjaOauthRedirectUrl=
    ports:
    - target: 80
      published: 5080
      protocol: tcp
      mode: host
    volumes:
      - ~/kabuto:/app/data
    networks:
      default:
`` 

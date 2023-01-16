#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS publish
WORKDIR /src
COPY ["Kabutobot.csproj", "."]
COPY . .
RUN dotnet restore "Kabutobot.csproj" --runtime alpine-x64

RUN dotnet publish "Kabutobot.csproj" -c Release -o /app/publish \
    --no-restore \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishTrimmed=false \
    /p:PublishSingleFile=true


# use different image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine AS final
WORKDIR /app

# create a new user and change directory ownership
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app

# impersonate into the new user
USER dotnetuser
WORKDIR /app

# just expose port 80

EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["./KabutoBot"]

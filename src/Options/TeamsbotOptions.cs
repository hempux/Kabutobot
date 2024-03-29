﻿using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace net.hempux.kabuto.Teamsoptions
{
    public static class TeamsbotOptions
    {
        private static IConfiguration configuration;
        private static readonly IConfigurationSection botOptions;
        private static MicrosoftAppCredentials _appCredentials;
        private static string MicrosoftAppTenantId;
        private static string MicrosoftAppType;
        public static void Initialize(IConfiguration Configuration)
        {

            configuration = Configuration;


            Appid = configuration.GetValue<string>("MicrosoftAppId");
            AppSecret = configuration.GetValue<string>("MicrosoftAppPassword");
            Botendpoint = configuration.GetValue<string>("MicrosoftBotendpoint");
            MicrosoftAppTenantId = configuration.GetValue<string>("MicrosoftAppTenantId");
            MicrosoftAppType = configuration.GetValue<string>("MicrosoftAppType") ?? "MultiTenant";
            // Set Teamschannel and TeamsServiceUrl to string.Empty if we want to use bot emulator.

            TeamsChannel = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") ?
                configuration.GetValue<string>("MicrosoftTeamsChannel") ?? string.Empty :
                configuration.GetValue<string>("MicrosoftTeamsChannel");
            TeamsServiceUrl = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") ?
                configuration.GetValue<string>("MicrosoftTeamsServiceURL") ?? string.Empty :
                configuration.GetValue<string>("MicrosoftTeamsServiceURL");

            if (configuration.GetValue<string>("MicrosoftAppType") == "UserAssignedMSI")
            {
                Log.Fatal("UserAssignedMSI not yet implemented for teams bot outgoing messages");
            }
            else
            {
                _appCredentials = new MicrosoftAppCredentials(Appid, AppSecret);
            }

        }

        public static string Appid { get; set; }
        public static string AppSecret { get; set; }
        public static string Botendpoint { get; set; }
        public static string TeamsServiceUrl { get; set; }
        public static string TeamsChannel { get; set; }

        public static MicrosoftAppCredentials AppCredentials
        {

            get
            {
                return _appCredentials;
            }


        }
    }
}


using Microsoft.Extensions.Configuration;
using net.hempux.kabuto.Utilities;
using Azure.Identity;
using Serilog;
using System;
using Azure.Core;
using System.Collections.Generic;
using net.hempux.kabuto.Database;

namespace net.hempux.kabuto.VaultOptions
{
    public static class KeyVaultOptions
    {
        private static IConfiguration configuration;
        private static string AZURE_CLIENT_SECRET;
        public static bool AZURE_PASSWORD_IS_BLANK = true;
        public static string AZURE_CLIENT_ID { get; private set; }
        public static string AZURE_TENANT_ID { get; private set; }
        public static string AZURE_VAULT_URL {get;private set;}
        public static TokenCredential Azurecredentials {get;private set;}


        public static void Initialize(IConfiguration Configuration)
        {

            configuration = Configuration;

            if(configuration.GetValue<string>("UseAzureKeyVault") != "true")
            {
                Keystore.Init(false);
                return;
            }

            if (configuration.GetValue<string>("MicrosoftAppId") != null &&
            configuration.GetValue<string>("AZURE_CLIENT_ID") == null)
            {
                Log.Information("Using MicrosoftAppId for Vault authentication");
                Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", configuration.GetValue<string>("MicrosoftAppId"));
                Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", configuration.GetValue<string>("MicrosoftAppPassword"));

            }

            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", "MicrosoftAppTenantId");
            AZURE_TENANT_ID = configuration.GetValue<string>("MicrosoftAppTenantId");
            AZURE_VAULT_URL = configuration.GetValue<string>("AZURE_VAULT_URL");

            Azurecredentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                AdditionallyAllowedTenants = { configuration.GetValue<string>("MicrosoftAppTenantId") }
            });


            }
        }

    }
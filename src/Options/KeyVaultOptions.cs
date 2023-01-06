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
        private static string AZURE_CLIENT_ID;
        private static string AZURE_TENANT_ID;
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
                Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", configuration.GetValue<string>("MicrosoftAppId"));

            if (configuration.GetValue<string>("MicrosoftAppPassword") != null &&
            configuration.GetValue<string>("AZURE_CLIENT_SECRET") == null)
                Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", configuration.GetValue<string>("MicrosoftAppPassword"));

            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", "MicrosoftAppTenantId");
            AZURE_TENANT_ID = configuration.GetValue<string>("MicrosoftAppTenantId");
            AZURE_VAULT_URL = configuration.GetValue<string>("AZURE_VAULT_URL");


            List<string> validations = new List<string>();
            if (configuration.GetValue<string>("MicrosoftAppType") == "UserAssignedMSI")
            {
                Azurecredentials = new ManagedIdentityCredential(clientId: AZURE_CLIENT_ID);
                validations.AddRange(new List<string> { AZURE_CLIENT_ID, AZURE_VAULT_URL });
            
            }
            else
            {
                Azurecredentials = new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    AdditionallyAllowedTenants = { configuration.GetValue<string>("MicrosoftAppTenantId") }
                });
                validations.AddRange(new List<string> { AZURE_TENANT_ID, AZURE_VAULT_URL });

            }


            try
            {
                foreach (object val in validations)
                    if (val == null) { throw new ArgumentNullException(); }
            }
            catch
            {
                Log.Error(
                     "Make sure the following is present in configuration variables\n" +
                     " - MicrosoftAppId\n "
                
                );

                if (!Utils.InDocker)
                {
                    Log.Error(
                     " - MicrosoftAppPassword\n" +
                     " - MicrosoftAppTenantId\n" + 
                        "Press Enter to exit.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }

                Environment.Exit(1);
            }
        }

    }
}


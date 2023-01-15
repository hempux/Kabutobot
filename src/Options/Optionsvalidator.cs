using Microsoft.Extensions.Configuration;
using net.hempux.kabuto.Ninja;
using net.hempux.kabuto.Teamsoptions;
using net.hempux.kabuto.Utilities;
using net.hempux.kabuto.VaultOptions;
using Serilog;
using System;
using System.Collections.Generic;

namespace net.hempux.kabuto.Options
{
    public class Optionsvalidator
    {
        private static IConfiguration config;
        public static void Validate(IConfiguration Configuration)
        {
            config = Configuration;
            List<string> errors = new List<string>();
            errors.Add("Configuration errors:");
            #region teams bot 
            if (!(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"))
            {
                if (string.IsNullOrEmpty(TeamsbotOptions.Appid))
                    errors.Add(" - MicrosoftAppId is missing.");
                if (string.IsNullOrEmpty(TeamsbotOptions.AppSecret))
                    errors.Add(" - MicrosoftAppPassword is missing.");

            }
            if (string.IsNullOrEmpty(TeamsbotOptions.Botendpoint))
                errors.Add(" - MicrosoftBotendpoint is missing.");

            #endregion
            #region ninja
            if (NinjaOptions.Appid == null)
                errors.Add(" - NinjaClientId is missing.");
            if (NinjaOptions.AppSecret == null)
                errors.Add(" - NinjaClientSecret is missing.");
            if (NinjaOptions.EndpointUrl == null)
                errors.Add(" - NinjaEndpointUrl is missing.");
            if (!Uri.TryCreate(NinjaOptions.EndpointUrl, UriKind.Absolute, out Uri? ninjaendpoint))
                errors.Add(" - NinjaEndpointUrl must be a valid url");
            if (ninjaendpoint?.Scheme != Uri.UriSchemeHttps)
                errors.Add(" - NinjaEndpointUrl must use https://");
            if (NinjaOptions.RedirectUrl == null)
                errors.Add(" - NinjaOauthRedirectUrl is missing.");
            if (!Uri.TryCreate(NinjaOptions.RedirectUrl, UriKind.Absolute, out Uri? redirectUrl))
                errors.Add(" - NinjaOauthRedirectUrl must be a valid url");
            if (redirectUrl?.Scheme != Uri.UriSchemeHttps)
                errors.Add(" - NinjaOauthRedirectUrl must use https://");
            if (NinjaOptions.NinjaInstanceUrl == null)
                errors.Add(" - NinjaInstanceUrl is missing.");
            if (!Uri.TryCreate(NinjaOptions.NinjaInstanceUrl, UriKind.Absolute, out Uri? ninjaOauthRedirectUrl))
                errors.Add(" - NinjaInstanceUrl must be a valid url");
            if (ninjaOauthRedirectUrl?.Scheme != Uri.UriSchemeHttps)
                errors.Add(" - NinjaInstanceUrl must use https://");
            #endregion

            #region keyvault
            if (config.GetValue<string>("UseAzureKeyVault") == "true" || config.GetValue<bool>("UseAzureKeyVault") == true)
            {

                if (string.IsNullOrEmpty(KeyVaultOptions.AZURE_VAULT_URL))
                    errors.Add(" - AZURE_VAULT_URL missing");
                if (!Uri.TryCreate(KeyVaultOptions.AZURE_VAULT_URL, UriKind.Absolute, out Uri? result))
                    errors.Add(" - AZURE_VAULT_URL must be a valid url");
                if (result?.Scheme != Uri.UriSchemeHttps)
                    errors.Add(" - AZURE_VAULT_URL must use https://");

                if (string.IsNullOrEmpty(KeyVaultOptions.AZURE_CLIENT_ID))
                    errors.Add(" - AZURE_CLIENT_ID is missing");
                if (string.IsNullOrEmpty(config.GetValue<string>("AZURE_CLIENT_SECRET")))
                    errors.Add(" - AZURE_CLIENT_SECRET is missing\n");
                if (KeyVaultOptions.AZURE_PASSWORD_IS_BLANK)
                    errors.Add(" - AZURE_CLIENT_PASSWORD is missing or blank");
            }


            if (errors.Count > 1)
            {
                string errormessage = string.Join(Environment.NewLine, errors);
                Log.Fatal(errormessage);

                if (!Utils.InDocker)
                    Console.ReadKey();
                Environment.Exit(1);

            }
            #endregion
        }




    }
}

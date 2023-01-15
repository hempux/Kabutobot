using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using net.hempux.kabuto.VaultOptions;
using Serilog;

namespace Net.Hempux.KabutoBot.Keyvault
{

    public class AzureKeyvault
    {

        private static SecretClient Client { get; set; }
        private static SecretClientOptions Options { get; set; }
        public AzureKeyvault()
        {
            if (Options == null)
                Options = new SecretClientOptions()
                {
                    Retry =
        {
            Delay= System.TimeSpan.FromSeconds(2),
            MaxDelay = System.TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
                };

            if (Client == null)
                Client = new SecretClient(new System.Uri(KeyVaultOptions.AZURE_VAULT_URL), KeyVaultOptions.Azurecredentials, Options);

        }
        public string GetSecret(string name)
        {
            try
            {
                return Client.GetSecret(name).Value.Value;
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException.Message.StartsWith("AADSTS700016"))
                {
                    Log.Error("Could not get secret {secretname}. Azure exception: {ex}\n" +
                        "Verify credentials and tennantID for azure keyvault", name, "AADSTS700016");
                }
                else
                {
                    Log.Error("Could not get secret {secretname}. Azure error: {ex}\n", name, ex.InnerException.Message);
                }
                return null;
            }

        }
        public void SetSecret(string name, string value)
        {
            try
            {
                Client.SetSecret(name, value);
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException.Message.StartsWith("AADSTS700016"))
                {
                    Log.Error("Could not get secret {secretname}. Azure exception: {ex}\n" +
                        "Verify credentials and tennantID for azure keyvault", name, "AADSTS700016");
                }
                else
                {
                    Log.Error("Could not get secret {secretname}. Azure error: {ex}\n", name, ex.InnerException.Message);
                }
            }

        }
    }
}
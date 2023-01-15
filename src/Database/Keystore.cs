using net.hempux.kabuto.database;
using Net.Hempux.KabutoBot.Keyvault;

namespace net.hempux.kabuto.Database
{
    public static class Keystore

    {

        private static AzureKeyvault vault;
        private static bool UseKeyVault;

        public static void Init(bool keyvault)
        {
            UseKeyVault = keyvault;
            if (UseKeyVault)
                vault = new AzureKeyvault();
        }

        public static void SaveKey(string name, string value)
        {
            if (UseKeyVault)
                vault.SetSecret(name, value);
            else
                SqliteEngine.SetPersistentdata(name, value);
        }

        public static string GetKey(string name)
        {
            if (UseKeyVault)
                return vault.GetSecret(name);
            else
                return SqliteEngine.GetPersistentdata(name);
        }


    }
}

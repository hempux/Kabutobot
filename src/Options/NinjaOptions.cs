using Microsoft.Extensions.Configuration;
using net.hempux.kabuto.Utilities;
using Serilog;
using System;

namespace net.hempux.kabuto.Ninja
{
    public static class NinjaOptions
    {
        private static IConfiguration configuration;
        public static void Initialize(IConfiguration Configuration)
        {
            configuration = Configuration;

            Appid = configuration.GetValue<string>("NinjaClientId");
            AppSecret = configuration.GetValue<string>("NinjaClientSecret");
            EndpointUrl = configuration.GetValue<string>("NinjaEndpointUrl");
            RedirectUrl = configuration.GetValue<string>("NinjaOauthRedirectUrl");
            NinjaInstanceUrl = configuration.GetValue<string>("NinjaInstanceUrl");

            //Null validation
            object[] opts = new object[] { Appid, AppSecret, EndpointUrl, RedirectUrl,NinjaInstanceUrl };
            try
            {
                foreach (object o in opts)
                    if (o == null) { throw new ArgumentNullException(); }
                
            }
            catch
            {
                Log.Error(
                    "Make sure the following is present in configuration variables:\n" +
                    " - NinjaClientSecret\n" +
                    " - NinjaClientId\n" +
                    " - NinjaEndpointUrl\n" +
                    " - NinjaOauthRedirectUrl\n" +
                    " - NinjaInstanceUrl\n"
                );

                if (!Utils.InDocker)
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                Environment.Exit(1);
            }
        }

        public static string Appid { get; set; }
        public static string AppSecret { get; set; }
        public static string EndpointUrl { get; set; }
        public static string NinjaInstanceUrl { get; private set; }
        public static string RedirectUrl { get; set; }
    }
}


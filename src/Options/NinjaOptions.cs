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
        }

        public static string Appid { get; set; }
        public static string AppSecret { get; set; }
        public static string EndpointUrl { get; set; }
        public static string NinjaInstanceUrl { get; private set; }
        public static string RedirectUrl { get; set; }
    }
}


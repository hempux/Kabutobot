using Newtonsoft.Json;
using System;

namespace net.hempux.kabuto.Ninja
{
    public class NinjaOauthToken
    {
        public NinjaOauthToken()
        {
            expires_at = DateTime.Now.AddHours(1);
        }
        [JsonProperty]
        public string access_token { get; set; }
        [JsonProperty]
        public string refresh_token { get; set; }
        [JsonProperty]
        public string token_type { get; set; }
        [JsonProperty]
        public int expires_in { get; set; }

        public DateTime expires_at { get; set; }

    }

}

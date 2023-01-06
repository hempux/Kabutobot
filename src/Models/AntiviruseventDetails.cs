// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.


using System.Text.Json.Serialization;

namespace net.hempux.ninjawebhook.Models
{
    internal class AntiviruseventDetails
    {

        [JsonPropertyName("threat_name")]
        public string Threat_name { get; set; }
        [JsonPropertyName("threat_path")]
        public string Threat_path { get; set; }
        [JsonPropertyName("threat_type")]
        public string Threat_type { get; set; }


    }
}
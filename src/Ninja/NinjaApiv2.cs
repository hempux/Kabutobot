using Microsoft.AspNetCore.WebUtilities;
using net.hempux.kabuto.database;
using net.hempux.kabuto.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace net.hempux.kabuto.Ninja
{
    public class NinjaApiv2 : INinjaApiv2
    {
        public NinjaApiv2()
        {
            refreshtoken = Keystore.GetKey("Refresh_token");
            if (string.IsNullOrEmpty(refreshtoken))
                NeedsAuthentication = true;
            else
                NeedsAuthentication = false;
        }

        private static NinjaOauthToken _token;
        private static SqliteEngine engine;
        private static string refreshtoken;
        private static string _msgId;
        public void setSqliteEngine(SqliteEngine Sqliteengine)
        {
            if (engine == null)
                engine = Sqliteengine;
            else
                return;

        }
        public async Task<bool> EnsureTokenExistsAndIsValid()
        {
            if (_token == null || _token.expires_at < DateTime.Now)
            {

                if (!string.IsNullOrEmpty(refreshtoken))
                    try
                    {
                        _token = await GetNewToken(refreshtoken, true);
                        return true;
                    }
                    catch (Exception ex)
                    {

                        if (ex.Message.Contains("invalid_token"))
                        {
                            Log.Error("Invalid token - Requesting reauth");
                            SetMessageId(null);
                            NeedsAuthentication = true;
                            Bot.NinjaMessageBot.TokenInit();
                        }
                        else
                            throw;

                        return false;
                        //throw new Exception($"{{\"Error\":{JsonConvert.ToString(ex.Message)}}}");
                    }
                else
                {
                    if (!NeedsAuthentication)
                    {
                        SetMessageId(null);
                        Bot.NinjaMessageBot.TokenInit();
                    }
                    return false;
                }
            }
            return true;
        }
        public static bool Tokenstatus()
        {
            if (_token == null || _token.expires_at < DateTime.Now)
                return false;
            else
                return true;
        }
        public static bool NeedsAuthentication { get; set; }
        public static string GetmessageId()
        {
            if (_msgId == null)
            {
                var idFromDB = Keystore.GetKey("messageId");
                _msgId = idFromDB;
            }
            return _msgId;
        }
        public static void SetMessageId(string messageId)
        {
            Keystore.SaveKey("messageId", messageId);
            _msgId = messageId;
        }
        public string GetAuthorizationCodeUrl()
        {

            var oauthstuff = new Dictionary<string, string>
                {
                    { "response_type", "code"},
                    { "client_id", NinjaOptions.Appid },
                    { "client_secret", NinjaOptions.AppSecret },
                    { "scope", "monitoring management offline_access" },
                    { "redirect_uri", NinjaOptions.RedirectUrl }
                };
            var uri = QueryHelpers.AddQueryString(string.Concat(NinjaOptions.EndpointUrl, NinjaApiEndpoint.oauthCode), oauthstuff);
            return uri;
        }
        public async Task<NinjaOauthToken> GetOauthToken(string code)
        {
            Log.Information("Checking DB for refresh token");
            refreshtoken = SqliteEngine.GetPersistentdata("Refresh_token");

            if (string.IsNullOrEmpty(refreshtoken))
            {

                OauthModel oauthtoken = engine.Storedtoken();

                if (oauthtoken == null)
                    _token = await GetNewToken(code);

                refreshtoken = oauthtoken.Refresh_token;

                _token = new NinjaOauthToken()
                {
                    access_token = oauthtoken.Token,
                    expires_in = int.Parse(DateTimeOffset.Now.ToUnixTimeSeconds().ToString()) - oauthtoken.Expires_at, // - DateTimeOffset.FromUnixTimeSeconds(oauthtoken.Expires_at)
                    refresh_token = oauthtoken.Refresh_token,
                    expires_at = DateTimeOffset.FromUnixTimeSeconds(oauthtoken.Expires_at).UtcDateTime.ToLocalTime()
                };
            }

            if (null == _token || _token.expires_at < DateTime.Now)
            {
                if (!string.IsNullOrEmpty(code))
                    _token = await GetNewToken(code);

                if (!string.IsNullOrEmpty(refreshtoken))
                    _token = await GetNewToken(refreshtoken, true);

                refreshtoken = _token.refresh_token;

                engine.Savetoken(new OauthModel
                {
                    Token = _token.access_token,
                    Refresh_token = _token.refresh_token,
                    Expires_at = int.Parse(new DateTimeOffset(_token.expires_at).ToUnixTimeSeconds().ToString())
                });

                Log.Information("Refreshed token");
                return _token;
            }
            else
            {
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
                    Log.Information("Reused existing token");
                return _token;
            }
        }
        public async Task<NinjaOauthToken> CompleteOauthFlow(string code)
        {
            try
            {
                NinjaOauthToken token = await GetNewToken(code);
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
                    Log.Information("Got new oauth token from interactive flow(user sign-in)");
                _token = token;
                refreshtoken = token.refresh_token;

                NeedsAuthentication = false;
                Keystore.SaveKey("messageId", "");
                Keystore.SaveKey("Refresh_token", _token.refresh_token);
                return token;
            }
            catch
            {
                NeedsAuthentication = true;
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Error))
                    Log.Error("An error occured while fetching the oauth token during the user authorization flow");
                return null;
            }


        }
        private async Task<NinjaOauthToken> GetNewToken(string authcode = null, bool refresh = false)
        {
            string granttype = refresh ? "refresh_token" : "authorization_code";
            string reqtype = refresh ? "refresh_token" : "code";
            string code;

            if (refresh)
                code = refreshtoken;
            else
                code = authcode;


            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(NinjaOptions.EndpointUrl, NinjaApiEndpoint.oauthToken)),
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", granttype },
                    { reqtype , code },
                    { "client_id", NinjaOptions.Appid },
                    { "client_secret", NinjaOptions.AppSecret },
                    { "scope", "monitoring management offline_access" },
                    { "redirect_uri", NinjaOptions.RedirectUrl }
                }),
            };

            using (var response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string body = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<NinjaOauthToken>(body);
                }
                catch (HttpRequestException)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    var error = JToken.Parse(body);
                    throw new Exception(body);

                }

            }
        }
        public async Task<T> NinjaFetchAsync<T>(string item)
        {
            if (!await EnsureTokenExistsAndIsValid())
            {
                throw new ArgumentNullException(nameof(_token.access_token));
            }

            string endpointurl = String.Concat(NinjaOptions.EndpointUrl, item);
            if (Log.IsEnabled(LogEventLevel.Debug))
                Log.Debug("Fetching data from {url}", endpointurl);

            try
            {
                var result = await CallNinjaApiAsync(endpointurl);
                Log.Information("Fetching data from {url}", endpointurl);
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (ArgumentNullException)
            {
                Log.Error("Can't request ninja data, no Oauth token available.");
                throw;
            }
        }
        public async Task<T> NinjaFetchAsync<T>(string item, int id)
        {
            if (!await EnsureTokenExistsAndIsValid())
            {
                throw new ArgumentNullException(nameof(_token.access_token));
            }


            string endpointurl = String.Concat(NinjaOptions.EndpointUrl, item.Replace("id", id.ToString()));
            if (Log.IsEnabled(LogEventLevel.Debug))
                Log.Debug("Fetching data from {url}", endpointurl);

            try
            {
                var result = await CallNinjaApiAsync(endpointurl);
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (ArgumentNullException)
            {
                Log.Error("Can't request ninja data, no Oauth token available.");
                throw;
            }

        }
        private async Task<string> CallNinjaApiAsync(string endpoint)
        {

            if (!await EnsureTokenExistsAndIsValid())
            {
                throw new ArgumentNullException(nameof(_token.access_token));
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(endpoint)

            };

            using (var response = await client.SendAsync(request))
            {
                try
                {

                    response.EnsureSuccessStatusCode();
                    string body = await response.Content.ReadAsStringAsync();
                    return body;
                }
                catch (HttpRequestException ex)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    var error = JToken.Parse(body);


                    if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                        throw new HttpRequestException(error.ToString(), ex.InnerException, System.Net.HttpStatusCode.NotFound);


                    throw;
                }
            }
        }
        public async Task<string> ApproveDevice(NinjaDeviceApproval devices)
        {
            if (!await EnsureTokenExistsAndIsValid())
                throw new ArgumentNullException(nameof(_token.access_token));

            var devicelist = JsonConvert.SerializeObject(devices);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.access_token);


            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(NinjaOptions.EndpointUrl, NinjaApiEndpoint.deviceApprovalUrl)),
                Content = new StringContent(devicelist, Encoding.UTF8, "application/json"),


            };

            using (var response = await client.SendAsync(request))
            {
                try
                {

                    response.EnsureSuccessStatusCode();

                    string body = await response.Content.ReadAsStringAsync();
                    return body;
                }
                catch
                {
                    throw new Exception($"{{\"Error\": \"{response.Content.ReadAsStringAsync()}\"}}");
                }
            }
        }
        /// <summary>
        /// Fetches all organizations and inserts them into the sqlite database
        /// </summary>
        /// <returns></returns>
         /*   Log.Information("Refreshing local organization cache");
        private async Task RefreshOrganizationCache()
        {
            string endpointurl = String.Concat(NinjaOptions.EndpointUrl, NinjaApiEndpoint.getOrganizations);
            var res = await CallApi(endpointurl) as string;
            var orgs = JsonConvert.DeserializeObject<NinjaOrganization[]>(res);

            foreach (NinjaOrganization org in orgs)
            {

                var o = new OrganizationModel
                {
                    Name = org.name,
                    Id = org.id
                };
                engine.Insert(o);
            }

        }*/
    }




}





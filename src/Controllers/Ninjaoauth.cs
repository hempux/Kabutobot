using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using net.hempux.kabuto.Ninja;
using net.hempux.kabuto.Teamsoptions;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace net.hempux.Controllers
{
    [Route("api/ninjaoauth")]
    [ApiController]
    public class Ninjaoauth : ControllerBase
    {


        private static NinjaApiv2 ninjaApi { get; set; }
        public Ninjaoauth(IConfiguration configuration)
        {
            if(ninjaApi == null)
            ninjaApi = new NinjaApiv2();
        }

        [Route("manualtoken")]
        [HttpGet]
        public async Task<IActionResult> ManualToken()
        {
            if (!HttpContext.Request.Query.ContainsKey("code"))
                return new ContentResult()
                {
                    Content = "{ \"Error\":\"Token is missing or malformed\"}",
                    ContentType = "Application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
           
                   var token = await ninjaApi.GetOauthToken(HttpContext.Request.Query["code"]);
                    return new ContentResult()
                    {
                        Content = "{ \"Token\":" + JsonConvert.SerializeObject(token) + "}",
                        ContentType = "Application/json",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                
            
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {

            if (HttpContext.Request.Query.ContainsKey("error"))
            {
                string error = HttpContext.Request.Query["errormessage"];
                return new ContentResult()
                {
                    Content = "{ \"Error\":" + JsonConvert.SerializeObject(error) + "}",
                    ContentType = "Application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            if (!HttpContext.Request.Query.ContainsKey("code"))
                return new ContentResult()
                {
                    Content = "{ \"Error\":\"Code is missing or malformed\"}",
                    ContentType = "Application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };

            string code = HttpContext.Request.Query["code"];

            await ninjaApi.CompleteOauthFlow(code);

            if (!string.IsNullOrWhiteSpace(NinjaApiv2.GetmessageId()))
                await Deletechannelmessage(NinjaApiv2.GetmessageId());
            else
                Log.Warning("The ID for the authorizaion message has gone missing.\n" +
                    "The message will not be deleted once the authentication completes.");



           return new RedirectResult("/api/oauthresult", permanent: false, preserveMethod: true);
        }


        private async Task Deletechannelmessage(string messageId)
        {
            try
            {
                var credentials = new MicrosoftAppCredentials(TeamsbotOptions.Appid, TeamsbotOptions.AppSecret);
                var connector = new ConnectorClient(new System.Uri(TeamsbotOptions.TeamsServiceUrl), credentials);
                await connector.Conversations.DeleteActivityAsync(TeamsbotOptions.TeamsChannel, messageId);
                Log.Information("Deleted interactive auth card (Id {id})", messageId);
            }
            catch
            {
                Log.Error("An error occured while deleting message {message} ", messageId);
            }

        }
    }



}

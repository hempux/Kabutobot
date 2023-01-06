using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using net.hempux.kabuto.database;
using net.hempux.kabuto.Ninja;
using net.hempux.kabuto.Teamsoptions;
using net.hempux.ninjawebhook.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace net.hempux.kabuto.Controllers
{
    [Route("api/test")]
    [ApiController]
    [Consumes("application/json")]
    public class TestController : ControllerBase
    {
        readonly INinjaApiv2 apiv2;

        readonly SqliteEngine engine;
        public TestController(INinjaApiv2 ninjaApiv2)
        {
            apiv2 = ninjaApiv2 ?? throw new ArgumentNullException(nameof(ninjaApiv2));
            engine = new SqliteEngine();
        }

        public async Task<IActionResult> Get()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                return new ContentResult()
                {
                    Content = "{\"msg\":" + JsonConvert.ToString("Forbidden outside development") + "}",
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.Forbidden,
                };
            }

            

            return new RedirectResult("/api/oauthresult");

        }
        /*  [Route("api/ninjarenew")]
          public async Task<IActionResult> Reewntoken()
          {
              Log.Information("Renewal");

              await apiv2.GetOauthToken("");

              return NoContent();
          }
          */
        [Route("testar")]
        [HttpGet]
        public virtual async Task<IActionResult> GetTest()
        {
            
            if (HttpContext.Request.Query.ContainsKey("id"))
            {
              


            try
            {

                    DeviceModel device = await engine.GetDeviceById(int.Parse( HttpContext.Request.Query["id"]));
                    
                    var content = JsonConvert.SerializeObject(device,Formatting.Indented, new JsonSerializerSettings
                    {
                        MaxDepth = 2
                    });
                    return new ContentResult()
                    {
                        Content = content,
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.Created,
                    };
                }
            catch (Exception ex)
            {
                Log.Error("Error during request: {msg}", ex.Message);
                    return new ContentResult()
                    {
                        Content = ex.Message,
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                    };
                }
            
            
            }

            return new ContentResult()
            {
                Content = String.Empty,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        [Route("testar3")]
        [HttpGet]
        public virtual async Task<IActionResult> GetTest3()
        {

            if (HttpContext.Request.Query.ContainsKey("id"))
            {



                try
                {
                    var org = engine.GetOrganizationById(int.Parse(HttpContext.Request.Query["id"]));
                    var content = JsonConvert.SerializeObject(org.Result, Formatting.Indented, new JsonSerializerSettings
                    {
                        MaxDepth = 1,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore

                    });

                    return new ContentResult()
                    {
                        Content = content,
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.Created,
                    };
                }
                catch (Exception ex)
                {
                    Log.Error("Error during incoming ninja webhook: {msg}", ex.Message);
                    return new ContentResult()
                    {
                        Content = ex.Message,
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                    };
                }


            }

            return new ContentResult()
            {
                Content = String.Empty,
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        [Route("testar2")]
        [HttpGet]
        public virtual async Task<IActionResult> GetTest2()
        {
            Bot.NinjaMessageBot.TokenInit();


            return new ContentResult()
            {
                Content = "GG",
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.Created,
            };



        }


        [HttpPost]
        public virtual async Task<IActionResult> OnActivity([FromBody] DetailedActivity detailedActivity)
        {

            var device = engine.GetDeviceById(detailedActivity.DeviceId);
            

            try
            {
                await Task.WhenAll(device);
            }
            catch (Exception ex)
            {
                Log.Error("Error during incoming ninja webhook: {msg}", ex.Message);
            }

            var card = Cardmanager.NinjaNotificationCard(device.Result.Organization.Name, device.Result.SystemName, detailedActivity.Message);
            await CreateChannelConversation(card);

            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(detailedActivity),
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.Created,
            };
        }
        private async Task CreateChannelConversation(Attachment card)
        {


            var credentials = new MicrosoftAppCredentials(TeamsbotOptions.Appid, TeamsbotOptions.AppSecret);
            var connector = new ConnectorClient(new Uri(TeamsbotOptions.TeamsServiceUrl), credentials);
            var channelData = new Dictionary<string, string>();
            channelData["teamsChannelId"] = TeamsbotOptions.TeamsChannel;
            IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.Attachments.Add(card);

            ConversationParameters conversationParams = new ConversationParameters(
                isGroup: true,
                bot: null,
                members: null,
                topicName: "New NinjaRMM Issue",
                activity: (Activity)newMessage,
                channelData: channelData
                );

            var result = await connector.Conversations.CreateConversationAsync(conversationParams);
            Console.WriteLine(result);
        }
    }

}



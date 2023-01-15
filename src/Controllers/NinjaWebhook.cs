// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using net.hempux.kabuto;
using net.hempux.kabuto.database;
using net.hempux.kabuto.Ninja;
using net.hempux.ninjawebhook.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static net.hempux.ninjawebhook.Models.DetailedActivity;

namespace net.hempux.Controllers
{
    [Route("api/ninjawebhook")]
    [ApiController]
    [Consumes("application/json")]
    public partial class NinjaApiController : ControllerBase
    {
        private readonly INinjaApiv2 ninjaApi;
        private readonly SqliteEngine engine;
        private readonly string savetarget;
        private static Dictionary<Guid, ConversationResourceResponse> actionFeed;
        private DeviceModel device;


        public NinjaApiController(IConfiguration configuration, INinjaApiv2 ninjaApiV2)
        {
            if (actionFeed == null)
                actionFeed = new Dictionary<Guid, ConversationResourceResponse>();

            ninjaApi = ninjaApiV2;
            engine = new SqliteEngine();
            savetarget = string.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "activites", Path.DirectorySeparatorChar);
        }

        [HttpPost]
        public async Task<IActionResult> OnActivity([FromBody] DetailedActivity detailedActivity)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                if (!Directory.Exists(savetarget))
                    try
                    {
                    Directory.CreateDirectory(savetarget);
                    }catch(Exception ex)
                    {
                        Log.Error("Could not create folder for storing ninjarequests.");
                    }

                string filepath = string.Concat(savetarget, detailedActivity.Id, ".json");
                Log.Information("Saving ninjareqest {id} to file {filename}", detailedActivity.Id, filepath);
                var json = JsonConvert.SerializeObject(detailedActivity, Formatting.Indented);
                System.IO.File.WriteAllText(filepath, json);
            }
            #region guards


            if (!await ninjaApi.EnsureTokenExistsAndIsValid())
                return new ContentResult()
                {
                    Content = $"{{\"Error\":\"No valid NinjaApi Token available\"}}",
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.NoContent,
                    // Send 204 instead of 404 so that NinjaOne does not disable the webhook/notification channel.
                };
            #endregion

            try
            {
                this.device = await engine.GetDeviceById(detailedActivity.DeviceId);
            }
            catch (HttpRequestException ex)
            {
                Log.Error("Could not find device with Node Id {id}", detailedActivity.DeviceId);
                return new ContentResult()
                {
                    Content = ex.Message,
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.NoContent
                    // Send 204 instead of 404 so that NinjaOne does not disable the webhook/notification channel.
                };
            }

            switch (detailedActivity.ActivityType)
            {
                case ActivityTypeEnum.ACTIONSETEnum:
                    await HandleActionSetActivity(detailedActivity, device);
                    return new ContentResult()
                    {
                        Content = JsonConvert.SerializeObject(detailedActivity),
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
               /* case ActivityTypeEnum.CONDITIONEnum:
                    await HandleConditionActivity(detailedActivity);
                    return new ContentResult()
                    {
                        Content = JsonConvert.SerializeObject(detailedActivity),
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                    
                case ActivityTypeEnum.ACTIONEnum:
                    break;
                case ActivityTypeEnum.CONDITIONACTIONSETEnum:
                    break;
                case ActivityTypeEnum.CONDITIONACTIONEnum:
                    break;
                case ActivityTypeEnum.PATCHMANAGEMENTEnum:
                    break;                   
                     */
                case ActivityTypeEnum.ANTIVIRUSEnum:
                    await HandleAntivirusActivity(detailedActivity, device);
                    return new ContentResult()
                    {
                        Content = JsonConvert.SerializeObject(detailedActivity),
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.OK,
                    };


                case ActivityTypeEnum.SYSTEMEnum:
                    await HandleSystemActivity(detailedActivity, device);
                    return new ContentResult()
                    {
                        Content = JsonConvert.SerializeObject(detailedActivity),
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.OK,
                    };

                default:
                    await FallbackActivityHandler(detailedActivity, device);
                    return new ContentResult()
                    {
                        Content = JsonConvert.SerializeObject(detailedActivity),
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
            }

        }
        private async Task HandleSystemActivity(DetailedActivity detailedActivity, DeviceModel device)
        {
            if (detailedActivity.StatusCode == StatusCodeEnum.NODECREATEDEnum)
            {



                switch (device.Organization.ApprovalMode)
                {
                    case Organization.NodeApprovalModeEnum.AUTOMATICEnum:
                        await SendApprovedAutomaticallyCardAsync(detailedActivity, device);
                        break;
                    case Organization.NodeApprovalModeEnum.MANUALEnum:
                        await SendApprovalCardAsync(device);
                        break;
                    case Organization.NodeApprovalModeEnum.REJECTEnum:
                        await SendRejectedAutomaticallyCardAsync(detailedActivity, device);
                        break;
                    default:
                        break;
                }
            }

        }
        private async Task HandleActionSetActivity(DetailedActivity detailedActivity, DeviceModel device)
        {

            if (detailedActivity.StatusCode == StatusCodeEnum.STARTEDEnum)
            {

                var activitydata = (JObject)JsonConvert.DeserializeObject(detailedActivity.Data.Values.FirstOrDefault().ToString());
                Attachment newdeviceCard = Cardmanager.NinjaNotificationCard(device.Organization.Name, device.SystemName, detailedActivity, "A ninja task was started.");
                var teamsMessage = await SendMessageWithMessageIdResultAsync("A ninja task was started.", newdeviceCard);

                actionFeed.TryAdd(detailedActivity.SeriesUid, teamsMessage);

            }
            else if (detailedActivity.StatusCode == StatusCodeEnum.COMPLETEDEnum)
            {

                if (actionFeed.TryGetValue(detailedActivity.SeriesUid, out var teamsmessage))
                {

                    var activitydata = (JObject)JsonConvert.DeserializeObject(detailedActivity.Data.Values.FirstOrDefault().ToString());
                    Attachment newdeviceCard = Cardmanager.NinjaNotificationCard(device.Organization.Name, device.SystemName, detailedActivity, "A ninja task has finished.");
                    await SendMessageReplyAsync(teamsmessage, newdeviceCard);

                }

            }

        }
        private async Task HandleAntivirusActivity(DetailedActivity detailedActivity, DeviceModel device)
        {
            switch (detailedActivity.StatusCode)
            {
                case StatusCodeEnum.BDASBITDEFENDERTHREATDELETEDEnum or
                     StatusCodeEnum.BDASBITDEFENDERTHREATBLOCKEDEnum or   
                     StatusCodeEnum.BDASBITDEFENDERTHREATCLEANEDEnum or            
                     StatusCodeEnum.BDASBITDEFENDERTHREATIGNOREDEnum or  
                     StatusCodeEnum.BDASBITDEFENDERTHREATPRESENTEnum or
                     StatusCodeEnum.BDASBITDEFENDERTHREATQUARANTINEDEnum:
                    await SendThreatDetailsCardAsync(device, detailedActivity);
                    break;
                default:
                    break;
            }



        }
        private async Task FallbackActivityHandler(DetailedActivity detailedActivity,DeviceModel device)
        {
            Attachment newdeviceCard = Cardmanager.NinjaNotificationCard(device.Organization.Name, device.SystemName, detailedActivity, "A NinjaRmm notifiation has been triggered");
            await SendMessageAsync("A NinjaRmm notifiation has been triggered.", newdeviceCard);
        }
        private async Task HandleConditionActivity(DetailedActivity detailedActivity, DeviceModel device)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {

            return new ContentResult()
            {
                Content = "{\"error\":\"Method not allowed\"}",
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.MethodNotAllowed,
            };

        }

    }
}







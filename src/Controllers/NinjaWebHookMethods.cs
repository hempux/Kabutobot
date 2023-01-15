using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using net.hempux.kabuto;
using net.hempux.kabuto.database;
using net.hempux.kabuto.Teamsoptions;
using net.hempux.ninjawebhook.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net.hempux.Controllers
{
    public partial class NinjaApiController : ControllerBase
    {

        private async Task SendRejectedAutomaticallyCardAsync(DetailedActivity detailedActivity, DeviceModel device)
        {
            Attachment newdeviceCard = Cardmanager.NinjaNotificationCard(device.Organization.Name, device.SystemName, detailedActivity, "A new device has been automatically rejected by organization policy.");
            await SendMessageAsync("A device was rejected automatically.", newdeviceCard);

        }
        private async Task SendApprovedAutomaticallyCardAsync(DetailedActivity detailedActivity, DeviceModel device)
        {
            Attachment newdeviceCard = Cardmanager.NinjaNotificationCard(device.Organization.Name, device.SystemName, detailedActivity, "A new device has been automatically approved by organization policy.");
            await SendMessageAsync("A device was approved automatically.", newdeviceCard);

        }
        private async Task SendApprovalCardAsync(DeviceModel device)
        {

            try
            {
                Attachment newdeviceCard = Cardmanager.CreateDeviceApprovalCard(device.SystemName, device.Organization.Name);
                await SendMessageAsync("Device approval request", newdeviceCard);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        private async Task SendMessageReplyAsync(ConversationResourceResponse message, Attachment attachment)
        {
            var connector = new ConnectorClient(new Uri(TeamsbotOptions.TeamsServiceUrl), TeamsbotOptions.AppCredentials);
            var channelData = new Dictionary<string, string>();
            channelData["teamsChannelId"] = TeamsbotOptions.TeamsChannel;

            IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.ReplyToId = message.ActivityId;
            newMessage.Attachments.Add(attachment);
            newMessage.Id = message.Id;
            newMessage.ChannelData = channelData;
            newMessage.ReplyToId = message.ActivityId;
            newMessage.Conversation = new ConversationAccount();
            newMessage.Conversation.Id = message.Id;

            await connector.Conversations.ReplyToActivityAsync((Activity)newMessage);

        }
        private async Task SendMessageAsync(string topic, Attachment attachment)
        {

            var connector = new ConnectorClient(new Uri(TeamsbotOptions.TeamsServiceUrl), TeamsbotOptions.AppCredentials);
            var channelData = new Dictionary<string, string>();
            channelData["teamsChannelId"] = TeamsbotOptions.TeamsChannel;
            IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.Attachments.Add(
                attachment
                );

            ConversationParameters conversationParams = new ConversationParameters(
                isGroup: true,
                bot: null,
                members: null,
                topicName: topic,
                activity: (Activity)newMessage,
                channelData: channelData
                );

            var result = await connector.Conversations.CreateConversationAsync(conversationParams);

        }
        private async Task SendThreatDetailsCardAsync(DeviceModel device, DetailedActivity detailedActivity)
        {
            var activitydata = (JObject)JsonConvert.DeserializeObject(detailedActivity.Data.Values.FirstOrDefault().ToString());
            AntiviruseventDetails details = activitydata.SelectToken("params").ToObject<AntiviruseventDetails>();
            Attachment newdeviceCard = Cardmanager.NinjaAntivirusThreatCard(device.Organization.Name, device.SystemName, detailedActivity, details);
            await SendMessageAsync("A new bitdefender event.", newdeviceCard);
        }
        private async Task<ConversationResourceResponse> SendMessageWithMessageIdResultAsync(string topic, Attachment attachment)
        {

            var connector = new ConnectorClient(new Uri(TeamsbotOptions.TeamsServiceUrl), TeamsbotOptions.AppCredentials);
            var channelData = new Dictionary<string, string>();
            channelData["teamsChannelId"] = TeamsbotOptions.TeamsChannel;

            IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.Attachments.Add(attachment);

            ConversationParameters conversationParams = new ConversationParameters(
                isGroup: true,
                bot: null,
                members: null,
                topicName: topic,
                activity: (Activity)newMessage,
                channelData: channelData
                );

            var result = await connector.Conversations.CreateConversationAsync(conversationParams);
            return result;
        }

    }
}

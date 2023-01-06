// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.


using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using net.hempux.kabuto.Ninja;
using net.hempux.kabuto.Teamsoptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using net.hempux.kabuto.database;

namespace net.hempux.kabuto.Bot
{
    public sealed partial class NinjaMessageBot : TeamsActivityHandler
    {
        private readonly ConcurrentDictionary<string, ConversationReference> _conversationReferences;
        private readonly string _baseUrl;
        private static NinjaApiv2 ninjaApi;

        public NinjaMessageBot(IConfiguration config, ConcurrentDictionary<string, ConversationReference> conversationReferences)
        {
            
            _conversationReferences = conversationReferences;
            _baseUrl = TeamsbotOptions.Botendpoint;
            ninjaApi = new NinjaApiv2();

        }
        public static void Init(NinjaApiv2 ninjaApiv2)
        {
            if (ninjaApi == null)
                ninjaApi = ninjaApiv2;
            else
                return;
        }
        public static void TokenInit()
        {
            if (NinjaApiv2.Tokenstatus())
            {
                Log.Information("Reusing token from previous startup");
                return;
            }
            string messageId = NinjaApiv2.GetmessageId();

            if (string.IsNullOrEmpty(messageId) && NinjaApiv2.NeedsAuthentication)
            {
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
                    Log.Information("Sending interactive sign-in card to reauth Ninja");
                var result = Task.Run(async () => { await SendAuthCard(); });
            }
            else if (!string.IsNullOrEmpty(messageId) && NinjaApiv2.NeedsAuthentication)
            {
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
                    Log.Information("Waiting for interactive sign-in (MessageId {id})", messageId);
            }

        }
        private void AddConversationReference(Activity activity)
        {
            var conversationReference = activity.GetConversationReference();
            _conversationReferences.AddOrUpdate(conversationReference.User.Id, conversationReference, (key, newValue) => conversationReference);
        }
        /// <summary>
        /// Used to route mention actions performed by users to tasks/methods.
        /// 
        /// </summary>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            AddConversationReference(turnContext.Activity as Activity);

            turnContext.Activity.RemoveRecipientMention();
            if (!(turnContext.Activity.Value == null))
            {
                try
                {
                    JObject details = JObject.Parse(turnContext.Activity.Value.ToString());
                    Log.Debug("Got Card value.");
                }
                catch (JsonReaderException)
                {

                    Log.Error("Got Card value but could not Json-decode it.");

                }

            }


            if (!(turnContext.Activity.Text == null))
            {



                string[] commanddata = turnContext.Activity.Text.Split(" ");

                // Do nothing if we are mentioned and the mention does not start with "botaction" or we only get "botaction"                 
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower() != "development")
                    return;

                switch (commanddata[0].ToLower())
                {
                    case "test":
                        await TestFunction(turnContext, cancellationToken);
                        break;
                    case "channelinfo":
                        await SendChannelInfoAsync(turnContext, cancellationToken);
                        break;
                    case ".devices":
                        await GetPendingDevices(turnContext, cancellationToken);
                        break;
                    case "deviceapproval":
                        await HandleDeviceApproval(turnContext, cancellationToken);
                        break;
                    default:
                        break;

                }

            }

        }
        private static async Task SendAuthCard()
        {
            if (!NinjaApiv2.NeedsAuthentication)
                return;

            var connector = new ConnectorClient(new Uri(TeamsbotOptions.TeamsServiceUrl), TeamsbotOptions.AppCredentials);
            var channelData = new Dictionary<string, string>();
            channelData["teamsChannelId"] = TeamsbotOptions.TeamsChannel;
            IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.Attachments.Add(
                Cardmanager.AuthenticationCard(
                    ninjaApi.GetAuthorizationCodeUrl()
                    )
                );

            ConversationParameters conversationParams = new ConversationParameters(
                isGroup: true,
                bot: null,
                members: null,
                topicName: "Ninja API sign-in",
                activity: (Activity)newMessage,
                channelData: channelData
                );

            var result = await connector.Conversations.CreateConversationAsync(conversationParams);
            NinjaApiv2.SetMessageId(result.ActivityId);
        }
        private async Task HandleDeviceApproval(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            JObject details = JObject.Parse(turnContext.Activity.Value.ToString());
            var activity = new Activity();
            activity.Id = turnContext.Activity.ReplyToId;
            if (!(details["action"].ToString() == "approve"))
                return;

            try
            {
                activity.Text = $"{details["action"]}d device with Id {details["systemid"]}";
                int[] deviceid = new int[1];
                deviceid[0] = Convert.ToInt32(details["id"]);
                await ninjaApi.ApproveDevice(new NinjaDeviceApproval() { devices = deviceid });

            }
            catch
            {
                activity.Text = $"Something went wrong trying to {details["action"]} device with Id {details["systemid"]}" + Environment.NewLine
                       + $"Go to the NinjaOne Control panel and manually {details["action"]} the device";
            }
            finally
            {

                await turnContext.UpdateActivityAsync(activity, cancellationToken);
            }

            await Task.Delay(5000);

            var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

        }
        private async Task GetPendingDevices(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var pendingdevices = await ninjaApi.NinjaFetchAsync <DeviceModel[]>(NinjaApiEndpoint.pendingDevices);
            
            if (pendingdevices.Count() == 0)
            {
                _ = await turnContext.SendActivityAsync(MessageFactory.Text($"No pending devices found")); 
                return;

            }


            List<int> devs = new List<int>();

            foreach (var device in pendingdevices)
            {

                devs.Add(device.DeviceModelId);
            }
            NinjaDeviceApproval approvals = new NinjaDeviceApproval { devices = devs.ToArray() };

            var resp = await ninjaApi.ApproveDevice(approvals);

            _ = await turnContext.SendActivityAsync(MessageFactory.Text(JsonConvert.SerializeObject(approvals)));
        }
        private async Task ApproveDevice(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken, int deviceid)
        {
            Console.WriteLine("Approving device ");
            _ = await turnContext.SendActivityAsync(MessageFactory.Text($"Approving device with Id {deviceid}"));
        }
        private async Task DeleteCardActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            JObject details = JObject.Parse(turnContext.Activity.Value.ToString());
            if (!(details["action"].ToString() == "delete"))
                return;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                await turnContext.DeleteActivityAsync(turnContext.Activity.ReplyToId, cancellationToken);
        }
        private async Task SendChannelInfoAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            TeamDetails teamDetails = await TeamsInfo.GetTeamDetailsAsync(turnContext, turnContext.Activity.TeamsGetTeamInfo().Id, cancellationToken);

            if (teamDetails != null)
            {
                var channels = await TeamsInfo.GetTeamChannelsAsync(turnContext).ConfigureAwait(false);
                var channelscard = Cardmanager.channelscard(channels,turnContext.Activity.ServiceUrl);

                await turnContext.SendActivityAsync(MessageFactory.Attachment(channelscard));

            }
            else
            {
                await turnContext.SendActivityAsync($"You can't request information about channels outside a team.");
            }
        }

        protected override async Task OnReactionsAddedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var reaction in messageReactions)
            {

                if (reaction.Type.ToLower() == "274c_crossmark" && turnContext.Activity.ReplyToId != null)
                    try
                    {
                        await turnContext.DeleteActivityAsync(turnContext.Activity.ReplyToId, cancellationToken);
                        Log.Information("Deleted Message {id}", turnContext.Activity.ReplyToId);
                    }
                    catch
                    {
                        Log.Error("An error occured trying to remove message {id}", turnContext.Activity.ReplyToId);
                    }
            }

        }

    }
}
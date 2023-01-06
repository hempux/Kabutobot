using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using net.hempux.kabuto.Teamsoptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace net.hempux.kabuto.Bot
{
    public sealed partial class NinjaMessageBot : TeamsActivityHandler
    {
        


        /// <summary>
        /// Send a status update message to the chat (if the issue is resolved and the status goes into "RECOVERED")
        /// </summary>
        /// <returns></returns>
        private async Task SendStatusUpdate()
        {

            var msg = MessageFactory.Text($"No pending devices found");
           IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.Text = "Isse has been resolved.";

            await SendMessageAsync(msg);
        }
        private async Task SendMessageAsync(Activity activity)
        {

            var connector = new ConnectorClient(new Uri(TeamsbotOptions.TeamsServiceUrl), TeamsbotOptions.AppCredentials);
            var channelData = new Dictionary<string, string>();
            channelData["teamsChannelId"] = TeamsbotOptions.TeamsChannel;
            
            
            ConversationParameters conversationParams = new ConversationParameters(
                isGroup: true,
                bot: null,
                members: null,
                topicName: "New NinjaRMM Issue",
                activity: activity,
                channelData: channelData
                );

            var result = await connector.Conversations.CreateConversationAsync(conversationParams);
        }

        private async Task SendMessageAsync(Attachment attachment)
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
                topicName: "New NinjaRMM Issue",
                activity: (Activity)newMessage,
                channelData: channelData
                );

            var result = await connector.Conversations.CreateConversationAsync(conversationParams);
        }

        private async Task TestFunction(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);

            var card1 = Cardmanager.NinjaNotificationCard("Devicename", "action", "name");

            var msg = MessageFactory.Attachment(card1);
            _ = await turnContext.SendActivityAsync(msg);

            /*
                        await Task.Delay(5000);


                        var reply = MessageFactory.Attachment(Cardmanager.DismissCard());
                        reply.Id = res.Id;
                        await turnContext.UpdateActivityAsync(reply, cancellationToken);
                        */

        }
        // For AdaptiveExecuteAction() 
        protected override async Task<InvokeResponse> OnInvokeActivityAsync(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Name == null && turnContext.Activity.ChannelId == Channels.Msteams || !(turnContext.Activity.Name == "adaptiveCard/action"))        
                return await OnTeamsCardActionInvokeAsync(turnContext, cancellationToken).ConfigureAwait(false);
                

            try {
                var details = JObject.Parse(turnContext.Activity.Value.ToString());
                if (!string.IsNullOrEmpty((string)details.SelectToken("action.verb")))
                {
                    switch ((string)details.SelectToken("action.verb").ToString().ToLower())
                    {
                        case "deletecard":
                            await turnContext.DeleteActivityAsync(turnContext.Activity.ReplyToId);
                            break;
                        default:
                            break;
                    }
                }

                return await base.OnInvokeActivityAsync(turnContext, cancellationToken);
            }    
                    
                
            
            catch (InvokeResponseException e)
            {
                return e.CreateInvokeResponse();
            }
        }

        #region Other unused functions
        /*

        private static async Task SendUpdatedCard(ITurnContext<IMessageActivity> turnContext, HeroCard card, CancellationToken cancellationToken)
        {
            card.Title = "I've been updated";

            var data = turnContext.Activity.Value as JObject;
            data = JObject.FromObject(data);
            data["count"] = data["count"].Value<int>() + 1;
            card.Text = $"Update count - {data["count"].Value<int>()}";

            card.Buttons.Add(new CardAction
            {
                Type = ActionTypes.MessageBack,
                Title = "Update Card",
                Text = "UpdateCardAction",
                Value = data
            });

            var activity = MessageFactory.Attachment(card.ToAttachment());
            activity.Id = turnContext.Activity.ReplyToId;

            await turnContext.UpdateActivityAsync(activity, cancellationToken);
        }

private async Task MessageAllMembersAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
{
    var teamsChannelId = turnContext.Activity.TeamsGetChannelId();
    var serviceUrl = turnContext.Activity.ServiceUrl;
    var credentials = new MicrosoftAppCredentials(_appId, _appPassword);
    ConversationReference conversationReference = null;

    var members = await GetPagedMembers(turnContext, cancellationToken);

    foreach (var teamMember in members)
    {
        var proactiveMessage = MessageFactory.Text($"Hello {teamMember.GivenName} {teamMember.Surname}. I'm a Teams conversation bot.");

        var conversationParameters = new ConversationParameters
        {
            IsGroup = false,
            Bot = turnContext.Activity.Recipient,
            Members = new ChannelAccount[] { teamMember },
            TenantId = turnContext.Activity.Conversation.TenantId,
        };

        await ((BotFrameworkAdapter)turnContext.Adapter).CreateConversationAsync(
            teamsChannelId,
            serviceUrl,
            credentials,
            conversationParameters,
            async (t1, c1) =>
            {
                conversationReference = t1.Activity.GetConversationReference();
                await ((BotFrameworkAdapter)turnContext.Adapter).ContinueConversationAsync(
                    _appId,
                    conversationReference,
                    async (t2, c2) =>
                    {
                        await t2.SendActivityAsync(proactiveMessage, c2);
                    },
                    cancellationToken);
            },
            cancellationToken);
    }

    await turnContext.SendActivityAsync(MessageFactory.Text("All messages have been sent."), cancellationToken);
}
*/

        //-----Subscribe to Conversation Events in Bot integration

        // Channel events
        /*
        protected override async Task OnTeamsChannelCreatedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var info = channelInfo;
            var heroCard = new HeroCard(text: $"{channelInfo.Name} is the Channel created");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }

        protected override async Task OnTeamsChannelRenamedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard(text: $"{channelInfo.Name} is the new Channel name");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }

        protected override async Task OnTeamsChannelDeletedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard(text: $"{channelInfo.Name} is the Channel deleted");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }
       
        protected override async Task OnTeamsMembersRemovedAsync(IList<TeamsChannelAccount> membersRemoved, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (TeamsChannelAccount member in membersRemoved)
            {
                if (member.Id == turnContext.Activity.Recipient.Id)
                {
                    // The bot was removed
                    // You should clear any cached data you have for this team
                }
                else
                {
                    var heroCard = new HeroCard(text: $"{member.Name} was removed from {teamInfo.Name}");
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                }
            }
        }

        protected override async Task OnTeamsTeamRenamedAsync(TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard(text: $"{teamInfo.Name} is the new Team name");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }
        protected override async Task OnReactionsAddedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var reaction in messageReactions)
            {
                var newReaction = $"You reacted with '{reaction.Type}' to the following message: '{turnContext.Activity.ReplyToId}'";
                var replyActivity = MessageFactory.Text(newReaction);
                await turnContext.SendActivityAsync(replyActivity, cancellationToken);
            }
           
        }

        protected override async Task OnReactionsRemovedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var reaction in messageReactions)
            {
                var newReaction = $"You removed the reaction '{reaction.Type}' from the following message: '{turnContext.Activity.ReplyToId}'";
                var replyActivity = MessageFactory.Text(newReaction);
                await turnContext.SendActivityAsync(replyActivity, cancellationToken);
            }
            
        }

         */
        #endregion








    }

}

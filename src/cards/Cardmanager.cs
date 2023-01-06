using AdaptiveCards;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using net.hempux.kabuto.AdaptiveCardShorteners;
using net.hempux.ninjawebhook.Models;
using net.hempux.kabuto.Ninja;

namespace net.hempux.kabuto
{

    public static class Cardmanager
    {

        internal static Attachment NinjaNotificationCard(string orgName, string deviceName, string message, string cardHeader = null)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 4));

            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = cardHeader ?? $"A NinjaRmm notifiation has been triggered",
                Size = AdaptiveTextSize.Default
            });

            card.Body.Add(new AdaptiveFactSet()
            {
                Facts = new List<AdaptiveFact>()
                                            {
                    new AdaptiveFact()
                    {
                        Title="Organization:",
                        Value=orgName
                    },
                    new AdaptiveFact()
                    {
                        Title ="Device:",
                        Value=deviceName
                    },
                    new AdaptiveFact()
                    {
                        Title ="Message:",
                        Value=message
                    }
                }
            });

            card.Body.Add(new AdaptiveActionSet()
            {
                Actions = { AdaptiveCardButtons.DeleteButton() }
            });

            // serialize the card to JSON
            string json = card.ToJson();
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(json),
            };

            return adaptiveCardAttachment;
        }
        internal static Attachment CreateDeviceApprovalCard(string deviceName, string organization)
        {
            var basepath = Directory.GetCurrentDirectory();
            var cardjson = Path.Combine(basepath, "cards", "deviceapproval.json");
            var adaptiveCardJson = File.ReadAllText(cardjson);

            var devicecard = adaptiveCardJson
                .Replace("##ORGPH##", organization)
                .Replace("DEVICENAMEPH", deviceName);

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(devicecard),
            };
            return adaptiveCardAttachment;
        }
        internal static Attachment channelscard(IList<ChannelInfo> channels, string serviceUrl)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 4));

            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = $"Use one of these 'TeamsChannel' options for application configuration to pick where the bot sends notifications.",
                Size = AdaptiveTextSize.Default
            });

            var channelList = new List<AdaptiveFact>();

            AdaptiveFact microsoftServiceUrl = new AdaptiveFact()
            {
                Title = "Service URL",
                Value = serviceUrl
            };
            channelList.Add(microsoftServiceUrl);

            foreach (ChannelInfo channel in channels)
            {
                AdaptiveFact fact = new AdaptiveFact()
                {
                    Title = channel.Name ?? "Default channel",
                    Value = channel.Id
                };
                channelList.Add(fact);
            }

            card.Body.Add(new AdaptiveFactSet()
            {
                Facts = channelList

            });
            card.Body.Add(new AdaptiveActionSet()
            {
                Actions = { AdaptiveCardButtons.DeleteButtonExecute() }
            });

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(card.ToJson()),
            };

            return adaptiveCardAttachment;
        }
        internal static Attachment AuthenticationCard(string authlink)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 4));

            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = $"The bot needs authorization with a system-administrator level account to make calls against the NinjaOne API.",
                Size = AdaptiveTextSize.Default,
                Wrap = true

            });

            card.Body.Add(new AdaptiveActionSet()
            {
                Actions = { AdaptiveCardButtons.AuthorizationButton(authlink) }
            });

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(card.ToJson()),
            };

            return adaptiveCardAttachment;
        }

        internal static Attachment NinjaAntivirusThreatCard(string organization, string systemName,DetailedActivity activity, AntiviruseventDetails antiviruseventDetails)
        {
            AdaptiveCard card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 4));

            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = $"A Bitdefender event has been trigered",
                Size = AdaptiveTextSize.Default
            });

            card.Body.Add(new AdaptiveFactSet()
            {
                Facts = new List<AdaptiveFact>()
                                            {
                    new AdaptiveFact()
                    {
                        Title="Organization:",
                        Value=organization
                    },
                    new AdaptiveFact()
                    {
                        Title ="Device:",
                        Value=systemName
                    },
                    new AdaptiveFact()
                    {
                        Title ="Action taken:",
                        Value= activity.Status
                    },
                    new AdaptiveFact()
                    {
                        Title ="",
                        Value=""
                    },
                    new AdaptiveFact()
                    {
                        Title ="Threat type:",
                        Value=antiviruseventDetails.Threat_type
                    },
                    new AdaptiveFact()
                    {
                        Title ="Threat name:",
                        Value=antiviruseventDetails.Threat_name
                    },
                    new AdaptiveFact()
                    {
                        Title ="Threat path:",
                        Value=antiviruseventDetails.Threat_path
                    }

                }
            });

            card.Body.Add(new AdaptiveTextBlock()
            {
                Text = $"[ninjaRmm device link]({string.Concat(NinjaOptions.NinjaInstanceUrl, "/#/deviceDashboard/", activity.DeviceId,"/overview")})",
                Size = AdaptiveTextSize.Medium
            });
            card.Body.Add(new AdaptiveActionSet()
            {
                Actions = { AdaptiveCardButtons.DeleteButton() }
            });

            // serialize the card to JSON
            string json = card.ToJson();
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(json),
            };

            return adaptiveCardAttachment;
        }
    }
}
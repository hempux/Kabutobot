using AdaptiveCards;
using Newtonsoft.Json.Linq;

namespace net.hempux.kabuto.AdaptiveCardShorteners
{
    public class AdaptiveCardButtons
    {
        public static AdaptiveSubmitAction DeleteButton()
        {

            dynamic dataObject = new JObject();
            dataObject.msteams = new JObject();
            dataObject.msteams.type = "messageBack";
            dataObject.msteams.text = "delete";
            dataObject.msteams.value = "{\"action\": \"delete\" }";
            var actionSubmit = new AdaptiveSubmitAction()
            {

                Title = "Delete",
                Data = dataObject,
            };

            return actionSubmit;
        }

        public static AdaptiveExecuteAction DeleteButtonExecute()
        {
            var button = new AdaptiveExecuteAction()
            {
                Title = "Delete",
                Verb = "deletecard"
            };

            return button;
        }
        public static AdaptiveOpenUrlAction AuthorizationButton(string authUrl)
        {
            var auth = new AdaptiveOpenUrlAction
            {
                Title = "Authorize Now",
                Url = new System.Uri(authUrl)

            };
            return auth;
        }

    }
}

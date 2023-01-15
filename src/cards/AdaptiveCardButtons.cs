using AdaptiveCards;

namespace net.hempux.kabuto.AdaptiveCardShorteners
{
    public class AdaptiveCardButtons
    {

        public static AdaptiveExecuteAction DeleteButtonExecute(string buttonText = null)
        {
            var button = new AdaptiveExecuteAction()
            {
                Title = buttonText ?? "Delete",
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

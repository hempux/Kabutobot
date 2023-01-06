using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using net.hempux.kabuto.Ninja;

namespace net.hempux.kabuto.Pages
{
    public class OauthStatusModel : PageModel
    {
        public string OauthStatus { get; set; }
        public void OnGet()
        {
            if (NinjaApiv2.Tokenstatus())
                OauthStatus = "The app has authenticated successfully." +
                    "and you can now close this window/tab.";
            else
                OauthStatus = "There was an issue with the authorization. " +
                    "Please retry or restart the application/bot";
                


        }
    }
}

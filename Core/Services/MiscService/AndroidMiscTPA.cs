using UnityEngine;
using UnityEngine.Networking;

namespace HoakleEngine.Core.Services.MiscService
{
    public class AndroidMiscTPA : MiscThirdPartyActor
    {
        public override void Init()
        {
            
        }

        public override void OpenEmail()
        {
            string email = "hoakle.twitch@gmail.com";
            string subject = MyEscapeURL("User Feedback");
            Application.OpenURL("mailto:" + email + "?subject=" + subject);
        }

        private string MyEscapeURL (string url)
        {
            return UnityWebRequest.EscapeURL(url).Replace("+","%20");
        }
        
    }
}

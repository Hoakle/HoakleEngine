using System;
using UnityEngine;
using UnityEngine.Networking;

namespace HoakleEngine.Core.Services.MiscService
{
    public class AndroidMiscTPA : MiscThirdPartyActor
    {
        public Action<ActorError> OnError { get; set; }

        public void Init()
        {
            
        }

        public void OpenEmail()
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

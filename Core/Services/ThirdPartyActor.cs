using System;
using UnityEngine;

namespace HoakleEngine.Core.Services
{
    public interface ThirdPartyActor
    {
        public Action<ActorError> OnError { get; set; }
        public void Init();
    }

    public abstract class ActorError
    {
        public int Code;
        public string Message;
    }
}

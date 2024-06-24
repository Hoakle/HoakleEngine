using System.Collections.Generic;
using HoakleEngine.Core.Audio;

namespace HoakleEngine.Core.Game
{
    public interface IGameEngine : IEngine
    {
        
    }
    
    public abstract class GameEngine : Engine, IGameEngine
    {
        public override void Init()
        {
            
        }

        public override void Update(bool isPaused)
        {
            base.Update(isPaused);
        }
    }
}

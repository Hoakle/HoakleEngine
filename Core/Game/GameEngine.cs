using System.Collections.Generic;
using HoakleEngine.Core.Audio;

namespace HoakleEngine.Core.Game
{
    public abstract class GameEngine : Engine
    {
        protected GameDataHandler _GameDataHandler;
        public GameEngine(GameRoot gameRoot) : base(gameRoot)
        {
            
        }

        public override void Init()
        {
            _UpdateableList.Add(AudioPlayer.Instance);
        }

        public override void Update(bool isPaused)
        {
            base.Update(isPaused);
        }
    }
}

using System.Collections.Generic;

namespace HoakleEngine.Core.Game
{
    public abstract class DataHandler
    {
        protected GameEngine _GameEngine;

        public DataHandler(GameEngine parent)
        {
            _GameEngine = parent;
        }
    }
}

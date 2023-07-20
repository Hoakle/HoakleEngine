using HoakleEngine.Core.Communication;

namespace HoakleEngine.Core
{
    public abstract class Engine
    {
        protected GameRoot _GameRoot;
        public Engine(GameRoot gameRoot)
        {
            _GameRoot = gameRoot;
        }

        public abstract void Init();

        public abstract void Update();
    }
}

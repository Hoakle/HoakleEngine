using HoakleEngine.Core.Communication;

namespace HoakleEngine.Core
{
    public abstract class Engine
    {
        protected EventBus _EventBus;

        public Engine(EventBus eventBus)
        {
            _EventBus = eventBus;
        }

        public abstract void Init();

        public abstract void Update();
    }
}

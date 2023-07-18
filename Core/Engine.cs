using HoakleEngine.Core.Communication;

namespace HoakleEngine.Core
{
    public abstract class Engine
    {
        public Engine()
        {
            
        }

        public abstract void Init();

        public abstract void Update();
    }
}

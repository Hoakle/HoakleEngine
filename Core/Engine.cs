using System.Collections.Generic;

namespace HoakleEngine.Core
{
    public abstract class Engine
    {
        protected GameRoot _GameRoot;
        protected List<Engine> _LinkedEngines;
        public Engine(GameRoot gameRoot)
        {
            _GameRoot = gameRoot;
            _LinkedEngines = new List<Engine>();
        }

        public abstract void Init();

        public abstract void Update();

        public void LinkEngine(Engine engine)
        {
            _LinkedEngines.Add(engine);
        }

        public TEngine GetEngine<TEngine>() where TEngine : Engine
        {
            foreach (var engine in _LinkedEngines)
            {
                if (engine is TEngine tEngine)
                    return tEngine;
            }
            
            return null;
        }
    }
}

using System.Collections.Generic;
using HoakleEngine.Core.Config;
using HoakleEngine.Core.Game;
using HoakleEngine.Core.Services;

namespace HoakleEngine.Core
{
    public abstract class Engine
    {
        protected GameRoot _GameRoot;
        public ConfigContainer ConfigContainer => _GameRoot.ConfigContainer;
        public GameSaveContainer GameSave => _GameRoot.GameSaveContainer;
        public ServicesContainer ServicesContainer => _GameRoot.ServicesContainer;
        
        protected List<Engine> _LinkedEngines;
        protected List<IUpdateable> _UpdateableList;
        public Engine(GameRoot gameRoot)
        {
            _GameRoot = gameRoot;
            _LinkedEngines = new List<Engine>();
            _UpdateableList = new List<IUpdateable>();
        }

        public abstract void Init();

        public virtual void Update(bool isPaused)
        {
            foreach (var updateable in _UpdateableList)
            {
                updateable.Update(isPaused);
            }
        }

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

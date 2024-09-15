using System.Collections.Generic;
using HoakleEngine.Core.Config;
using HoakleEngine.Core.Game;
using HoakleEngine.Core.Services;
using UnityEngine;
using Zenject;

namespace HoakleEngine.Core
{
    public interface IEngine
    {
        public void Init();
        public void Update(bool isPaused);
        public void LinkEngine(IEngine engine);
    }
    
    public abstract class Engine : IEngine
    {
        protected List<IEngine> _LinkedEngines;
        protected List<IUpdateable> _UpdateableList;
        
        public Engine()
        {
            _LinkedEngines = new List<IEngine>();
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

        public void LinkEngine(IEngine engine)
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

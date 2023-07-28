using System.Collections.Generic;

namespace HoakleEngine.Core.Game
{
    public abstract class GameEngine : Engine
    {
        protected List<IUpdateable> _UpdateableList;
        public GameEngine(GameRoot gameRoot) : base(gameRoot)
        {
            _UpdateableList = new List<IUpdateable>();
        }

        public override void Init()
        {
            
        }

        public override void Update()
        {
            foreach (var updateable in _UpdateableList)
            {
                updateable.Update();
            }
        }
    }
}

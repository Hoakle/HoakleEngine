using System;
using HoakleEngine.Core.Services.GameSaveService;
using Zenject;

namespace HoakleEngine.Core.Game
{
    [Serializable]
    public abstract class GameSaveHandler<TData> : IInitializable where TData : struct
    {
        protected IGameSaveService _GameSaveService;
        protected string _Identifier;
        protected TData _Data;

        public GameSaveHandler(string identifier)
        {
            _Identifier = identifier;
        }

        [Inject]
        public void Inject(IGameSaveService gameSaveService)
        {
            _GameSaveService = gameSaveService;
        }
        
        public void Initialize()
        {
            BuildData();
        }
        
        protected virtual void BuildData()
        {
            if (_GameSaveService.Exist<TData>(_Identifier))
            {
                _Data = _GameSaveService.Get<TData>(_Identifier);
            }
            else
            {
                CreateData();
            }
        }

        protected abstract void CreateData();
        public virtual void Save()
        {
            _GameSaveService.Save(_Data, _Identifier);
        }
    }
}

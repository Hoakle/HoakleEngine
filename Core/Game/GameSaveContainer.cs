using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine.Core.Game
{
    public class GameSaveContainer
    {
        [SerializeField] private List<GameSave> _Saves = new List<GameSave>();

        private Dictionary<Type, GameSave> _SaveCache = new Dictionary<Type, GameSave>();

        public void SetSave<T>(T gameSave) where T : GameSave
        {
            _Saves.Add(gameSave);
        }
        public T GetSave<T>() where T : GameSave
        {
            Type type = typeof(T);
            if (!_SaveCache.ContainsKey(type))
            {
                _SaveCache[type] = _Saves.Find(so => so is T);
            }
            
            return (T) _SaveCache[type];
        }

        public void LoadSaves()
        {
            foreach (var save in _Saves)
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(save.SaveName), save);
            }
        }

        public void Save()
        {
            foreach (var save in _Saves)
            {
                PlayerPrefs.SetString(save.SaveName, JsonUtility.ToJson(save));
            }
        }

        public void Init()
        {
            foreach (var save in _Saves)
            {
                save.Init();
            }
        }
    }
}

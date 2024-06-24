using UnityEngine;

namespace HoakleEngine.Core.Services
{
    public interface IGameSaveService
    {
        public TData Get<TData>(string identifier) where TData : struct;
        public void Save<TData>(TData data, string identifier) where TData : struct;
        public bool Exist<TData>(string identifier) where TData : struct;
    }

    public class PlayerPrefService : IGameSaveService
    {
        public TData Get<TData>(string identifier) where TData : struct
        {
            return (TData) JsonUtility.FromJson(PlayerPrefs.GetString(identifier), typeof(TData));
        }

        public void Save<TData>(TData data, string identifier) where TData : struct
        {
            PlayerPrefs.SetString(identifier, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        public bool Exist<TData>(string identifier) where TData : struct
        {
            return PlayerPrefs.HasKey(identifier);
        }
    }
}

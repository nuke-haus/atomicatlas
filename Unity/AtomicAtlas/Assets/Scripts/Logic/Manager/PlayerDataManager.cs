using UnityEngine;

namespace Atlas.Logic
{
    public static class PlayerDataManager
    {
        public static T LoadFromData<T>(string key)
        {
            string data = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(data);
        }

        public static void SaveData(string key, object data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
        }
    } 
}

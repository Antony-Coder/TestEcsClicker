using UnityEngine;
using System;
using System.Collections.Generic;

namespace TestClickerEcs
{
    public class SaveLoadService
    {

        public void Save<T>(string key, T data) where T : class
        {
            if (data == null)
            {
                Debug.LogError($"Failed to save null object with key: {key}");
                return;
            }

            try
            {
                string jsonData = JsonUtility.ToJson(data, true);
                PlayerPrefs.SetString(key, jsonData);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data with key {key}: {e.Message}");
            }
        }

        public T Load<T>(string key, T defaultValue = default) where T : class
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return defaultValue;
            }

            try
            {
                string jsonData = PlayerPrefs.GetString(key);
                T loadedData = JsonUtility.FromJson<T>(jsonData);

                if (loadedData == null)
                {
                    Debug.LogWarning($"Failed to deserialize data with key: {key}");
                    return defaultValue;
                }

                return loadedData;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data with key {key}: {e.Message}");
                return defaultValue;
            }
        }


        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void DeleteKey(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}


using System.IO;
using Code.Data;
using UnityEngine;

namespace Code.Services.SaveSystem
{
    public class SaveService : ISaveService
    {
        private const string Directory = "/SaveData/";
        private const string FileName = "data.json";

        public void Save(SaveData data)
        {
            string directory = Application.persistentDataPath + Directory;

            if (System.IO.Directory.Exists(directory) == false)
                System.IO.Directory.CreateDirectory(directory);

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(directory + FileName, json);
        }

        public SaveData Load()
        {
            string fullPath = Application.persistentDataPath + Directory + FileName;

            SaveData data = new SaveData();

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                data = JsonUtility.FromJson<SaveData>(json);
            }

            return data;
        }
    }
}
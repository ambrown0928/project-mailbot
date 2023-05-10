using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving
{
    public class SaveLoad<T>
    {
        public virtual void SaveToJson(T item, string path)
        {
            string itemData = JsonConvert.SerializeObject(item);
            System.IO.FileInfo filePath = new System.IO.FileInfo(Application.persistentDataPath + path);
            Debug.Log(filePath);
            filePath.Directory.Create();
            System.IO.File.WriteAllText(filePath.FullName, itemData);
        }
        public virtual T LoadFromJson(string path)
        {
            string filePath = Application.persistentDataPath + path;
            string itemData = System.IO.File.ReadAllText(filePath);

            T item = JsonConvert.DeserializeObject<T>(itemData);
            return item;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);    
        }
    }

    public void SaveGameData(GameData gameData, int slotIndex)
    {
        string path = GetGameDataPath(slotIndex);
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, gameData);
        }
    }

    public GameData LoadGameData(int slotIndex)
    {
        string path = GetGameDataPath(slotIndex);
        if (File.Exists(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            using(FileStream stream = new FileStream(path, FileMode.Open))
            {
                return (GameData)serializer.Deserialize(stream);
            }
        }
        return null;
    }

    private string GetGameDataPath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, $"gameData_slot{slotIndex}.xml");
    }
}

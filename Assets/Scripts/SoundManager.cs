using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private string settingsFilePath;
    public SoundSettings currentSettings;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            settingsFilePath = Path.Combine(Application.persistentDataPath, "soundSettings.json");
            LoadSettings();
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveSettings(SoundSettings settings)
    {
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(settingsFilePath, json);
        currentSettings = settings;
        ApplySettings();
    }

    public void LoadSettings()
    {
        if(File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            currentSettings = JsonUtility.FromJson<SoundSettings>(json);
        }
        else
        {
            currentSettings = new SoundSettings();
        }
        ApplySettings();
    }

    public void ApplySettings()
    {
        AudioListener.volume = currentSettings.masterVolume;
    }
}

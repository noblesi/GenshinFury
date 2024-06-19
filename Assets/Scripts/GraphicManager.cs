using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GraphicManager : MonoBehaviour
{
    public static GraphicManager Instance;

    private string settingsFilePath;
    public GraphicSettings currentSettings;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            settingsFilePath = Path.Combine(Application.persistentDataPath, "graphicSettings.json");
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveSettings(GraphicSettings settings)
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
            currentSettings = JsonUtility.FromJson<GraphicSettings>(json);
        }
        else
        {
            currentSettings = new GraphicSettings();
        }
        ApplySettings();
    }

    public void ApplySettings()
    {
        Resolution res = Screen.resolutions[currentSettings.resolutionIndex];
        Screen.SetResolution(res.width, res.height, currentSettings.isFullscreen);
        QualitySettings.SetQualityLevel(currentSettings.graphicQuality);
    }
}

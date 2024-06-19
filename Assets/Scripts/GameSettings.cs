using UnityEngine;

[System.Serializable]
public class GraphicSettings
{
    public int resolutionIndex;
    public bool isFullscreen;
    public int graphicQuality;

    public GraphicSettings()
    {
        resolutionIndex = 0;
        isFullscreen = false;
        graphicQuality = 2;
    }
}

[System.Serializable]
public class SoundSettings
{
    public float masterVolume;
    public float musicVolume;   
    public float sfxVolume;

    public SoundSettings()
    {
        masterVolume = 1.0f;
        musicVolume = 1.0f;
        sfxVolume = 1.0f;
    }
}

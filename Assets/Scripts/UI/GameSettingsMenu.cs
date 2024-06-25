using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown graphicsQualityDropdown;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void OnEnable()
    {
        InitializeSettingsMenu();
        LoadSettingsUI();
    }

    private void InitializeSettingsMenu()
    {
        resolutionDropdown.ClearOptions();
        List<string> resolutions = new List<string>();
        foreach (Resolution res in Screen.resolutions)
        {
            resolutions.Add(res.width + " x " + res.height);
        }
        resolutionDropdown.AddOptions(resolutions);

        graphicsQualityDropdown.ClearOptions();
        graphicsQualityDropdown.AddOptions(new List<string> { "Low", "Medium", "High", "Ultra" });
    }

    private void LoadSettingsUI()
    {
        GraphicSettings graphicSettings = GraphicManager.Instance.currentSettings;
        resolutionDropdown.value = graphicSettings.resolutionIndex;
        fullscreenToggle.isOn = graphicSettings.isFullscreen;
        graphicsQualityDropdown.value = graphicSettings.graphicQuality;

        SoundSettings soundSettings = SoundManager.Instance.currentSettings;
        masterVolumeSlider.value = soundSettings.masterVolume;
        musicVolumeSlider.value = soundSettings.musicVolume;
        sfxVolumeSlider.value = soundSettings.sfxVolume;
    }

    public void SaveAndExit()
    {
        GraphicSettings graphicsSettings = new GraphicSettings
        {
            resolutionIndex = resolutionDropdown.value,
            isFullscreen = fullscreenToggle.isOn,
            graphicQuality = graphicsQualityDropdown.value
        };

        SoundSettings soundSettings = new SoundSettings
        {
            masterVolume = masterVolumeSlider.value,
            musicVolume = musicVolumeSlider.value,
            sfxVolume = sfxVolumeSlider.value
        };

        GraphicManager.Instance.SaveSettings(graphicsSettings);
        SoundManager.Instance.SaveSettings(soundSettings);

        UIManager.Instance.CloseUI(UIType.GameSettingsMenu);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.GameSettingsMenu);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }
}

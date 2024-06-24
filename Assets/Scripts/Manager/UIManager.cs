using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("===UI Elements for popups===")]
    [SerializeField] private GameObject notificationPopup;
    [SerializeField] private Text notificationText;
    [SerializeField] private GameObject nicknamePopup;
    [SerializeField] private InputField nicknameInputField;
    [SerializeField] private Text nicknameErrorText;
    [SerializeField] private ToggleGroup classToggleGroup;
    [SerializeField] private Toggle warriorToggle;
    [SerializeField] private Toggle archerToggle;
    [SerializeField] private Toggle wizardToggle;
    [SerializeField] private GameObject registerPopup;
    [SerializeField] private InputField registerUsernameInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private Text registerNotifiedText;

    [Header("===UI Elements for menus===")]
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameSlotMenu;
    [SerializeField] private GameObject gameSettingsMenu;
    [SerializeField] private InputField loginUsernameInput;
    [SerializeField] private InputField loginPasswordInput;
    [SerializeField] private Text loggedInUsernameText;

    [Header("===Game Settings for UI===")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown graphicsQualityDropdown;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("===Game Slot UI Elements===")]
    [SerializeField] private List<Button> gameSlotButtons;
    [SerializeField] private List<Text> gameSlotTexts;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private List<GameObject> defaultSlotObjects;
    [SerializeField] private List<GameObject> selectedSlotObjects;

    [Header("===Game Slot Data===")]
    [SerializeField] private List<GameSlot> gameSlots = new List<GameSlot>();
    [SerializeField] private int selectedSlotIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGameSlots();
            InitializeSettingsMenu();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGameSlots()
    {
        for(int i = 0; i< gameSlotButtons.Count; i++)
        {
            gameSlots.Add(new GameSlot());
            UpdateGameSlotUI(i);
        }
        UpdateStartGameButtonState();
    }

    private void UpdateGameSlotUI(int slotIndex)
    {
        if (gameSlots[slotIndex].isEmpty)
        {
            gameSlotTexts[slotIndex].text = "Empty Slot";
        }
        else
        {
            gameSlotTexts[slotIndex].text = $"Saved on: {gameSlots[slotIndex].savedTime}\n" +
                                             $"Player: {gameSlots[slotIndex].playerName}\n" +
                                             $"Level: {gameSlots[slotIndex].playerLevel}";
        }
    }

    private void UpdateStartGameButtonState()
    {
        startGameButton.interactable = selectedSlotIndex != -1;
    }

    private void ShowNotification(string message, float duration = 2.0f)
    {
        notificationText.text = message;
        notificationPopup.SetActive(true);
        Invoke(nameof(HideNotification), duration);
    }

    private void HideNotification()
    {
        notificationPopup.SetActive(false);
    }

    public void ShowNicknamePopup()
    {
        nicknameInputField.text = "";
        nicknameErrorText.text = "";
        classToggleGroup.SetAllTogglesOff();
        nicknamePopup.SetActive(true);
    }

    public void HideNicknamePopup()
    {
        nicknamePopup.SetActive(false);
    }

    private void ShowRegisterPopup()
    {
        registerPopup.SetActive(true);
        registerUsernameInput.text = "";
        registerPasswordInput.text = "";
        registerNotifiedText.text = "";
    }

    public void HideRegisterPopup()
    {
        registerPopup.SetActive(false);
    }

    public void OnGameSlotClicked(int slotIndex)
    {
        if (selectedSlotIndex == slotIndex)
        {
            DeselectGameSlot();
        }
        else
        {
            SelectGameSlot(slotIndex);
        }
    }

    private void SelectGameSlot(int slotIndex)
    {
        if(selectedSlotIndex != -1)
        {
            DeselectGameSlot();
        }

        selectedSlotIndex = slotIndex;
        defaultSlotObjects[slotIndex].SetActive(false);
        selectedSlotObjects[slotIndex].SetActive(true);
        UpdateStartGameButtonState();
    }

    private void DeselectGameSlot()
    {
        if (selectedSlotIndex != -1)
        {
            defaultSlotObjects[selectedSlotIndex].SetActive(true);
            selectedSlotObjects[selectedSlotIndex].SetActive(false);
        }
        selectedSlotIndex = -1;
        UpdateStartGameButtonState();
    }

    public void OnNewGameClicked()
    {
        ShowNicknamePopup();
    }

    public void OnLoadGameClicked()
    {
        ShowGameSlotMenu();

        for (int i = 0; i < gameSlots.Count; i++)
        {
            UpdateGameSlotUI(i);
        }
    }

    public void LoadGame()
    {
        if (selectedSlotIndex != -1)
        {
            GameData gameData = DataManager.Instance.LoadGameData(selectedSlotIndex);
            if (gameData == null)
            {
                ShowNicknamePopup();
            }
            else
            {
                LoadGameScene(gameData, false);
            }
        }
    }

    private void LoadGameScene(GameData gameData, bool isNewGame)
    {
        GameManager.Instance.SetGameData(gameData, isNewGame);
        SceneManager.LoadScene("GameScene");
    }

    public void ConfirmNickname()
    {
        string nickname = nicknameInputField.text;
        if (!classToggleGroup.AnyTogglesOn())
        {
            nicknameErrorText.text = "Please select a class.";
            return;
        }

        PlayerClass selectedClass = PlayerClass.None;
        if (warriorToggle.isOn) selectedClass = PlayerClass.Warrior;
        if (archerToggle.isOn) selectedClass = PlayerClass.Archer;
        if (wizardToggle.isOn) selectedClass = PlayerClass.Wizard;

        if (Utility.IsValidNickname(nickname))
        {
            GameData newGameData = new GameData
            {
                playerClass = selectedClass
                // 나머지 데이터 필드들도 초기화 필요
            };
            HideNicknamePopup();
            LoadGameScene(newGameData, true);
        }
        else
        {
            nicknameErrorText.text = $"Invalid nickname. Please enter a nickname between {Utility.MinNicknameLength} and {Utility.MaxNicknameLength} characters long, containing only letters and numbers.";
        }
    }

    public void CancelNickname()
    {
        HideNicknamePopup();
    }

    public void OnRegisterButtonClicked()
    {
        ShowRegisterPopup();
    }

    public void RegisterUser()
    {
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;

        if (UserManager.RegisterUser(username, password))
        {
            ShowRegistrationSuccessMessage();
        }
        else
        {
            ShowRegistrationFailureMessage();
        }
    }

    private void ShowRegistrationSuccessMessage()
    {
        registerNotifiedText.text = "User Registration Successful";
        registerNotifiedText.color = Color.green;
    }

    private void ShowRegistrationFailureMessage()
    {
        registerNotifiedText.text = "User Registration Failed. User already exists.";
        registerNotifiedText.color = Color.red;
    }

    public void OnLoginClicked()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;

        if (UserManager.ValidateUser(username, password))
        {
            ShowLoginSuccess(username);
        }
        else
        {
            ShowLoginFailure();
        }
    }

    private void ShowLoginSuccess(string username)
    {
        loggedInUsernameText.text = username;
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void ShowLoginFailure()
    {
        ShowNotification("로그인 실패.\n아이디 또는 비밀번호를 다시 확인해주세요.");
    }

    public void ShowGameSlotMenu()
    {
        mainMenu.SetActive(false);
        gameSlotMenu.SetActive(true);
    }

    public void ShowGameSettingsMenu()
    {
        mainMenu.SetActive(false);
        gameSettingsMenu.SetActive(true);
    }

    public void BackToMainMenuFromGameSlot()
    {
        gameSlotMenu.SetActive(false);
        mainMenu.SetActive(true);
        DeselectGameSlot();
    }

    public void BackToMainMenuFromGameSettings()
    {
        gameSettingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SaveAndExitSettings()
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

        gameSettingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Logout()
    {
        mainMenu.SetActive(false);
        loginMenu.SetActive(true);
        ShowNotification("로그아웃 성공");
    }

    private void InitializeSettingsMenu()
    {
        resolutionDropdown.ClearOptions();
        List<string> resolutions = new List<string>();
        foreach(Resolution res in Screen.resolutions)
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

    
}

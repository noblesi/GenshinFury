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
    [SerializeField] private GameObject warningPopup;
    [SerializeField] private Text warningText;
    [SerializeField] private GameObject nicknamePopup;
    [SerializeField] private InputField nicknameInputField;
    [SerializeField] private Text nicknameErrorText;
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

    [Header("===Game Slot UI Elements===")]
    [SerializeField] private List<Button> gameSlotButtons;
    [SerializeField] private List<Text> gameSlotTexts;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button cancelButton;

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

    public void ShowNotification(string message, float duration = 2.0f)
    {
        notificationText.text = message;
        notificationPopup.SetActive(true);
        Invoke(nameof(HideNotification), duration);
    }

    public void HideNotification()
    {
        notificationPopup.SetActive(false);
    }

    public void ShowWarning(string message)
    {
        warningText.text = message;
        warningPopup.SetActive(true);
    }

    public void HideWarning()
    {
        warningPopup.SetActive(false);
    }

    public void ShowNicknamePopup()
    {
        nicknameInputField.text = "";
        nicknameErrorText.text = "";
        nicknamePopup.SetActive(true);
    }

    public void HideNicknamePopup()
    {
        nicknamePopup.SetActive(false);
    }

    public void ShowRegisterPopup()
    {
        registerUsernameInput.text = "";
        registerPasswordInput.text = "";
        registerNotifiedText.text = "";
        registerPopup.SetActive(true);
    }

    public void HideRegisterPopup()
    {
        registerPopup.SetActive(false);
    }

    public void ShowLoginSuccess()
    {
        ShowNotification("로그인 성공.");
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowLoginFailure()
    {
        ShowNotification("로그인 실패.\n아이디 또는 비밀번호를 다시 확인해주세요.");
    }

    public void ShowRegistrationSuccessMessage()
    {
        registerNotifiedText.text = "User Registration Successful";
        registerNotifiedText.color = Color.green;
    }

    public void ShowRegistrationFailureMessage()
    {
        registerNotifiedText.text = "User Registration Failed. User already exists.";
        registerNotifiedText.color = Color.red;
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

    public void Logout()
    {
        mainMenu.SetActive(false);
        loginMenu.SetActive(true);
        ShowNotification("로그아웃 성공");
    }

    public void OnNewGameClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnLoadGameClicked()
    {
        ShowGameSlotMenu();

        for (int i = 0; i < gameSlots.Count; i++)
        {
            UpdateGameSlotUI(i);
        }
    }

    public void OnGameSettingsClicked()
    {
        ShowGameSettingsMenu(); 
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

    public void OnGameSlotClicked(int slotIndex)
    {
        if(selectedSlotIndex == slotIndex)
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
        selectedSlotIndex = slotIndex;
        UpdateStartGameButtonState();
    }

    private void DeselectGameSlot()
    {
        selectedSlotIndex = -1;
        UpdateStartGameButtonState();
    }

    private void UpdateStartGameButtonState()
    {
        startGameButton.interactable = selectedSlotIndex != -1;
    }

    public void ConfirmOverwrite()
    {
        HideWarning();
        ShowNicknamePopup();
    }

    public void CancelOverwrite()
    {
        HideWarning();
    }

    public void ConfirmNickname()
    {
        string nickname = nicknameInputField.text;
        if (Utility.IsValidNickname(nickname))
        {
            gameSlots[selectedSlotIndex].isEmpty = false;
            gameSlots[selectedSlotIndex].savedTime = DateTime.Now;
            gameSlots[selectedSlotIndex].playerName = nickname;
            gameSlots[selectedSlotIndex].playerLevel = 1; // Initial level
            UpdateGameSlotUI(selectedSlotIndex);
            HideNicknamePopup();
            SceneManager.LoadScene("GameScene"); // Replace with actual game scene name
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

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnRegisterButtonClicked()
    {
        ShowRegisterPopup();
    }

    public void RegisterUser()
    {
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;

        if(UserManager.RegisterUser(username, password))
        {
            ShowRegistrationSuccessMessage();
        }
        else
        {
            ShowRegistrationFailureMessage();
        }
    }

    public void OnLoginClicked()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;

        if(UserManager.ValidateUser(username, password))
        {
            ShowLoginSuccess();
        }
        else
        {
            ShowLoginFailure();
        }
    }
}

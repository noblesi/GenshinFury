using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("===UI Elements for popups===")]
    [SerializeField] private GameObject notificationPopup;
    [SerializeField] private Text notificationText;
    [SerializeField] private GameObject warningPopup;
    [SerializeField] private Text warningText;
    [SerializeField] private GameObject nicknamePopup;
    [SerializeField] private InputField nicknameInputField;

    [Header("===UI Elements for menus===")]
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameSlotMenu;
    [SerializeField] private GameObject gameSettingsMenu;

    [Header("===Game Slot UI Elements===")]
    [SerializeField] private List<Button> gameSlotButtons;
    [SerializeField] private List<Text> gameSlotTexts;

    [Header("===Game Slot Data")]
    [SerializeField] private List<GameSlot> gameSlots = new List<GameSlot>();
    [SerializeField] private int selectedSlotIndex;

    private void InitializeGameSlots()
    {
        for(int i = 0; i< gameSlotButtons.Count; i++)
        {
            gameSlots.Add(new GameSlot());
            UpdateGameSlotUI(i);
        }
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
        nicknamePopup.SetActive(true);
    }

    public void HideNicknamePopup()
    {
        nicknamePopup.SetActive(false);
    }

    public void ShowLoginSuccess()
    {
        ShowNotification("�α��� ����.");
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowLoginFailure()
    {
        ShowNotification("�α��� ����.\n���̵� �Ǵ� ��й�ȣ�� �ٽ� Ȯ�����ּ���.");
    }

    public void ShowRegistrationSuccess()
    {
        ShowNotification("��������� ��� ����.");
    }

    public void ShowRegistrationFailure()
    {
        ShowNotification("��������� ��� ����.\n���̵� �Ǵ� ��й�ȣ�� �ٽ� Ȯ�����ּ���.");
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
        ShowNotification("�α׾ƿ� ����");
    }

    public void OnNewGameClicked()
    {
        ShowGameSlotMenu();

        for(int i = 0; i < gameSlots.Count; i++)
        {
            UpdateGameSlotUI(i);
        }
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
    }

    public void BackToMainMenuFromGameSettings()
    {
        gameSettingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OnGameSlotClicked(int slotIndex)
    {
        selectedSlotIndex = slotIndex;
        if (gameSlots[slotIndex].isEmpty)
        {
            ShowNicknamePopup();
        }
        else
        {
            ShowWarning("This slot contains saved data. Starting a new game will overwrite it. Do you want to continue?");
        }
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
        if (!string.IsNullOrEmpty(nickname))
        {
            gameSlots[selectedSlotIndex].isEmpty = false;
            gameSlots[selectedSlotIndex].savedTime = DateTime.Now;
            gameSlots[selectedSlotIndex].playerName = nickname;
            gameSlots[selectedSlotIndex].playerLevel = 1; // Initial level
            UpdateGameSlotUI(selectedSlotIndex);
            HideNicknamePopup();
            SceneManager.LoadScene("GameScene"); // Replace with actual game scene name
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
}

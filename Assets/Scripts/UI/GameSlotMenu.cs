using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSlotMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Text loggedInUsernameText;

    [SerializeField] private List<Button> gameSlotButtons;
    [SerializeField] private List<Text> gameSlotTexts;
    [SerializeField] private List<Image> gameSlotImages;  // 각 슬롯의 이미지 컴포넌트
    [SerializeField] private Sprite defaultSprite;  // 비선택 상태 스프라이트
    [SerializeField] private Sprite selectedSprite;  // 선택 상태 스프라이트
    [SerializeField] private Vector2 defaultTextPosition;
    [SerializeField] private Vector2 selectedTextPosition;

    private List<GameSlot> gameSlots;
    private int selectedSlotIndex = -1;

    private void OnEnable()
    {
        InitializeGameSlots();
        EventManager.Instance.OnLoginSuccess += UpdateUsername;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnLoginSuccess -= UpdateUsername;
    }

    private void UpdateUsername(string username)
    {
        loggedInUsernameText.text = username;
    }

    private void InitializeGameSlots()
    {
        gameSlots = new List<GameSlot>();

        for (int i = 0; i < gameSlotButtons.Count; i++)
        {
            gameSlots.Add(new GameSlot());
            UpdateGameSlotUI(i);
        }

        UpdateStartGameButtonState();
    }

    private void UpdateGameSlotUI(int slotIndex)
    {
        if (slotIndex >= gameSlots.Count) return;  // 인덱스 범위를 벗어나는 경우 처리

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

        gameSlotImages[slotIndex].sprite = defaultSprite;
        gameSlotTexts[slotIndex].rectTransform.anchoredPosition = defaultTextPosition;
    }

    private void UpdateStartGameButtonState()
    {
        startGameButton.interactable = selectedSlotIndex != -1;
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
        if (selectedSlotIndex != -1)
        {
            gameSlotImages[selectedSlotIndex].sprite = defaultSprite;
            gameSlotTexts[selectedSlotIndex].rectTransform.anchoredPosition = defaultTextPosition;
        }

        selectedSlotIndex = slotIndex;
        gameSlotImages[slotIndex].sprite = selectedSprite;
        gameSlotTexts[slotIndex].rectTransform.anchoredPosition = selectedTextPosition;

        UpdateStartGameButtonState();
    }

    private void DeselectGameSlot()
    {
        if (selectedSlotIndex != -1)
        {
            gameSlotImages[selectedSlotIndex].sprite = defaultSprite;
            gameSlotTexts[selectedSlotIndex].rectTransform.anchoredPosition = defaultTextPosition;
            selectedSlotIndex = -1;
        }

        UpdateStartGameButtonState();
    }

    public void LoadGame()
    {
        if (selectedSlotIndex != -1)
        {
            GameData gameData = DataManager.Instance.LoadGameData(selectedSlotIndex);
            if (gameData == null)
            {
                UIManager.Instance.OpenUI(UIType.PlayerSettingsPopup);
            }
            else
            {
                Vector3 spawnPosition = new Vector3(0, 0, 0); // 스폰 위치 설정
                Quaternion spawnRotation = Quaternion.identity; // 스폰 회전 설정
                UIManager.Instance.StartGame(gameData, false, spawnPosition, spawnRotation);
            }
        }
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.GameSlotMenu);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }
}

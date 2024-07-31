using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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
        gameSlots = DataManager.Instance.LoadAllGameSlots();

        for (int i = 0; i < gameSlotButtons.Count; i++)
        {
            if (i < gameSlots.Count)
            {
                UpdateGameSlotUI(i);
            }
            else
            {
                gameSlots.Add(new GameSlot(i));
                UpdateGameSlotUI(i);
            }
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
            GameSlot selectedSlot = gameSlots[selectedSlotIndex];
            if (selectedSlot.isEmpty)
            {
                // 게임 데이터를 새로 생성
                GameData newGameData = new GameData
                {
                    name = "New Player",
                    level = 1,
                    savedTime = DateTime.Now
                };
                DataManager.Instance.SaveGameData(selectedSlotIndex, newGameData);
                selectedSlot.character = new Character(newGameData.name, newGameData.level, newGameData.savedTime);
            }
            GameData gameData = DataManager.Instance.LoadGameData(selectedSlotIndex);
            DataManager.Instance.SetSelectedCharacter(selectedSlot.character);
            UIManager.Instance.StartGame();
        }
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.GameSlotMenu);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSlotMenu : MonoBehaviour
{
    [SerializeField] private List<Button> gameSlotButtons;
    [SerializeField] private List<Text> gameSlotTexts;
    [SerializeField] private Button startGameButton;
    [SerializeField] private List<GameObject> defaultSlotObjects;
    [SerializeField] private List<GameObject> selectedSlotObjects;
    [SerializeField] private List<GameSlot> gameSlots = new List<GameSlot>();
    private int selectedSlotIndex = -1;

    private void OnEnable()
    {
        InitializeGameSlots();
    }

    private void InitializeGameSlots()
    {
        for (int i = 0; i < gameSlotButtons.Count; i++)
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
                UIManager.Instance.LoadGameScene(gameData, false);
            }
        }
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.GameSlotMenu);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }
}

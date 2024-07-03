using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInfoUI : MonoBehaviour
{
    [SerializeField] private ToggleGroup dungeonToggleGroup;
    [SerializeField] private ToggleGroup difficultyToggleGroup;
    [SerializeField] private Button enterButton;

    private void Start()
    {
        enterButton.onClick.AddListener(OnEnterButtonClick);
    }

    private void OnEnterButtonClick()
    {
        string selectedDungeon = GetSelectedToggle(dungeonToggleGroup).GetComponentInChildren<Text>().text;
        string selectedDifficultyText = GetSelectedToggle(difficultyToggleGroup).GetComponentInChildren<Text>().text;
        Difficulty selectedDifficulty = (Difficulty)System.Enum.Parse(typeof(Difficulty), selectedDifficultyText);

        DungeonManager.Instance.SelectDungeon(selectedDungeon, selectedDifficulty);
        DungeonManager.Instance.EnterDungeon();
        UIManager.Instance.CloseUI(UIType.DungeonInfoUI);
    }

    private Toggle GetSelectedToggle(ToggleGroup toggleGroup)
    {
        return toggleGroup.ActiveToggles().FirstOrDefault();
    }
}

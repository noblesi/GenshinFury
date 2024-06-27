using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsPopup : MonoBehaviour
{
    [SerializeField] private InputField nicknameInputField;
    [SerializeField] private Text nicknameErrorText;

    [SerializeField] private ToggleGroup classToggleGroup;
    [SerializeField] private Toggle warriorToggle;
    [SerializeField] private Toggle archerToggle;
    [SerializeField] private Toggle wizardToggle;
    [SerializeField] private Text classErrorText;

    private void OnEnable()
    {
        ResetUI();
    } 

    private void ResetUI()
    {
        nicknameInputField.text = string.Empty;
        nicknameErrorText.text = string.Empty;
        classErrorText.text = string.Empty;
        classToggleGroup.SetAllTogglesOff();
    }

    public void ConfirmNickname()
    {
        string nickname = nicknameInputField.text;
        if (Utility.IsValidNickname(nickname))
        {
            // 닉네임이 유효한 경우
            PlayerSettings.Instance.SetNickname(nickname);
            nicknameErrorText.text = string.Empty;
        }
        else
        {
            // 닉네임이 유효하지 않은 경우
            nicknameErrorText.text = "Invalid Nickname!\nPlease check nickname.";
        }
    }

    public void StartGame()
    {
        if (!classToggleGroup.AnyTogglesOn())
        {
            classErrorText.text = "Please select a class.";
            return;
        }

        PlayerClass selectedClass = PlayerClass.None;
        if (warriorToggle.isOn) selectedClass = PlayerClass.Warrior;
        if (archerToggle.isOn) selectedClass = PlayerClass.Archer;
        if (wizardToggle.isOn) selectedClass = PlayerClass.Wizard;

        PlayerSettings.Instance.SetPlayerClass(selectedClass);
        classErrorText.text = string.Empty;

        UIManager.Instance.CloseUI(UIType.PlayerSettingsPopup);

        Vector3 spawnPosition = new Vector3(-13.14f, 0, 22.63f); // 스폰 위치 설정
        Quaternion spawnRotation = Quaternion.identity; // 스폰 회전 설정

        UIManager.Instance.StartGame(new GameData
        {
            playerClass = selectedClass,
            name = PlayerSettings.Instance.Nickname
            // 나머지 데이터 필드들도 초기화 필요
        }, true, spawnPosition, spawnRotation);
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.PlayerSettingsPopup);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }
}

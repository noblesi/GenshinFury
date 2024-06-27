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
            // �г����� ��ȿ�� ���
            PlayerSettings.Instance.SetNickname(nickname);
            nicknameErrorText.text = string.Empty;
        }
        else
        {
            // �г����� ��ȿ���� ���� ���
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

        Vector3 spawnPosition = new Vector3(-13.14f, 0, 22.63f); // ���� ��ġ ����
        Quaternion spawnRotation = Quaternion.identity; // ���� ȸ�� ����

        UIManager.Instance.StartGame(new GameData
        {
            playerClass = selectedClass,
            name = PlayerSettings.Instance.Nickname
            // ������ ������ �ʵ�鵵 �ʱ�ȭ �ʿ�
        }, true, spawnPosition, spawnRotation);
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.PlayerSettingsPopup);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }
}

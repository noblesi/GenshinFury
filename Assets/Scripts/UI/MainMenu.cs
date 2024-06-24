using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OpenGameSlotMenu()
    {
        UIManager.Instance.CloseUI(UIType.MainMenu);
        UIManager.Instance.OpenUI(UIType.GameSlotMenu);
    }

    public void OpenGameSettingsMenu()
    {
        UIManager.Instance.CloseUI(UIType.MainMenu);
        UIManager.Instance.OpenUI(UIType.GameSettingsMenu);
    }

    public void Logout()
    {
        UIManager.Instance.CloseUI(UIType.MainMenu);
        UIManager.Instance.OpenUI(UIType.LoginMenu);
    }
}

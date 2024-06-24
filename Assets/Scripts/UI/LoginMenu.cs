using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private InputField loginUsernameInput;
    [SerializeField] private InputField loginPasswordInput;
    [SerializeField] private Text loggedInUsernameText;
    [SerializeField] private Text loginErrorText;

    private void OnEnable()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        loginUsernameInput.text = string.Empty;
        loginPasswordInput.text = string.Empty;
        loginErrorText.text = string.Empty; // 에러 메시지 초기화
    }

    public void Login()
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
        UIManager.Instance.CloseUI(UIType.LoginMenu);
        UIManager.Instance.OpenUI(UIType.MainMenu);
    }

    private void ShowLoginFailure()
    {
        loginErrorText.text = "Login Failure. Check ID or PWD.";
    }

    public void ClickRegisterBtn()
    {
        UIManager.Instance.OpenUI(UIType.RegisterPopup, true);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text loggedInUsernameText;

    private void OnEnable()
    {
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

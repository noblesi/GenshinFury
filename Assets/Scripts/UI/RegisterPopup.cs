using UnityEngine;
using UnityEngine.UI;

public class RegisterPopup : MonoBehaviour
{
    [SerializeField] private InputField registerUsernameInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private Text registerNotifiedText;

    private void OnEnable()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        registerUsernameInput.text = "";
        registerPasswordInput.text = "";
        registerNotifiedText.text = "";
    }

    public void RegisterUser()
    {
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;

        if (UserManager.RegisterUser(username, password))
        {
            ShowRegistrationSuccessMessage();
        }
        else
        {
            ShowRegistrationFailureMessage();
        }
    }

    private void ShowRegistrationSuccessMessage()
    {
        registerNotifiedText.text = "User Registration Successful";
        registerNotifiedText.color = Color.green;
    }

    private void ShowRegistrationFailureMessage()
    {
        registerNotifiedText.text = "User Registration Failed. User already exists.";
        registerNotifiedText.color = Color.red;
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.RegisterPopup);
    }
}

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

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowRegistrationFailureMessage("Username and password cannot be empty.");
            Debug.Log("Registration failed: Username or password is empty.");
            return;
        }

        Debug.Log($"Trying to register user with Username: {username}, Password: {password}");

        if (UserManager.RegisterUser(username, password))
        {
            ShowRegistrationSuccessMessage();
            DataManager.Instance.CreateAccount(username, password); // 사용자 데이터 저장
            Debug.Log("User registration successful.");
        }
        else
        {
            ShowRegistrationFailureMessage("User registration failed. User already exists.");
            Debug.Log("User registration failed. User already exists.");
        }
    }

    private void ShowRegistrationSuccessMessage()
    {
        registerNotifiedText.text = "User Registration Successful";
        registerNotifiedText.color = Color.green;
    }

    private void ShowRegistrationFailureMessage(string message)
    {
        registerNotifiedText.text = message;
        registerNotifiedText.color = Color.red;
    }

    public void Cancel()
    {
        UIManager.Instance.CloseUI(UIType.RegisterPopup);
    }
}

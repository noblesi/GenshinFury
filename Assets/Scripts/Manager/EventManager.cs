using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    // �α��� �̺�Ʈ
    public event Action<string> OnLoginSuccess;
    public event Action OnLoginFailure;
    public event Action OnLogout;

    // ���� �̺�Ʈ
    public event Action OnBattleEnded;
    public event Action OnApplicationQuitEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �α��� ���� �̺�Ʈ Ʈ���� �޼���
    public void LoginSuccess(string username)
    {
        OnLoginSuccess?.Invoke(username);
    }

    public void LoginFailure()
    {
        OnLoginFailure?.Invoke();
    }

    public void Logout()
    {
        OnLogout?.Invoke();
    }

    // ���� �̺�Ʈ Ʈ���� �޼���
    public void BattleEnded()
    {
        OnBattleEnded?.Invoke();
    }

    public void ApplicationQuit()
    {
        OnApplicationQuitEvent?.Invoke();
    }
}

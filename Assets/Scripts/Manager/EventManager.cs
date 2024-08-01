using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    // 로그인 이벤트
    public event Action<string> OnLoginSuccess;
    public event Action OnLoginFailure;
    public event Action OnLogout;

    // 게임 이벤트
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

    // 로그인 관련 이벤트 트리거 메서드
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

    // 게임 이벤트 트리거 메서드
    public void BattleEnded()
    {
        OnBattleEnded?.Invoke();
    }

    public void ApplicationQuit()
    {
        OnApplicationQuitEvent?.Invoke();
    }
}

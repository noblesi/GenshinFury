using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    NONE,
    START,
    GAME,
    END
}
public class SystemManager : MonoSingleton<SystemManager>
{
    [SerializeField] private GameState GameState = GameState.NONE;

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public void SetState(GameState state)
    {
        GameState = state;
        ProcessState();
    }

    public GameState GetState()
    {
        return GameState;
    }

    private void ProcessState()
    {
        switch(GameState)
        {
            case GameState.START:
                SceneManager.LoadScene("Start");
                break;
            case GameState.GAME:
                StartCoroutine(LoadSceneAsyncCoroutine("Game"));
                break;
            case GameState.END:
                SceneManager.LoadScene("End");
                break;
        }
    }

    IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName);

        handle.allowSceneActivation = false;

        while(handle.progress < 0.9)
        {
            Debug.Log(handle.progress);

            yield return null;
        }
        float progress = handle.progress + 0.1f;
        Debug.Log(handle.progress);

        handle.allowSceneActivation = true;
    }
}

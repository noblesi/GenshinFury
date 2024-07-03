using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance {  get; private set; }

    private string selectedDungeon;
    private Difficulty selectedDifficulty;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectDungeon(string dungeon, Difficulty diff)
    {
        selectedDungeon = dungeon;
        selectedDifficulty = diff;
    }

    public void EnterDungeon()
    {
        StartCoroutine(LoadDungeonScene(selectedDungeon));
    }

    private IEnumerator LoadDungeonScene(string dungeon)
    {
        LoadingScreen.Instance.Show();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(dungeon);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        LoadingScreen.Instance.Hide();

        InitializeDungeon();
    }

    private void InitializeDungeon()
    {
        DungeonSetup setup = FindObjectOfType<DungeonSetup>();
        if(setup != null)
        {
            setup.SetupDifficulty(selectedDifficulty);
        }
    }
}

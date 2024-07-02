using Cinemachine;
using System;
using System.Collections;
using System.Web;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerClass
{
    None, Warrior, Archer, Wizard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("===Player Settings===")]
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private GameData currentGameData;
    private bool isNewGame;

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

    public void StartGame()
    {
        StartCoroutine(LoadSceneAsync("GameScene"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        GameObject playerPrefab = null;

        switch (currentGameData.playerClass)
        {
            case PlayerClass.Warrior:
                playerPrefab = warriorPrefab;
                break;
            case PlayerClass.Archer:
                playerPrefab = archerPrefab;
                break;
            case PlayerClass.Wizard:
                playerPrefab = wizardPrefab;
                break;
            default:
                Debug.LogError("Invalid player class.");
                return;
        }

        if (playerPrefab != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            Player player = playerObject.GetComponent<Player>();

            if (player != null)
            {
                player.Initialize();

                CinemachineVirtualCamera virtualCamera = Instantiate(virtualCameraPrefab);

                virtualCamera.Follow = playerObject.transform;
            }
        }
        else
        {
            Debug.LogError("Player prefab not set in GameManager.");
        }
    }

    public void SetGameData(GameData gameData, bool isNewGame)
    {
        currentGameData = gameData;
        this.isNewGame = isNewGame;
    }

    public void SetSpawnPoint(Vector3 position, Quaternion rotation)
    {
        spawnPosition = position;
        spawnRotation = rotation;
    }
}

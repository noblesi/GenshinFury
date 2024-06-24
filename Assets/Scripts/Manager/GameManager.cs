using System;
using System.Collections.Generic;
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
    [SerializeField] private Transform playerSpawnPoint;

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
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            InstantiatePlayer();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void InstantiatePlayer()
    {
        GameObject playerPrefab = null;  // playerPrefab을 조건문 외부에서 선언

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

        if (playerPrefab != null && playerSpawnPoint != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
            Player player = playerObject.GetComponent<Player>();

            if (player != null)
            {
                player.Initialize(currentGameData);  // InitializePlayer -> Initialize로 변경
            }
        }
        else
        {
            Debug.LogError("Player prefab or spawn point not set in GameManager.");
        }
    }

    public void SetGameData(GameData gameData, bool isNewGame)
    {
        currentGameData = gameData;
        this.isNewGame = isNewGame;
    }
}

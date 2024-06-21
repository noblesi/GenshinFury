using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        if(currentGameData == null)
        {
            Debug.LogError("No game data found. Please start a new game or load an existing one.");
            return;
        }

        if (isNewGame)
        {
            StartNewGame(currentGameData);
        }
        else
        {
            LoadExistingGame(currentGameData);
        }

        //InitializeGameSceneSettings();
    }


    private void StartNewGame(GameData gameData)
    {
        InitializePlayer(gameData);
    }

    private void LoadExistingGame(GameData gameData)
    {
        InitializePlayer(gameData);
    }

    private void InitializePlayer(GameData gameData)
    {
        GameObject playerPrefab = null;
        switch (gameData.playerClass)
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
            GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
            Player playerComponent = player.GetComponent<Player>();

            if (playerComponent != null)
            {
                playerComponent.Initialize(gameData);
            }
        }
        else
        {
            Debug.LogError("PlayerPrefab or PlayerSpawnPoint is not set in the GameManager.");
        }
    }

    public void SetGameData(GameData gameData, bool isNewGame)
    {
        currentGameData = gameData;
        this.isNewGame = isNewGame;
    }
}

using Cinemachine;
using System.Collections;
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

    [Header("===Player Data===")]
    [SerializeField] private PlayerData warriorData;
    [SerializeField] private PlayerData archerData;
    [SerializeField] private PlayerData wizardData;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private GameData currentGameData;
    private bool isNewGame;

    public delegate void PlayerCreated(Player player);
    public event PlayerCreated OnPlayerCreated;

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
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame(); // UI 설정 전에 약간의 지연을 추가
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        GameObject playerPrefab = null;
        PlayerData playerData = null;

        switch (currentGameData.playerClass)
        {
            case PlayerClass.Warrior:
                playerPrefab = warriorPrefab;
                playerData = warriorData;
                break;
            case PlayerClass.Archer:
                playerPrefab = archerPrefab;
                playerData = archerData;
                break;
            case PlayerClass.Wizard:
                playerPrefab = wizardPrefab;
                playerData = wizardData;
                break;
            default:
                Debug.LogError("Invalid player class.");
                return;
        }

        if (playerPrefab != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            Player player = playerObject.GetComponent<Player>();

            if (player != null && playerData != null)
            {
                player.Initialize(playerData);

                CinemachineVirtualCamera virtualCamera = Instantiate(virtualCameraPrefab);
                virtualCamera.Follow = playerObject.transform;

                OnPlayerCreated?.Invoke(player);
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

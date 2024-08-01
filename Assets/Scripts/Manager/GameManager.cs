using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("===Player Settings===")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;
    [SerializeField] private GameObject spawnPointPrefab;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

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
        LoadScene("GameScene");
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        StartCoroutine(WaitForSceneLoad(sceneName));
    }

    private IEnumerator WaitForSceneLoad(string sceneName)
    {
        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }

        InstantiateSpawnPoint();
        InstantiatePlayer();
    }

    private void InstantiateSpawnPoint()
    {
        if (spawnPointPrefab != null)
        {
            GameObject spawnPointObject = Instantiate(spawnPointPrefab);
            spawnPosition = spawnPointObject.transform.position;
            spawnRotation = spawnPointObject.transform.rotation;
        }
        else
        {
            Debug.LogError("SpawnPoint prefab is not set in GameManager.");
        }
    }

    private void InstantiatePlayer()
    {
        Character selectedCharacter = DataManager.Instance.GetSelectedCharacter();

        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not set in GameManager.");
            return;
        }

        if (selectedCharacter == null)
        {
            Debug.LogError("Selected character is not set in GameManager.");
            return;
        }

        Debug.Log("Instantiating player...");

        GameObject playerObject = Instantiate(playerPrefab, spawnPosition, spawnRotation);
        Player player = playerObject.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log("Player component found, initializing player...");

            PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();
            playerData.level = selectedCharacter.level;
            playerData.strength = selectedCharacter.level * 10;
            playerData.dexterity = selectedCharacter.level * 10;
            playerData.intelligence = selectedCharacter.level * 10;
            playerData.maxHealth = CalculateMaxHealth(playerData);
            playerData.currentHealth = playerData.maxHealth;
            playerData.maxMana = CalculateMaxMana(playerData);
            playerData.currentMana = playerData.maxMana;

            player.Initialize(playerData);

            CinemachineVirtualCamera virtualCamera = Instantiate(virtualCameraPrefab);
            virtualCamera.Follow = playerObject.transform;

            OnPlayerCreated?.Invoke(player);
        }
        else
        {
            Debug.LogError("Player component not found on instantiated player prefab.");
        }
    }

    private int CalculateMaxHealth(PlayerData playerData)
    {
        return 100 + (playerData.strength * 2) + (int)(playerData.dexterity * 1.5f) + (playerData.level * 10);
    }

    private int CalculateMaxMana(PlayerData playerData)
    {
        return 50 + (playerData.intelligence * 2) + (playerData.level * 5);
    }
}
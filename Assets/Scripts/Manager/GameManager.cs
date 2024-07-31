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
    [SerializeField] private GameObject spawnPointPrefab; // SpawnPoint ������ ����

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
            playerData.maxHealth = selectedCharacter.level * 10;  // ���Ƿ� ���� �� ����
            playerData.currentHealth = selectedCharacter.level * 10;
            playerData.maxMana = selectedCharacter.level * 5;  // ���Ƿ� ���� �� ����
            playerData.currentMana = selectedCharacter.level * 5;
            playerData.strength = selectedCharacter.level;  // ���Ƿ� ���� �� ����
            playerData.dexterity = selectedCharacter.level;  // ���Ƿ� ���� �� ����
            playerData.intelligence = selectedCharacter.level;  // ���Ƿ� ���� �� ����

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
}

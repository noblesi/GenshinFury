using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIType
{
    PlayerSettingsPopup,
    RegisterPopup,
    LoginMenu,
    MainMenu,
    GameSlotMenu,
    GameSettingsMenu,
    DungeonInfoUI
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject UIRoot;

    private Dictionary<UIType, GameObject> _createdUIDic = new Dictionary<UIType, GameObject>();
    private HashSet<UIType> _openedUIDic = new HashSet<UIType>();

    private void Awake()
    {
        Instance = this;
    }

    public void OpenUI(UIType uiType)
    {
        if (_openedUIDic.Contains(uiType) == false)
        {
            var uiObject = GetCreatedUI(uiType);
            if (uiObject != null)
            {
                uiObject.SetActive(true);
                _openedUIDic.Add(uiType);
            }
        }
    }

    public void CloseUI(UIType uiType)
    {
        if (_openedUIDic.Contains(uiType))
        {
            var uiObject = _createdUIDic[uiType];
            uiObject.SetActive(false);
            _openedUIDic.Remove(uiType);
        }
    }

    public GameObject GetCreatedUI(UIType uiType)
    {
        if (_createdUIDic.ContainsKey(uiType) == false)
        {
            CreateUI(uiType);
        }

        return _createdUIDic[uiType];
    }

    private void CreateUI(UIType uiType)
    {
        string path = GetUIPath(uiType);
        GameObject loadedObj = (GameObject)Resources.Load(path);
        GameObject gObj = Instantiate(loadedObj, UIRoot.transform);
        if (gObj != null)
        {
            _createdUIDic.Add(uiType, gObj);
        }
    }

    private string GetUIPath(UIType uiType)
    {
        string path = string.Empty;
        switch (uiType)
        {
            case UIType.PlayerSettingsPopup:
                path = "Prefabs/UI/PlayerSettingsPopup";
                break;
            case UIType.RegisterPopup:
                path = "Prefabs/UI/RegisterPopup";
                break;
            case UIType.LoginMenu:
                path = "Prefabs/UI/LoginMenu";
                break;
            case UIType.MainMenu:
                path = "Prefabs/UI/MainMenu";
                break;
            case UIType.GameSlotMenu:
                path = "Prefabs/UI/GameSlotMenu";
                break;
            case UIType.GameSettingsMenu:
                path = "Prefabs/UI/GameSettingsMenu";
                break;
            case UIType.DungeonInfoUI:
                path = "Prefabs/UI/DungeonInfoUI";
                break;
        }
        return path;
    }

    public void StartGame(GameData gameData, bool isNewGame, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameManager.Instance.SetGameData(gameData, isNewGame);
        GameManager.Instance.SetSpawnPoint(spawnPosition, spawnRotation);
        GameManager.Instance.StartGame();
    }
}

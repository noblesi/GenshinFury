using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIType
{
    NotificationPopup,
    PlayerSettingsPopup,
    RegisterPopup,
    LoginMenu,
    MainMenu,
    GameSlotMenu,
    GameSettingsMenu,
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject UIRoot;
    [SerializeField] private GameObject modalBackground;

    private Dictionary<UIType, GameObject> _createdUIDic = new Dictionary<UIType, GameObject>();
    private HashSet<UIType> _openedUIDic = new HashSet<UIType>();

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

    public void OpenUI(UIType uiType, bool isModal = false)
    {
        if (_openedUIDic.Contains(uiType) == false)
        {
            var uiObject = GetCreatedUI(uiType);
            if (uiObject != null)
            {
                uiObject.SetActive(true);
                _openedUIDic.Add(uiType);
                if (isModal)
                {
                    modalBackground.SetActive(true);
                    modalBackground.transform.SetAsLastSibling();
                    uiObject.transform.SetAsLastSibling();
                }
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
            modalBackground.SetActive(false);
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
            case UIType.NotificationPopup:
                path = "Prefabs/UI/NotificationPopup";
                break;
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
        }
        return path;
    }

    public void LoadGameScene(GameData gameData, bool isNewGame)
    {
        GameManager.Instance.SetGameData(gameData, isNewGame);
        SceneManager.LoadScene("GameScene");
    }
}

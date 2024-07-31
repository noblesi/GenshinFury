using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIType
{
    RegisterPopup,
    LoginMenu,
    MainMenu,
    GameSlotMenu,
    GameSettingsMenu,
    PlayerHUD,
    Inventory
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject UIRoot;

    private Dictionary<UIType, GameObject> _createdUIDic = new Dictionary<UIType, GameObject>();
    private HashSet<UIType> _openedUIDic = new HashSet<UIType>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("UIManager Instance set");
            OpenUI(UIType.LoginMenu);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate UIManager instance destroyed");
        }
    }

    public void OpenUI(UIType uiType)
    {
        if (!_openedUIDic.Contains(uiType))
        {
            var uiObject = GetCreatedUI(uiType);
            if (uiObject != null)
            {
                uiObject.SetActive(true);
                _openedUIDic.Add(uiType);
                Debug.Log($"Opened UI: {uiType}");
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
            Debug.Log($"Closed UI: {uiType}");
        }
    }

    public void ToggleUI(UIType uiType)
    {
        if (_openedUIDic.Contains(uiType))
        {
            CloseUI(uiType);
        }
        else
        {
            OpenUI(uiType);
        }
    }

    public GameObject GetCreatedUI(UIType uiType)
    {
        if (!_createdUIDic.ContainsKey(uiType))
        {
            CreateUI(uiType);
        }

        return _createdUIDic[uiType];
    }

    private void CreateUI(UIType uiType)
    {
        string path = GetUIPath(uiType);
        GameObject loadedObj = Resources.Load<GameObject>(path);
        GameObject gObj = Instantiate(loadedObj, UIRoot.transform);
        if (gObj != null)
        {
            _createdUIDic.Add(uiType, gObj);
            Debug.Log($"Created UI: {uiType}");
        }
        else
        {
            Debug.LogError($"Failed to create UI: {uiType}");
        }
    }

    private string GetUIPath(UIType uiType)
    {
        string path = string.Empty;
        switch (uiType)
        {
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
            case UIType.PlayerHUD:
                path = "Prefabs/UI/PlayerHUD";
                break;
            case UIType.Inventory:
                path = "Prefabs/UI/Inventory";
                break;
        }
        return path;
    }

    public void StartGame()
    {
        CloseUI(UIType.GameSlotMenu); // ∞‘¿” ΩΩ∑‘ ∏ﬁ¥∫∏¶ ¥›¿Ω
        GameManager.Instance.StartGame();
    }
}

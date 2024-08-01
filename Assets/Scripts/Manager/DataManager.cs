using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private Account currentAccount;
    private Character selectedCharacter;
    private string currentSlotFilePath;

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

        LoadAccountData();
    }

    public void CreateAccount(string username, string password)
    {
        currentAccount = new Account(username, password);
        SaveAccountData();
    }

    public bool Login(string username, string password)
    {
        LoadAccountData();
        if (currentAccount != null && currentAccount.username == username && currentAccount.password == password)
        {
            LoadLastPlayedSlot();
            return true;
        }
        return false;
    }

    public List<GameSlot> LoadAllGameSlots()
    {
        if (currentAccount == null)
        {
            Debug.LogError("No account is logged in.");
            return new List<GameSlot>();
        }

        if (currentAccount.gameSlots == null || currentAccount.gameSlots.Count == 0)
        {
            Debug.Log("No game slots found. Creating default game slots.");
            for (int i = 0; i < 3; i++)
            {
                currentAccount.gameSlots.Add(new GameSlot(i));
            }
            SaveAccountData();
        }

        return currentAccount.gameSlots;
    }

    public GameData LoadGameData(int slotID)
    {
        currentSlotFilePath = GetSlotFilePath(slotID);
        if (File.Exists(currentSlotFilePath))
        {
            string jsonData = File.ReadAllText(currentSlotFilePath);
            selectedCharacter = JsonUtility.FromJson<Character>(jsonData);
            return new GameData
            {
                name = selectedCharacter.name,
                level = selectedCharacter.level,
                savedTime = selectedCharacter.savedTime
            };
        }
        else
        {
            Debug.LogError("No save file found in slot " + slotID);
            return null;
        }
    }

    public void SaveGameData(int slotID, GameData gameData)
    {
        if (currentAccount == null)
        {
            Debug.LogError("No account is logged in.");
            return;
        }

        currentSlotFilePath = GetSlotFilePath(slotID);
        GameSlot slot = currentAccount.gameSlots.Find(s => s.slotID == slotID);
        if (slot == null)
        {
            slot = new GameSlot(slotID);
            currentAccount.gameSlots.Add(slot);
        }

        if (slot.character == null)
        {
            slot.character = new Character(gameData.name, gameData.level, gameData.savedTime);
        }
        else
        {
            slot.character.name = gameData.name;
            slot.character.level = gameData.level;
            slot.character.savedTime = gameData.savedTime;
        }

        SaveSlotData(slot);
        SaveAccountData();
    }

    public Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public void SetSelectedCharacter(Character character)
    {
        selectedCharacter = character;
    }

    private void SaveSlotData(GameSlot slot)
    {
        string jsonData = JsonUtility.ToJson(slot.character, true);
        File.WriteAllText(currentSlotFilePath, jsonData);
    }

    private void SaveAccountData()
    {
        if (currentAccount != null)
        {
            string accountFilePath = GetAccountFilePath();
            string jsonData = JsonUtility.ToJson(currentAccount, true);
            File.WriteAllText(accountFilePath, jsonData);
        }
    }

    private void LoadAccountData()
    {
        string accountFilePath = GetAccountFilePath();
        if (File.Exists(accountFilePath))
        {
            string jsonData = File.ReadAllText(accountFilePath);
            currentAccount = JsonUtility.FromJson<Account>(jsonData);
        }
        else
        {
            CreateDefaultAccount();
        }
    }

    private void LoadLastPlayedSlot()
    {
        // 예: 마지막으로 저장된 슬롯을 불러오기 위해 데이터 추가 및 수정 필요
        GameSlot lastPlayedSlot = currentAccount?.gameSlots.Find(slot => slot.savedTime == DateTime.MaxValue);
        if (lastPlayedSlot != null)
        {
            LoadGameData(lastPlayedSlot.slotID);
        }
    }

    private void CreateDefaultAccount()
    {
        currentAccount = new Account("defaultUser", "defaultPassword");
        SaveAccountData();
    }

    private string GetAccountFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "accountData.json");
    }

    private string GetSlotFilePath(int slotID)
    {
        return Path.Combine(Application.persistentDataPath, $"slot_{slotID}_data.json");
    }
}

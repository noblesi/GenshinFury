using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private Account currentAccount;
    private Character selectedCharacter;

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
            return true;
        }
        return false;
    }

    public List<GameSlot> LoadAllGameSlots()
    {
        if (currentAccount == null)
        {
            Debug.LogError("No account is logged in.");
            return new List<GameSlot>();  // 빈 리스트 반환
        }

        // 게임 슬롯이 비어있는 경우 기본 슬롯 생성
        if (currentAccount.gameSlots == null || currentAccount.gameSlots.Count == 0)
        {
            Debug.Log("No game slots found. Creating default game slots.");
            for (int i = 0; i < 3; i++) // 기본적으로 3개의 슬롯 생성
            {
                currentAccount.gameSlots.Add(new GameSlot(i));
            }
            SaveAccountData();
        }

        return currentAccount.gameSlots;
    }

    public GameData LoadGameData(int slotID)
    {
        if (currentAccount == null)
        {
            Debug.LogError("No account is logged in.");
            return null;
        }

        GameSlot slot = currentAccount.gameSlots.Find(s => s.slotID == slotID);
        if (slot != null && slot.character != null)
        {
            selectedCharacter = slot.character;  // selectedCharacter 설정
            return new GameData
            {
                name = slot.character.characterName,
                level = slot.character.level,
                savedTime = slot.character.savedTime
            };
        }

        Debug.LogError("Character not found in the specified slot.");
        return null;
    }

    public void SaveGameData(int slotID, GameData gameData)
    {
        if (currentAccount == null)
        {
            Debug.LogError("No account is logged in.");
            return;
        }

        GameSlot slot = currentAccount.gameSlots.Find(s => s.slotID == slotID);
        if (slot == null)
        {
            slot = new GameSlot(slotID);
            currentAccount.gameSlots.Add(slot);
        }

        slot.character = new Character(gameData.name, gameData.level, gameData.savedTime);
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

    private void SaveAccountData()
    {
        if (currentAccount != null)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Account));
            using (FileStream stream = new FileStream(GetAccountFilePath(), FileMode.Create))
            {
                serializer.Serialize(stream, currentAccount);
            }
        }
    }

    private void LoadAccountData()
    {
        if (File.Exists(GetAccountFilePath()))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Account));
            using (FileStream stream = new FileStream(GetAccountFilePath(), FileMode.Open))
            {
                currentAccount = (Account)serializer.Deserialize(stream);
            }
        }
        else
        {
            Debug.LogError("No account data found. Creating default account.");
            CreateDefaultAccount();  // 기본 계정 생성
        }
    }

    private void CreateDefaultAccount()
    {
        currentAccount = new Account("defaultUser", "defaultPassword");
        SaveAccountData();
    }

    private string GetAccountFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "accountData.xml");
    }
}

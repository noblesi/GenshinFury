using System.Collections.Generic;
using UnityEngine;

public static class UserManager
{
    private static Dictionary<string, string> users = new Dictionary<string, string>();

    public static bool RegisterUser(string username, string password)
    {
        if (users.ContainsKey(username))
        {
            Debug.Log($"Registration failed: User '{username}' already exists.");
            return false; // �̹� �����ϴ� �����
        }
        users.Add(username, password);
        DataManager.Instance.CreateAccount(username, password); // ����� ������ ����
        Debug.Log($"User '{username}' registered successfully.");
        return true;
    }

    public static bool ValidateUser(string username, string password)
    {
        if (users.ContainsKey(username) && users[username] == password)
        {
            Debug.Log($"User '{username}' validated successfully.");
            return true;
        }
        bool isValid = DataManager.Instance.Login(username, password);
        if (isValid)
        {
            Debug.Log($"User '{username}' validated successfully through DataManager.");
        }
        else
        {
            Debug.Log($"User '{username}' validation failed.");
        }
        return isValid;
    }
}

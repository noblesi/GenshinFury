using System.Collections.Generic;

public static class UserManager
{
    private static Dictionary<string, string> users = new Dictionary<string, string>();

    public static bool RegisterUser(string username, string password)
    {
        if (users.ContainsKey(username))
        {
            return false;
        }
        users.Add(username, password);
        return true;
    }

    public static bool ValidateUser(string username, string password)
    {
        return users.ContainsKey(username) && users[username] == password;
    }
}
    

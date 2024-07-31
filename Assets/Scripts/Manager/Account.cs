using System;
using System.Collections.Generic;

[Serializable]
public class Account
{
    public string username;
    public string password;
    public List<GameSlot> gameSlots;

    public Account()
    {
        gameSlots = new List<GameSlot>();
    }

    public Account(string username, string password)
    {
        this.username = username;
        this.password = password;
        gameSlots = new List<GameSlot>();
    }
}

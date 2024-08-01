using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Account
{
    public string username;
    public string password;
    public List<GameSlot> gameSlots;

    public Account() { }

    public Account(string username, string password)
    {
        this.username = username;
        this.password = password;
        this.gameSlots = new List<GameSlot>();
    }
}

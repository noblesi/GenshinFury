using System;

[Serializable]
public class GameSlot 
{
    public bool isEmpty;
    public DateTime savedTime;
    public string playerName;
    public int playerLevel;

    public GameSlot()
    {
        isEmpty = true;
        savedTime = DateTime.MinValue;
        playerName = string.Empty;
        playerLevel = 1;
    }
}

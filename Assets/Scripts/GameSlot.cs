using System;

[Serializable]
public class GameSlot 
{
    public bool isEmpty = true;
    public string playerName = "";
    public int playerLevel = 0;
    public DateTime savedTime;

    public GameSlot()
    {
        isEmpty = true;
        savedTime = DateTime.MinValue;
        playerName = string.Empty;
        playerLevel = 1;
    }
}

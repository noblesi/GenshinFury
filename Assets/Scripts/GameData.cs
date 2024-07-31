using System;
using System.Xml.Serialization;

[Serializable]
public class GameData
{
    public string name;
    public int level;
    public DateTime savedTime;

    public GameData() { }

    public GameData(string name, int level, DateTime savedTime)
    {
        this.name = name;
        this.level = level;
        this.savedTime = savedTime;
    }
}

using System;
using System.Xml.Serialization;

[Serializable]
public class GameData
{
    public string name;
    public float stamina;
    public float currentSpeed;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public float rotationW;
    public int level;
    public int experience;
    public DateTime savedTime;

    public GameData(string playerName, int playerLevel, DateTime savedTime)
    {
        name = playerName;
        level = playerLevel;
        this.savedTime = savedTime;
    }
}

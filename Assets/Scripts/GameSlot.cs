using System;

[Serializable]
public class GameSlot
{
    public int slotID;
    public Character character;

    public GameSlot() { }

    public GameSlot(int slotID)
    {
        this.slotID = slotID;
        this.character = null;
    }

    public bool isEmpty => character == null;
    public DateTime savedTime => character?.savedTime ?? DateTime.MinValue;
    public string playerName => character?.name ?? "Unknown";
    public int playerLevel => character?.level ?? 0;
}

using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public string description;
    public bool isStackable;

    public Item(string name, Sprite icon, string description, bool isStackable = false)
    {
        this.itemName = name;
        this.icon = icon;
        this.description = description;
        this.isStackable = isStackable;
    }
}

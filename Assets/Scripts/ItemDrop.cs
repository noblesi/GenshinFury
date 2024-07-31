using System;
using UnityEngine;

[Serializable]
public class ItemDrop
{
    public Item item;
    public int minAmount;
    public int maxAmount;
    public float dropChance;
}

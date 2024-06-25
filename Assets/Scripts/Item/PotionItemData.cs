using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory/ItemData/Potion", order = 3)]
public class PotionItemData : CountableItemData
{
    public PotionType PotionType => potionType;
    public int EffectAmount => effectAmount;
    public float Duration => duration;

    [SerializeField] private PotionType potionType;
    [SerializeField] private int effectAmount; // Healing amount or buff amount
    [SerializeField] private float duration; // Duration for buff potions

    public override Item CreateItem()
    {
        return new PotionItem(this);
    }
}

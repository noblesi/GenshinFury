using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : CountableItem, IUsableItem
{
    public PotionItem(PotionItemData data, int amount = 1) : base(data, amount) { }

    public bool Use()
    {
        Amount--;

        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new PotionItem(CountableItemData as PotionItemData, amount);
    }
}

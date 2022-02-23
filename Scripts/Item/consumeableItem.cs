using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Pickupable Item/Consumeable Item")]
public class consumeableItem : Item
{
    public int maxStack = 1;
    public attributeData.attributes[] attribute;
    public int[] modifier;
    public bool[] isPercentageModifier;

    public Buff[] buffs;

    public override void setupItem()
    {

        equipable = false;
        consumable = true;
        stackLimit = maxStack;
        itemType = ItemType.Pickup;
        statModifiers = new Dictionary<attributeData.attributes, atrVal>();

        for (int i = 0; i < attribute.Length; i++)
        {
            statModifiers.Add(attribute[i], new atrVal(modifier[i],isPercentageModifier[i]));
        }
    }
}



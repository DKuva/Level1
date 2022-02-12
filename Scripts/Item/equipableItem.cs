using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/Pickupable Item/Equipable Item")]
public class equipableItem : Item
{
    public EupipmentType equipmentSlot;

    public int durability;
    public int durabilityMax;

    public playerAttributes.attributes[] attribute;
    public int[] modifier;
    public bool[] isPercentageModifier;


    public override void setupItem()
    {
        
        this.eupipmentType = equipmentSlot;
        currentDurability = durability;
        maxDurability = durabilityMax;
        equipable = true;
        consumable = false;
        stackLimit = 1;
        itemType = ItemType.Pickup;
        statModifiers = new Dictionary<playerAttributes.attributes, atrVal>();

        for (int i = 0; i < attribute.Length; i++)
        {
            statModifiers.Add(attribute[i], new atrVal(modifier[i],isPercentageModifier[i]));
        }
}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/Pickupable Item/Static Item")]
public class staticItem : Item
{
    public int maxStack = 1;
    public override void setupItem()
    {
        equipable = false;
        consumable = false;
        stackLimit = maxStack;
        itemType = ItemType.Pickup;
    }
}

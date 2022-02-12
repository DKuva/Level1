using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Permanent Item")]
public class permanentItem : Item
{

    public override void setupItem()
    {
        equipable = false;
        consumable = false;
        stackLimit = 999;
        itemType = ItemType.Permanent;
    }

}

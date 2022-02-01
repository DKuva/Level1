using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject
{
    public enum ItemType { Permanent, Pickup };
    public enum EupipmentType { None, Head, Torso, Hands, Legs };

    public bool consumable = false;
    
    public EupipmentType eupipmentType = EupipmentType.None;

    public ItemType itemType = ItemType.Pickup;
 

    public int[] statModifiers;

    public int stackLimit;
   
    public bool equipable = false;

    public string itemName = "item";
    public Sprite sprite;
    public Vector2 itemSize;

    public string description;


}

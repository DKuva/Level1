using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : ScriptableObject
{
    [HideInInspector]
    public enum ItemType { Permanent, Pickup };
    [HideInInspector]
    public enum EupipmentType { None, Head, Torso, Hands, Feet, LeftHand, RightHand, Ring};
    [HideInInspector]
    public EupipmentType eupipmentType = EupipmentType.None;

    [HideInInspector]
    public bool consumable = false;
    [HideInInspector]
    public bool equipable = false;
    [HideInInspector]
    public ItemType itemType = ItemType.Pickup;
    [HideInInspector]
    public Dictionary<attributeData.attributes,atrVal> statModifiers = new Dictionary<attributeData.attributes, atrVal>();
    [HideInInspector]
    public int stackLimit;
    [HideInInspector]
    public int currentDurability;
    [HideInInspector]
    public int maxDurability;
    
    public string itemName = "item";
    public Sprite sprite;
    public Vector2 itemSize;

    public string description;

    public virtual void setupItem()
    {
    }


}

public class atrVal
{
    public int value;
    public bool isPercentage;

    public atrVal(int v, bool p)
    {
        value = v;
        isPercentage = p;
    }
}


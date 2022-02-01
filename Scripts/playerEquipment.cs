using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEquipment : MonoBehaviour
{
    private GameObject _container;
    private equipSlot[] _slots;

    void Awake()
    {
        _container = GameObject.Find("Player/Camera/UI/Equipment/Panel");
        _slots = _container.GetComponentsInChildren<equipSlot>();

    }

    public void equipItem(Item item) //Will replace item in slot
    {
        Item.EupipmentType slotType = item.eupipmentType;
        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i].equipType == item.eupipmentType)
            {
                _slots[i].placeItemInSlot(item);

            }
        }
    }
    public bool tryToEquip(Item item) //Will return false if item in slot
    {
        if(item.equipable == false)
        {
            return false;
        }

        Item.EupipmentType slotType = item.eupipmentType;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].equipType == item.eupipmentType)
            {
                if(_slots[i].item == null)
                {
                    _slots[i].placeItemInSlot(item);
                    return true;
                }
            }
        }
        return false;
    }

    
}

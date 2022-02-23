using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerEquipment : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler
{
    

    private GameObject _container;
    private equipSlot[] _slots;
    public GameObject player;

    void Awake()
    {
        Debug.Log("playerEquipment");
        _container = transform.GetChild(1).gameObject;
        _slots = _container.GetComponentsInChildren<equipSlot>();

        playerMovement.walkedFiveUnits += updateDurability;
        playerMovementAndroid.walkedFiveUnits += updateDurability;
        playerInventory.equipItem += tryToEquip;
 
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void equipItem(Item item) //Will replace item in slot
    {
        Item.EupipmentType slotType = item.eupipmentType;
        for (int i = 0; i < _slots.Length; i++)
        {
            if(_slots[i].equipType == item.eupipmentType)
            {
                _slots[i].placeItemInSlot(item);
                break;
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

    public void updateDurability() //Removes 1 durability, and destroys item if 0
    {
        for(int i= 0; i < _slots.Length; i++)
        {
            if(_slots[i].item != null)
            {
                if(_slots[i].item.maxDurability < 999)
                {
                    _slots[i].item.currentDurability -= 1;
                    
                    if (_slots[i].item.currentDurability <= 0)
                    {
                        _slots[i].removeItemInSlot();
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }
}

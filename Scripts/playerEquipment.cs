using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerEquipment : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler
{

    private GameObject _container;
    private equipSlot[] _slots;
    public GameObject player;

    private detailsPanel _detailsPanel;
    private inAirItem _inAirItem;
    void Awake()
    {
        _container = transform.GetChild(1).gameObject;
        _slots = _container.GetComponentsInChildren<equipSlot>();
        _detailsPanel = player.GetComponent<playerUI>().inventory.GetComponent<playerInventory>().detailsPanel.GetComponent<detailsPanel>();
        _inAirItem = player.GetComponent<playerUI>().inAirItem.GetComponent<inAirItem>();

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
        if (_inAirItem.gameObject.activeSelf)
        {
            _inAirItem.dropItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       _detailsPanel.gameObject.SetActive(false);
    }
}

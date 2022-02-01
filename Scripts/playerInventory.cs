using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryType {Limited, Infinite};
public class playerInventory : MonoBehaviour
{


    public InventoryType inventoryType;
    
    private GameObject _container;
    private GameObject _panel;
    public GameObject slotPrefab;
    public GameObject slotBgPrefab;
    private GameObject _inAir;
   
    public int inventorySize = 16;

    private inventorySlot[] _slots;
    private playerEquipment _equipment;

    
    private void Awake()
    {
        _equipment = GameObject.Find("Player/Camera/UI/Equipment").GetComponent<playerEquipment>();
        _container = GameObject.Find("Player/Camera/UI/Inventory/Scroll View/Viewport/Content");
        _panel = GameObject.Find("Player/Camera/UI/Inventory/Scroll View/Viewport/Panel");

        _slots = _container.GetComponentsInChildren<inventorySlot>();
        inventorySize = _slots.Length;

        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < numberOfRows(); j++)
            {
                _slots[i + j * 8].slotIndex = new Vector2(i, j);
            }
        }
    }

    public bool addToInventory(Item item) //Try to add a single item to inventory
    {

        if(inventoryType == InventoryType.Limited)
        {
            if (item.itemType == Item.ItemType.Permanent)
            {
                Debug.Log("Permanent item picked up");
                return true;
            }

            if (!_equipment.tryToEquip(item))
            {
                for (int i = 0; i < inventorySize; i++)
                {
                    if (_slots[i].gameObject.activeSelf)
                    {
                        if (_slots[i].placeItemInSlot(item)) //Returns false if unable to place
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            while(true){
                if (item.itemType == Item.ItemType.Permanent)
                {
                    Debug.Log("Permanent item picked up");
                    return true;
                }

                if (!_equipment.tryToEquip(item))
                {
                    for (int i = 0; i < inventorySize; i++)
                    {
                        if (_slots[i].gameObject.activeSelf)
                        {
                            if (_slots[i].placeItemInSlot(item)) //Returns false if unable to place
                            {
                                return true;
                            }
                        }
                    }
                    addRow();
                }
                else
                {
                    return true;
                }
            }
            
        }
    }
    public void placeItemInSlot(Item item, Vector2 slot) //Assumes it's a valid slot
    {
        int s = (int)(slot.x + slot.y * 8);
        _slots[s].placeItemInSlot(item);
    }

    public GameObject getSlot(int x, int y)
    {
        int slotNumber = x + y * 8;
        return _slots[slotNumber].gameObject;

    }
    public bool slotHasItem(int x, int y)
    {
        int slotNumber = x + y * 8;
        if(_slots[slotNumber].item == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public int numberOfRows()
    {
        int i = Mathf.RoundToInt(inventorySize / 8);
        return i;
    }

    public void addRow()
    {
        for(int k = 0; k< _panel.transform.childCount; k++)
        {
            Destroy(_panel.transform.GetChild(k).gameObject);
        }
        for (int k = 0; k < _slots.Length+8; k++)
        {
            var v = Instantiate(slotBgPrefab);
            v.transform.SetParent(_panel.transform,false);
        }

        inventorySlot[] newSlots = new inventorySlot[_slots.Length + 8];
        //int i = 0;
        //foreach(inventorySlot oldSlot in _slots)
        //{
         //   newSlots[i] = oldSlot;
         //   _slots.CopyTo(newSlots,i);
        //    i++;
        //}
        _slots.CopyTo(newSlots,0);
        _container.GetComponent<UnityEngine.UI.GridLayoutGroup>().enabled = true;
        for(int j = 0; j < 8; j++)
        {
            var v = Instantiate(slotPrefab);
            v.transform.SetParent(_container.transform,false);
            newSlots[_slots.Length + j] = v.GetComponent<inventorySlot>();
        }
        _container.GetComponent<UnityEngine.UI.GridLayoutGroup>().enabled = false;

        _slots = newSlots;
        inventorySize = _slots.Length;

        for (int q = 0; q < 8; q++)
        {
            for (int p = 0; p < numberOfRows(); p++)
            {
                _slots[q + p * 8].slotIndex = new Vector2(q, p);
            }
        }
    }
}



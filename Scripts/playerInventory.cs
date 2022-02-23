using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public enum InventoryType {Limited, Infinite};
public class playerInventory : MonoBehaviour
{


    public InventoryType inventoryType;
    
    public GameObject slotPrefab;
    public GameObject slotBgPrefab;

    public GameObject player;

    public GameObject inventoryContent;
    public GameObject inventoryPanel;

    public GameObject detailsPanel;
    public GameObject inAirPanel;
    public int inventorySize = 16;

    private inventorySlot[] _slots;

    public delegate void pickup(Item item);
    public static event pickup itemAddedToInventory;

    public delegate bool equip(Item item);
    public static event equip equipItem;

    public delegate void message(string message);
    public static event message sendMessage;

    private UnityEngine.UI.ScrollRect[] _scrollRects;

    private void Awake()
    {
        _scrollRects = transform.GetComponentsInChildren<UnityEngine.UI.ScrollRect>();
        _slots = inventoryContent.GetComponentsInChildren<inventorySlot>();
        inventorySize = _slots.Length;

        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < numberOfRows(); j++)
            {
                _slots[i + j * 8].slotIndex = new Vector2(i, j);
                
            }
        }
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public bool addToInventory(Item item)
    {
        return addToInventory(item, 1, false);
    }

    public bool addToInventory(Item item,int stack, bool addNew)
    {
        if(inventoryType == InventoryType.Limited)
        {
           return addToInventoryLimited(item, stack, addNew);
        }
        else
        {
           return addToInvenotryInfinite(item, stack, addNew);
        }
    }
    private bool addToInventoryLimited(Item item, int stack, bool addNew) //Try to add a stack of items to inventory, trying to add over the stack limit returns false, addNew- ignore stacking items already in invenotry
    {
        if(item == null)
        {
            Debug.LogError("item is null");
            return false;
        }
        if(stack == 0)
        {
            return true;
        }
        item.setupItem(); //Creates the dictionary used for attribute modifiers

        if (item.itemType == Item.ItemType.Permanent)
        {
            itemAddedToInventory(item);
            Debug.Log("Permanent item picked up");
            return true;
        }

        if(equipItem != null)
        {
            if (equipItem(item))
            {
                return true;
            }
        }

        for (int i = 0; i < inventorySize; i++)
        {
            if (_slots[i].gameObject.activeSelf)
            {
                if (addNew)
                {
                    if ((_slots[i].item == null) || (_slots[i].item.itemName != item.itemName))
                    {
                        if (_slots[i].placeItemInSlot(item, stack)) //Returns false if unable to place
                        {
                            itemAddedToInventory(item);
                            return true;
                        }
                    }
                }
                else
                {
                    if (_slots[i].placeItemInSlot(item, stack)) //Returns false if unable to place
                    {
                        itemAddedToInventory(item);
                        return true;
                    }
                }
            }
        }
        if(sendMessage != null)
        {
            sendMessage("inventory Full");
        }
        return false;

    }

    private bool addToInvenotryInfinite(Item item, int stack, bool addNew)
    {
        while (true)
        {
            if (!addToInventoryLimited(item, stack, addNew))
            {
                addRow();
            }
            else
            {
                return true;
            }
        }
    }

    public void addToInventoryNoEquip(Item item) //Adds to inventory without calling an event or trying to equip - used for returning an equipped item to the inventory
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (_slots[i].gameObject.activeSelf)
            {
                if (_slots[i].placeItemInSlot(item)) //Returns false if unable to place
                {                 
                    break;
                }
            }
        }
    }

    public bool placeItemInSlot(Item item, int stack, Vector2 slot)
    {
        int s = (int)(slot.x + slot.y * 8);
        return _slots[s].placeItemInSlot(item,stack);
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

    public void enableScrolling(bool enable)
    {
        _scrollRects[0].enabled = enable;
        _scrollRects[1].enabled = enable;
    }
    public int numberOfRows()
    {
        int i = Mathf.RoundToInt(inventorySize / 8);
        return i;
    }

    public void addRow()
    {
        for(int k = 0; k< inventoryPanel.transform.childCount; k++)
        {
            Destroy(inventoryPanel.transform.GetChild(k).gameObject);
        }
        for (int k = 0; k < _slots.Length+8; k++)
        {
            var v = Instantiate(slotBgPrefab);
            v.transform.SetParent(inventoryPanel.transform,false);
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
        inventoryContent.GetComponent<UnityEngine.UI.GridLayoutGroup>().enabled = true;
        for(int j = 0; j < 8; j++)
        {
            var v = Instantiate(slotPrefab);
            v.transform.SetParent(inventoryContent.transform,false);
            newSlots[_slots.Length + j] = v.GetComponent<inventorySlot>();
        }
        inventoryContent.GetComponent<UnityEngine.UI.GridLayoutGroup>().enabled = false;

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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public enum InventoryType {Limited, Infinite};
public class playerInventory : MonoBehaviour
{


    public InventoryType inventoryType;
    
    private GameObject _container;
    private GameObject _panel;

    public GameObject slotPrefab;
    public GameObject slotBgPrefab;
    private GameObject _inAir;
    public GameObject player;

    public GameObject inventoryContent;
    public GameObject inventoryPanel;

    public GameObject detailsPanel;
    public int inventorySize = 16;

    private inventorySlot[] _slots;
    private playerEquipment _equipment;

    
    private void Awake()
    {
        
        _equipment = player.GetComponent<playerUI>().equipment.GetComponent<playerEquipment>();
        _container = inventoryContent;
        _panel = inventoryPanel;

        if(_container == null){Debug.LogError("Cant find InventoryContainer");}
        if (_panel == null){Debug.LogError("Cant find InventoryPanel");}

        _slots = _container.GetComponentsInChildren<inventorySlot>();
        inventorySize = _slots.Length;

        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < numberOfRows(); j++)
            {
                _slots[i + j * 8].slotIndex = new Vector2(i, j);
            }
        }
        
        gameObject.SetActive(false);
    }

    public bool addToInventory(Item item) //Try to add a single item to inventory
    {
        if(item == null)
        {
            Debug.LogError("item is null");
        }
        item.setupItem(); //Creates the dictionary used for attribute modifiers
       
        if (inventoryType == InventoryType.Limited)
        {
            if (item.itemType == Item.ItemType.Permanent)
            {
                sendAnalyticsPickup(item);
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
                            sendAnalyticsPickup(item);
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
                    sendAnalyticsPickup(item);
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
                                sendAnalyticsPickup(item);
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

    public bool addStack(Item item, int Stack) //Try to add a stack of items to inventory, doesent consider about max stack
    {
        if (item == null)
        {
            Debug.LogError("item is null");
        }
        item.setupItem();


        if (inventoryType == InventoryType.Limited)
        {

            for (int i = 0; i < inventorySize; i++)
            {
                if (_slots[i].gameObject.activeSelf)
                {
                    if(_slots[i].item == null)
                    {
                        if (_slots[i].placeItemInSlot(item)) //Returns false if unable to place
                        {
                            sendAnalyticsPickup(item);
                            _slots[i].changeStackNumber(Stack);
                            return true;
                        }
                    }else if(_slots[i].item.itemName != item.itemName)
                    {
                        if (_slots[i].placeItemInSlot(item)) //Returns false if unable to place
                        {
                            sendAnalyticsPickup(item);
                            _slots[i].changeStackNumber(Stack);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        else

        {
            while (true)//doesen't work
            {
                if (item.itemType == Item.ItemType.Permanent)
                {
                    sendAnalyticsPickup(item);
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
                                sendAnalyticsPickup(item);
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

    private void sendAnalyticsPickup(Item item)
    {
        AnalyticsResult res = Analytics.CustomEvent("Picked up item", new Dictionary<string, object> { { "Item name : ", item.itemName }, { "item type : ", item.itemType } });
        Debug.Log("AnalyticsResult -Picked up Item- " + res);
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



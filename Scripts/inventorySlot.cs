using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;


public class inventorySlot : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
{

    private inAirItem _inAirObject;


    private detailsPanel _detailsPanel;

    public GameObject inventory;

    private playerInventory _inventory;
    private playerEquipment _equipment;
    private attributeHandler _attributes;
    private UnityEngine.UI.Text _counter;
    private UnityEngine.UI.Image _image;

    [HideInInspector]
    public Item item;
    private int _stack = 0;

    [HideInInspector]
    public Vector2 slotIndex;

    private bool _touching;

    private bool _android;

    public delegate void consumeEvent(Item item);
    public static event consumeEvent consumedItem;

    public delegate void message(string message);
    public static event message sendMessage;

    private void Awake()
    {
       
        
        _counter = GetComponentInChildren<UnityEngine.UI.Text>();
        _image = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _image.gameObject.SetActive(false);

        _inventory = inventory.GetComponent<playerInventory>();

        _detailsPanel = _inventory.player.GetComponent<playerUI>().detailsPanel.GetComponent<detailsPanel>();
        _detailsPanel.gameObject.SetActive(false);

        _equipment = _inventory.player.GetComponent<playerUI>().equipment.GetComponent<playerEquipment>();
        _attributes = _inventory.player.GetComponent<attributeHandler>();

        _inAirObject = _inventory.player.GetComponent<playerUI>().inAirItem.GetComponent<inAirItem>();
        _android = _inventory.player.GetComponent<PlayerScript>().android;

        if (_attributes == null) { Debug.LogError("failed to find attributes"); }
        if (_equipment == null) { Debug.LogError("failed to find equipment"); }
        if (_inAirObject == null) { Debug.LogError("failed to find inAirObject"); }
        if (_detailsPanel == null) { Debug.LogError("failed to find detailsPanel"); }

        if (this._stack <= 1)
        {
            _counter.text = "";
        }
        else
        {
            _counter.text = _stack.ToString();
        }

    }
    private void Update()
    {
        if(Input.touchCount == 0)
        {
            _touching = false;
        }
        if (_touching)
        {           
            if(Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (item != null)
                {
                    //Place item in air on touch hold
                    _inAirObject.openPanel(this.item, this._stack, transform, this.slotIndex, "Inventory");
                    changeStackNumber(1);
                    removeItemInSlot();
                    _touching = false;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
     
        //Tell inAirObject where to drop an item
        if (_inAirObject.gameObject.activeSelf == true)
        {
            _inAirObject.setHoveringOver(gameObject);
        }
        else
        {
            //Display details panel for item - pc only
            if (!_android)
            {
                if (item != null)
                {
                    _detailsPanel.showSlot(this);
                }
                else
                {
                    _detailsPanel.gameObject.SetActive(false);
                }
            }
        }      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //InAirItem no longer on slot
        if (_inAirObject.gameObject.activeSelf == true)
        {
            _inAirObject.setHoveringOver(null);
        }
    }
    public void OnPointerDown(PointerEventData eventData) //On item slot pressed
    {
        _inventory.enableScrolling(item == null);

        if (_android)
        {
            pointerDownAndroid(eventData);
        }
        else
        {
            pointerDown(eventData);
        }
        
    }
    //slot behaviour on click
    private void pointerDown(PointerEventData eventData)
    {
        if (_inAirObject.gameObject.activeSelf == true)
        {
            //drop inAirItem into slot
            _inAirObject.dropItem();

        }
        else if (item != null)
        {         
            if ((eventData.button == PointerEventData.InputButton.Right) & (item.equipable))
            {
                //equip item
                if (!_equipment.tryToEquip(item))
                {
                    _equipment.equipItem(item);
                }
                removeItemInSlot();

            }
            else
            if ((eventData.button == PointerEventData.InputButton.Middle) & (item.consumable == true))
            {
                //consume item
                consumeItem();
                removeItemInSlot();
            }
            else
            {
                //Place item in air
                _inAirObject.openPanel(this.item, this._stack, transform, this.slotIndex, "Inventory");
                changeStackNumber(1);
                removeItemInSlot();

            }
        }
    }
    //slot behaviour on click - android
    private void pointerDownAndroid(PointerEventData eventData)
    {

        Touch touch = Input.GetTouch(0);

        if(touch.tapCount == 1)
        {
            if(item != null)
            {
                //display details panel on tap
                _touching = true;
                _detailsPanel.showSlotStatic(this);
            }

        }else if(touch.tapCount == 2)
        {
            if(item != null)
            {
                if ((item.equipable))
                {
                    //equip item
                    if (!_equipment.tryToEquip(item))
                    {
                        _equipment.equipItem(item);
                    }
                    removeItemInSlot();
                    _detailsPanel.gameObject.SetActive(false);
                }
                else
                if ((item.consumable == true))
                {
                    //consume item
                    consumeItem();
                    removeItemInSlot();
                }
            }
        }
    }
    public bool placeItemInSlot(Item item)
    {
        return placeItemInSlot(item, 1);
    }
    public bool placeItemInSlot(Item item, int stack) 
    {
        int nRows = _inventory.numberOfRows();
        if(item == null || stack == 0)
        {
            return true;
        }
        if(this.item == null)
        {
            if ((((int)slotIndex.x + item.itemSize.x) > 8) || (((int)slotIndex.y + item.itemSize.y) > nRows))
            {               
                return false;
            }

            for (int i = ((int)slotIndex.x); i < ((int)slotIndex.x + item.itemSize.x); i++)
            {
                for (int j = ((int)slotIndex.y); j < ((int)slotIndex.y + item.itemSize.y); j++)
                {
                    if (!((i == ((int)slotIndex.x)) && (j == ((int)slotIndex.y))))
                    {
                        if (_inventory.slotHasItem(i, j))
                        {
                            return false;
                        }
                        if (!_inventory.getSlot(i,j).activeSelf)
                        {
                            return false;
                        }
                    }
                }
            }
            for (int i = ((int)slotIndex.x); i < ((int)slotIndex.x + item.itemSize.x); i++)
            {
                for (int j = ((int)slotIndex.y); j < ((int)slotIndex.y + item.itemSize.y); j++)
                {
                    if (!((i == ((int)slotIndex.x)) && (j == ((int)slotIndex.y))))
                    {
                       _inventory.getSlot(i, j).SetActive(false);
                    }
                }
            }

            this.item = item;
            _image.gameObject.SetActive(true);
            _image.sprite = item.sprite;
            transform.localScale = new Vector3(this.item.itemSize.x, this.item.itemSize.y, 1);

        }
        else
        {
            if(this.item.itemName != item.itemName)
            {
                return false;
            }
        }

        if (item.stackLimit != -1) // infinite stack
        {
            if ((this._stack + stack) > item.stackLimit)
            {
                return false;
            }
        }
        this._stack += stack;
        if (this._stack <= 1)
        {
            _counter.text = "";
        }
        else
        {
            _counter.text = _stack.ToString();
        }
        return true;



    }
    public void removeStackInSlot()
    {
        if(item != null)
        {
            changeStackNumber(1);
            removeItemInSlot();
        }
    }
    public void removeItemInSlot()
    {
        this._stack -= 1;
        if (this._stack == 0)
        {
            for (int i = ((int)slotIndex.x); i < ((int)slotIndex.x + this.item.itemSize.x); i++)
            {
                for (int j = ((int)slotIndex.y); j < ((int)slotIndex.y + this.item.itemSize.y); j++)
                {
                    if (!((i == ((int)slotIndex.x)) && (j == ((int)slotIndex.y))))
                    {
                        if ((i < 8) && (j < _inventory.numberOfRows()))
                        {
                            _inventory.getSlot(i, j).SetActive(true);
                        }
                    }
                }
            }

            transform.localScale = new Vector3(1, 1, 1);
            this.item = null;
            _image.sprite = null;
            _image.gameObject.SetActive(false);
            _detailsPanel.gameObject.SetActive(false);
        }

        if (this._stack <= 1)
        {
            _counter.text = "";
        }
        else
        {
            _counter.text = _stack.ToString();
        }

    }
    public void consumeItem()
    {
        if(consumedItem != null)
        {
            consumedItem(item);
        }

        consumeableItem tempItem = (consumeableItem)item;

        _attributes.addStats(item.statModifiers);
        foreach(Buff b in tempItem.buffs)
        {
            _attributes.addBuff(b);
        }

        _detailsPanel.GetComponent<detailsPanel>().decStack();
        sendMessage("You consumed " + item.itemName);
    }

    public void changeStackNumber(int newStack)
    {
        this._stack = newStack;
        if (this._stack <= 1)
        {
            _counter.text = "";
        }
        else
        {
            _counter.text = _stack.ToString();
        }

    }
    public int getCurrentStack()
    {
        return this._stack;
    }
}

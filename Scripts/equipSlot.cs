using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;



public class equipSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    private inAirItem _inAirObject;
    public Item.EupipmentType equipType;

    private attributeHandler _attributes;
    private UnityEngine.UI.Image _image;
    public Item item;
    public GameObject inventory;
    private playerInventory _inventory;
    private detailsPanel _detailsPanel;
    private bool _touching;

    private bool _android;

    public delegate void pickup(Item item);
    public static event pickup equippedItem;
    public void Awake()
    {

        _image = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _image.gameObject.SetActive(false);
        _inventory = inventory.GetComponent<playerInventory>();
        
        _attributes = _inventory.player.GetComponent<attributeHandler>();
        _inAirObject = _inventory.player.GetComponent<playerUI>().inAirItem.GetComponent<inAirItem>();

        _android = _inventory.player.GetComponent<PlayerScript>().android;

        _detailsPanel = _inventory.player.GetComponent<playerUI>().detailsPanel.GetComponent<detailsPanel>();
        _detailsPanel.gameObject.SetActive(false);

        if (_attributes == null) { Debug.LogError("failed to find attributes"); }
        if (_inAirObject == null) { Debug.LogError("failed to find inAirObject"); }

    }

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            _touching = false;
        }
        if (_touching)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (item != null)
                {
                    //Place item in air on touch hold
                    _inAirObject.openPanel(this.item, transform, "Equipment");
                    removeItemInSlot();
                    _touching = false;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_android)
        {
            pointerDownAndroid(eventData);
        }
        else
        {
            pointerDown(eventData);
        }
    }
    public void OnPointerEnter(PointerEventData eventData) 
    {
        // Tell InAirItem where to drop
        if (_inAirObject.gameObject.activeSelf == true)
        {
            _inAirObject.setHoveringOver(gameObject);
        }
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

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_inAirObject.gameObject.activeSelf == true)
        {
            _inAirObject.setHoveringOver(null);
        }
        if (!_android)
        {
            _detailsPanel.gameObject.SetActive(false);
        }
  
    }
    private void pointerDown(PointerEventData eventData)
    {

        if (_inAirObject.gameObject.activeSelf == true)
        {
            _inAirObject.dropItem();

        }
        else
        {
            if (item != null)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    moveToInventory();
  
                }
                else
                {

                    _inAirObject.openPanel(this.item, transform, "Equipment");
                    removeItemInSlot();
                }
            }
        }
    }
    private void pointerDownAndroid(PointerEventData eventData)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.tapCount == 1)
        {
            if (item != null)
            {
                // set touching flag for hold detection in update
                _touching = true;
                _detailsPanel.showSlotStatic(this);
            }

        }
        else if (touch.tapCount == 2)
        {
            if (item != null)
            {
                //unequip item on double tap
                moveToInventory();
            }
        }
    }
    public bool placeItemInSlot(Item item)
    {
        if(equippedItem != null)
        {
            equippedItem(item);
        }

        if(this.equipType != item.eupipmentType)
        {
            return false;
        }

        if (this.item != null)
        {
            _inventory.addToInventoryNoEquip(this.item);
            removeItemInSlot();
        }
        
        _image.gameObject.SetActive(true);
        this.item = item;
        _image.sprite = item.sprite;
        addAttributes();
        return true;
    }

    public void removeItemInSlot()
    {
        removeAttributes();
        this.item = null;
        _image.sprite = null;
        _image.gameObject.SetActive(false);
        _detailsPanel.gameObject.SetActive(false);

    }

    public void moveToInventory()
    {
        _inventory.addToInventoryNoEquip(this.item);
        removeItemInSlot();
    }
    private void addAttributes()
    {
        _attributes.addStats(item.statModifiers);
    }
    private void removeAttributes()
    {
        _attributes.removeStats(item.statModifiers);
    }

}

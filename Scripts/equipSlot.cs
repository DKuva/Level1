using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;



public class equipSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    private inAirItem _inAirObject;
    public Item.EupipmentType equipType;

    private playerAttributes _attributes;
    private UnityEngine.UI.Image _image;
    public Item item;
    public GameObject inventory;
    private playerInventory _inventory;

    private bool _touching;
    private int _touchedTimer = 0;
    private int _holdTime = 2000;

    private bool _android;

    private void Awake()
    {
 
        _image = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _image.gameObject.SetActive(false);
        _inventory = inventory.GetComponent<playerInventory>();
        
        _attributes = _inventory.player.GetComponent<playerUI>().attributes.GetComponent<playerAttributes>();
        _inAirObject = _inventory.player.GetComponent<playerUI>().inAirItem.GetComponent<inAirItem>();

        _android = _inventory.player.GetComponent<PlayerScript>().android;

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
            _touchedTimer++;

            if (_touchedTimer >= _holdTime * Time.deltaTime)
            {
                if (item != null)
                {
                    //Place item in air on touch hold
                    _inAirObject.openPanel(this.item, transform, "Equipment");
                    removeItemInSlot();

                    _touchedTimer = 0;
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_inAirObject.gameObject.activeSelf == true)
        {
            _inAirObject.setHoveringOver(null);
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
        if (Input.touchCount == 1)
        {
            if (item != null)
            {
                // set touching flag for hold detection in update
                _touching = true;             
            }

        }
        else if (Input.touchCount == 2)
        {
            if (item != null)
            {
                //unequip item on double tap
                moveToInventory();
            }
        }
    }
    public void placeItemInSlot(Item item)
    {
        AnalyticsResult res = Analytics.CustomEvent("Equipped item", new Dictionary<string, object> { { "Item name : ", item.itemName }, { "item slot : ", item.eupipmentType } });
        Debug.Log("AnalyticsResult -Equipped Item- " + res);

        if (this.item == null)
        {
            _image.gameObject.SetActive(true);
            this.item = item;
            _image.sprite = item.sprite;
            addAttributes();

        }
        else
        {
            _inventory.addToInventoryNoEquip(this.item);
            removeItemInSlot();

            _image.gameObject.SetActive(true);
            this.item = item;
            _image.sprite = item.sprite;
            addAttributes();
        }
    }

    public void removeItemInSlot()
    {
        removeAttributes();
        this.item = null;
        _image.sprite = null;
        _image.gameObject.SetActive(false);

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

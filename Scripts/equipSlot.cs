using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class equipSlot : MonoBehaviour, IPointerDownHandler
{
    
    private GameObject _inAirObject;
    public Item.EupipmentType equipType;

    private playerAttributes _attributes;
    private UnityEngine.UI.Image _image;
    public Item item;
    private playerInventory _inventory;

    private void Awake()
    {
 
        _image = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _image.gameObject.SetActive(false);
        _attributes = GameObject.Find("Player/Camera/UI/Attributes").GetComponent<playerAttributes>();
        _inventory = GameObject.Find("Player/Camera/UI/Inventory").GetComponent<playerInventory>();
        _inAirObject = GameObject.Find("Player/Camera/UI/InAirItem");

    }
    public void OnPointerDown(PointerEventData eventData)
    {

        if (_inAirObject.activeSelf == true)
        {
            if (_inAirObject.GetComponent<inAirItem>().item.eupipmentType == this.equipType)
            {

                if (this.item == null)
                {
                    placeItemInSlot(_inAirObject.GetComponent<inAirItem>().item);
                    _inAirObject.GetComponent<inAirItem>().closePanel();
                }
                else
                {
                    _inventory.addToInventory(this.item);

                    placeItemInSlot(_inAirObject.GetComponent<inAirItem>().item);
                    _inAirObject.GetComponent<inAirItem>().closePanel();

                }
            }
            else
            {
                _inAirObject.GetComponent<inAirItem>().returnItem();
            }

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
                    _inAirObject.GetComponent<inAirItem>().openPanel(this.item, transform, "Equipment");
                    removeItemInSlot();
                }
            }
        }
    }

    public void placeItemInSlot(Item item)
    {
        
        if(this.item == null)
        {
            _image.gameObject.SetActive(true);
            this.item = item;
            _image.sprite = item.sprite;
            addAttributes();

        }
        else
        {
            _inventory.addToInventory(this.item);
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
        _inventory.addToInventory(this.item);
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

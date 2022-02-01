using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class inventorySlot : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
{

    public GameObject inAirObject;
    private GameObject _detailsPanel;

    private playerInventory _inventory;
    private playerEquipment _equipment;
    private UnityEngine.UI.Text _counter;
    private UnityEngine.UI.Image _image;

    public Item item;
    private int _stack = 0;


    public Vector2 slotIndex;

    private void Awake()
    {
        _counter = GetComponentInChildren<UnityEngine.UI.Text>();
        _image = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _image.gameObject.SetActive(false);

        _detailsPanel = GameObject.Find("Player/Camera/UI/DetailsPanel");
        _detailsPanel.gameObject.SetActive(false);

        _inventory = GameObject.Find("Player/Camera/UI/Inventory").GetComponent<playerInventory>();
        _equipment = GameObject.Find("Player/Camera/UI/Equipment").GetComponent<playerEquipment>();

        if (this._stack <= 1)
        {
            _counter.text = "";
        }
        else
        {
            _counter.text = _stack.ToString();
        }

    }

    public void OnPointerEnter(PointerEventData eventData) //Show detail panel if hovering
    {
        if(item != null)
        {
            _detailsPanel.gameObject.SetActive(true);
            var pos = new Vector3(transform.position.x + 1.5f, transform.position.y + 1.2f, transform.position.z);
            _detailsPanel.transform.position = pos;
            _detailsPanel.GetComponent<detailsPanel>().showItem(this.item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _detailsPanel.gameObject.SetActive(false);

    }
    public void OnPointerDown(PointerEventData eventData) //On item slot pressed
    {

        if (inAirObject.activeSelf == true)
        {
            if (placeItemInSlot(inAirObject.GetComponent<inAirItem>().item))
            {
                changeStackNumber(inAirObject.GetComponent<inAirItem>().stack);
                inAirObject.GetComponent<inAirItem>().closePanel();
            }
            else
            {
                inAirObject.GetComponent<inAirItem>().returnItem();
            }

        }
        else
        {

            if (item != null)
            {
                if ( (eventData.button == PointerEventData.InputButton.Right) & (item.equipable) )
                {
                    //equip item
                    _equipment.equipItem(item);
                    removeItemInSlot();

                }else
                if ((eventData.button == PointerEventData.InputButton.Middle) & (item.consumable == true))
                {
                    //consume item
                    consumeItem();
                    removeItemInSlot();
                }
                else
                {
                    inAirObject.GetComponent<inAirItem>().openPanel(this.item,this._stack, transform, this.slotIndex, "Inventory");
                    changeStackNumber(1);
                    removeItemInSlot();
                    
                }
            }
        }
    }

    public bool placeItemInSlot(Item item)
    {
        int nRows = _inventory.numberOfRows();

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
            this._stack += 1;
            _image.sprite = item.sprite;
            transform.localScale = new Vector3(this.item.itemSize.x, this.item.itemSize.y, 1);

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
        else
        {
            if(this.item.itemName != item.itemName)
            {
                return false;
            }
            if(item.stackLimit != -1)
            {
                if ((this._stack + 1) > item.stackLimit)
                {
                    return false;
                }
            }

            this._stack += 1;
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
            _counter.text = " ";
            _image.sprite = null;

            _image.gameObject.SetActive(false);
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
        GameObject.Find("Player/Camera/UI/Message").SetActive(true);
        GameObject.Find("Player/Camera/UI/Message/Text").GetComponent<UnityEngine.UI.Text>().text = ("You consumed " + this.item.itemName);

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
}

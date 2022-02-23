using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class inAirItem : MonoBehaviour 
{
    private Item _item;
    private Vector2 _startSlot;
    private string _cameFrom;
    private int _stack;
    private UnityEngine.UI.Text _stackText;

    private playerInventory _inventory;
    private playerAttributes _attributes;
    private playerEquipment _equipment;
    public GameObject lootObjectPrefab;
    public GameObject inventory;

    private GameObject _hoveringOver;
    private bool _android;
    private void Awake()
    {
        _inventory = inventory.GetComponent<playerInventory>();

        _attributes = _inventory.player.GetComponent<playerUI>().attributes.GetComponent<playerAttributes>();
        _equipment = _inventory.player.GetComponent<playerUI>().equipment.GetComponent<playerEquipment>();

        _stackText = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        _android = _inventory.player.GetComponent<PlayerScript>().android;
    }
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        transform.position = mousePos;

        if(!_inventory.gameObject.activeSelf &&
            !_equipment.gameObject.activeSelf &&
            !_attributes.gameObject.activeSelf)
        {
            returnItem();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            dropToMap();
        }

        if (_android)
        {
            if(Input.touchCount == 0)
            {
                dropItem();
            }
        }
    }

    public void closePanel()
    {

        GetComponent<UnityEngine.UI.Image>().sprite = null;
        _item = null;
        _cameFrom = null;
        _stack = 0;
        _stackText.text = "";
        gameObject.SetActive(false);

    }
    public void openPanel(Item item, int stacksize, Transform location, Vector2 slot, string cameFrom)
    {

        gameObject.SetActive(true);
        _item = item;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        transform.position = mousePos;
        _cameFrom = cameFrom;
        _startSlot = slot;
        _stack = stacksize;
        if(_stack <= 1)
        {
            _stackText.text = "";
        }
        else
        {
            _stackText.text = stacksize.ToString();
        }
        transform.localScale = new Vector3(_item.itemSize.x, _item.itemSize.y, 1);
        GetComponent<UnityEngine.UI.Image>().sprite = item.sprite;

    }
    public void openPanel(Item item, Transform location, string cameFrom)
    {
        openPanel(item, 1, location, _startSlot, cameFrom);
    }
    public void returnItem()
    {
        if(_cameFrom == "Inventory")
        {
            _inventory.placeItemInSlot(_item, _stack, _startSlot);          
        }
        else
        {
            _equipment.equipItem(_item);
        }
        closePanel();

    }

    public void setHoveringOver(GameObject obj)
    {

        if (_android)
        {
            Debug.Log(obj.name);
        }
        _hoveringOver = obj;
    }

    public void dropItem()
    {
        

        if(_hoveringOver == null)
        {

            returnItem();

        }else if(_hoveringOver.GetComponent<inventorySlot>() != null)
        {
 
            dropInInventory();

        }else if(_hoveringOver.GetComponent<playerOverlay>() != null)
        {
  
            dropToMap();

        }else if(_hoveringOver.GetComponent<equipSlot>() != null)
        {
 
            dropInEquipment();
        }

    }
    private void dropInInventory()
    {
        inventorySlot slot = _hoveringOver.GetComponent<inventorySlot>();

        if (slot.placeItemInSlot(_item,_stack)) //try to place
        {
            closePanel();
        }
        else if(tryToSwap(slot)) // no room for item
        {
            closePanel();
        }
        else
        {
            returnItem();
        }
    }
    private void dropInEquipment()
    {
        equipSlot slot = _hoveringOver.GetComponent<equipSlot>();

        if (!slot.placeItemInSlot(_item))
        {
            returnItem();
        }
        else
        {
            closePanel();
        }
 
    }
    private void dropToMap()
    {
        for (int j = 0; j < _stack; j++)
        {
            var i = Instantiate(lootObjectPrefab);
            i.GetComponent<lootableObject>().setItem(_item);
            i.GetComponent<lootableObject>().placeNearPlayer();
            i.transform.parent = null;
        }

        closePanel();
    }

    private bool tryToSwap(inventorySlot slot)
    {

        Item oldItem = slot.item;
        int oldStack = slot.getCurrentStack();
        inventorySlot oldSlot = _inventory.getSlot((int)_startSlot.x, (int)_startSlot.y).GetComponent<inventorySlot>();

        slot.removeStackInSlot();

        if (oldSlot.placeItemInSlot(oldItem, oldStack) && slot.placeItemInSlot(_item, _stack))
        {
            return true;
        }
        else
        {
            slot.removeStackInSlot();
            oldSlot.removeStackInSlot();
            slot.placeItemInSlot(oldItem, oldStack);
            return false;
        }
    }
}

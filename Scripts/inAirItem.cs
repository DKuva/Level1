using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class inAirItem : MonoBehaviour 
{
    public Item item;
    private Vector2 _startSlot;
    private string _cameFrom;
    [HideInInspector]
    public int stack;
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
        this.item = null;
        _cameFrom = null;
        stack = 0;
        _stackText.text = "";
        gameObject.SetActive(false);

    }
    public void openPanel(Item item, int stacksize, Transform location, Vector2 slot, string cameFrom)
    {

        gameObject.SetActive(true);
        this.item = item;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        transform.position = mousePos;
        _cameFrom = cameFrom;
        _startSlot = slot;
        stack = stacksize;
        _stackText.text = stacksize.ToString();
        transform.localScale = new Vector3(this.item.itemSize.x, this.item.itemSize.y, 1);
        GetComponent<UnityEngine.UI.Image>().sprite = item.sprite;

    }
    public void openPanel(Item item, Transform location, string cameFrom)
    {
        gameObject.SetActive(true);
        this.item = item;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        transform.position = mousePos;
        _cameFrom = cameFrom;
        stack = 1;
        _stackText.text = "";
        transform.localScale = new Vector3(this.item.itemSize.x, this.item.itemSize.y, 1);
        GetComponent<UnityEngine.UI.Image>().sprite = item.sprite;

    }
    public void returnItem()
    {
        if(_cameFrom == "Inventory")
        {
            _inventory.placeItemInSlot(this.item, _startSlot);
            _inventory.getSlot((int)_startSlot.x, (int)_startSlot.y).GetComponent<inventorySlot>().changeStackNumber(this.stack);
        }
        else
        {
            _equipment.equipItem(this.item);
        }
        closePanel();

    }

    public void setHoveringOver(GameObject obj)
    {
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

        if (slot.placeItemInSlot(item))
        {
            slot.changeStackNumber(stack);
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

        if (item.eupipmentType == slot.equipType)
        {

            if (slot.item == null)
            {

                slot.placeItemInSlot(item);
                closePanel();
            }
            else
            {

                _inventory.addToInventoryNoEquip(this.item);
                slot.placeItemInSlot(item);
                closePanel();

            }
        }
        else
        {
            returnItem();
        }
    }
    private void dropToMap()
    {
        for (int j = 0; j < stack; j++)
        {
            Vector2 v = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
            v.Normalize();

            Transform pos = _inventory.player.transform;

            float d = 2f;
            Vector3 newPos = new Vector3(pos.position.x + v.x * d, pos.position.y + v.y * d, pos.position.z);

            var i = Instantiate(lootObjectPrefab, newPos, Quaternion.identity);
            i.GetComponent<lootableObject>().setItem(item);
            i.transform.parent = null;
        }

        closePanel();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class inAirItem : MonoBehaviour 
{
    public Item item;
    private Vector2 _startSlot;
    private string _cameFrom;
    public int stack;
    private UnityEngine.UI.Text _stackText;

    private playerInventory _inventory;
    private playerAttributes _attributes;
    private playerEquipment _equipment;
    public GameObject lootObjectPrefab;

    private void Awake()
    {
        _inventory = GameObject.Find("Player/Camera/UI/Inventory").GetComponent<playerInventory>();
        _attributes = GameObject.Find("Player/Camera/UI/Attributes").GetComponent<playerAttributes>();
        _equipment = GameObject.Find("Player/Camera/UI/Equipment").GetComponent<playerEquipment>();
        _stackText = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();

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
            dropItem();
        }

    }

    public void closePanel()
    {
        GetComponent<inAirItem>().item = null;
        GetComponent<UnityEngine.UI.Image>().sprite = null;
        this.item = null;
        this._cameFrom = null;
        gameObject.SetActive(false);
        this.stack = 0;
        transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";
    }
    public void openPanel(Item item, int stacksize, Transform location, Vector2 slot, string cameFrom)
    {

        this.item = item;
        transform.position = location.position;
        this._cameFrom = cameFrom;
        this._startSlot = slot;
        this.stack = stacksize;
        transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = stacksize.ToString();

        gameObject.SetActive(true);
        GetComponent<inAirItem>().item = item;
        GetComponent<UnityEngine.UI.Image>().sprite = item.sprite;

    }
    public void openPanel(Item item, Transform location, string cameFrom)
    {
        this.item = item;
        transform.position = location.position;
        this._cameFrom = cameFrom;
        this.stack = 1;
        transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "";

        gameObject.SetActive(true);
        GetComponent<inAirItem>().item = item;
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
    public void dropItem()
    {
        for (int j = 0; j < this.stack; j++)
        {

            Vector2 v = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
            v.Normalize();

            Transform pos = GameObject.Find("Player").transform;

            float d = 2f;
            Vector3 newPos = new Vector3(pos.position.x + v.x * d, pos.position.y + v.y * d, pos.position.z);

            var i = Instantiate(lootObjectPrefab, newPos, Quaternion.identity);
            i.GetComponent<lootableObject>().setItem(this.item);
            i.transform.parent = null;

        }

        closePanel();
    }

}

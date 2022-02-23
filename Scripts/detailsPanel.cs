using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detailsPanel : MonoBehaviour
{
    private UnityEngine.UI.Image _image;
    private UnityEngine.UI.Text _name;
    private int _maxStack;
    private UnityEngine.UI.Text _description;
    private UnityEngine.UI.InputField _inputText;
    private GameObject _sPanel;
    private inventorySlot _currentSlot;
    public GameObject player;
    private void Awake()
    {
        _image = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        _name = transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();
        _description = transform.GetChild(2).GetComponent<UnityEngine.UI.Text>();
        _sPanel = transform.GetChild(3).gameObject;
        _inputText = _sPanel.GetComponentInChildren<UnityEngine.UI.InputField>();
        
        if (_sPanel == null)
        {
            Debug.LogError("cant find panel");
        }
    }

    //Display a inventory slot on the details Panel
    public void showSlot(inventorySlot slot)
    {
        gameObject.SetActive(true);
        Vector3 pos;
        _currentSlot = slot;

        if(slot.getCurrentStack() > 1)
        {
            _sPanel.gameObject.SetActive(true);

            _inputText.text = slot.getCurrentStack().ToString();
            pos = new Vector3(slot.transform.position.x + 1.6f, slot.transform.position.y + 1.6f, slot.transform.position.z);
            transform.position = pos;

        }
        else
        {
            _sPanel.gameObject.SetActive(false);
            pos = new Vector3(slot.transform.position.x + 1.6f, slot.transform.position.y + 1.0f, slot.transform.position.z);
            transform.position = pos;
        }

        showItem(slot.item);

    }
    public void showSlot(equipSlot slot)
    {
        gameObject.SetActive(true);
        Vector3 pos;

        _sPanel.gameObject.SetActive(false);
        pos = new Vector3(slot.transform.position.x - 1.6f, slot.transform.position.y - 1.0f, slot.transform.position.z);
        transform.position = pos;

        showItem(slot.item);
    }
    //Shows slot without changing its position - used in android build
    public void showSlotStatic(inventorySlot slot) 
    {
        gameObject.SetActive(true);

        _currentSlot = slot;

        if (slot.getCurrentStack() > 1)
        {
            _sPanel.gameObject.SetActive(true);
            _inputText.text = slot.getCurrentStack().ToString();
        }
        else
        {
            _sPanel.gameObject.SetActive(false);
        }

        showItem(slot.item);

    }

    public void showSlotStatic(equipSlot slot)
    {
        gameObject.SetActive(true);
       _sPanel.gameObject.SetActive(false);
        showItem(slot.item);
    }

    private void showItem(Item item)
    {
        _image.sprite = item.sprite;
        _name.text = item.itemName;
        _maxStack = item.stackLimit;
        //generate description from item
       
        if(item.equipable)
        {
            string desc;
            if (item.maxDurability < 999)
            {
                desc = "Equip- " + item.eupipmentType + "\n" + item.currentDurability + "/" + item.maxDurability;
            }
            else
            {
                desc = "Equip- " + item.eupipmentType;
            }

            foreach (KeyValuePair<attributeData.attributes, atrVal> stat in item.statModifiers)
            {
                if(stat.Value.value > 0)
                {
                    desc += "\n" + stat.Key.ToString() + " +" + stat.Value.value.ToString();
                }
                else
                {
                    desc += "\n" + stat.Key.ToString() + " -" + stat.Value.value.ToString();
                }
                if (stat.Value.isPercentage)
                {
                    desc += "%";
                }
            }
            _description.text = desc;
        }
        else
        {
            _description.text = item.description;
        }

    }

    public void decStack()
    {
        int num = int.Parse(_inputText.text);
        if(num > 0)
        {
            _inputText.text = (num - 1).ToString();
        }
       
    }
    public void incStack()
    {
        int num = int.Parse(_inputText.text);
        if (num < _currentSlot.getCurrentStack())
        {
            _inputText.text = (num + 1).ToString();
        }
    }
    public void splitStack()
    {
        int newStack = int.Parse(_inputText.text);
        player.GetComponent<playerUI>().inventory.GetComponent<playerInventory>().addToInventory(_currentSlot.item, (_currentSlot.getCurrentStack() - newStack),true); 
        if(newStack == 0)
        {
            _currentSlot.changeStackNumber(1);
            _currentSlot.removeItemInSlot();
        }
        else
        {
            _currentSlot.changeStackNumber(newStack);
        }
        
    }
}

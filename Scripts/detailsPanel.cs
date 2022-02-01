using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detailsPanel : MonoBehaviour
{
    private GameObject _image;
    private GameObject _name;
    private GameObject _description;

    private void Awake()
    {
        _image = GameObject.Find("Player/Camera/UI/DetailsPanel/Image");
        _name = GameObject.Find("Player/Camera/UI/DetailsPanel/Name");
        _description = GameObject.Find("Player/Camera/UI/DetailsPanel/Description");
    }
    public void showItem(Item item)
    {
        _image.GetComponent<UnityEngine.UI.Image>().sprite = item.sprite;
        _name.GetComponent<UnityEngine.UI.Text>().text = item.itemName;


        //generate description from item

        if(item.equipable)
        {
            string desc = "Equip- "+ item.eupipmentType + "\n" + 
                "Str: " +item.statModifiers[0].ToString() + "  " +
                "Dex: " +item.statModifiers[1].ToString() + "\n" +
                "End: " +item.statModifiers[2].ToString() + "  " +
                "Int: " +item.statModifiers[3].ToString();
            _description.GetComponent<UnityEngine.UI.Text>().text = desc;
        }
        else
        {
            _description.GetComponent<UnityEngine.UI.Text>().text = item.description;
        }

    }
}

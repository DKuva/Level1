using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerOverlay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public GameObject lootObjectPrefab;
    private inAirItem _inAirObject;

    public GameObject hpContainer;
    public GameObject buffContainer;

    public GameObject player;

    private detailsPanel _detailsPanel;
    private bool _android;

    private void Awake()
    {
        _detailsPanel = player.GetComponent<playerUI>().inventory.GetComponent<playerInventory>().detailsPanel.GetComponent<detailsPanel>();
        _inAirObject = player.GetComponent<playerUI>().inAirItem.GetComponent<inAirItem>();
        if (_inAirObject == null) { Debug.LogError("failed to find inAirObject"); }
        _android = player.GetComponent<PlayerScript>().android;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //if an item is inAir, drop it
        if (_inAirObject.gameObject.activeSelf)
        {
            _inAirObject.dropItem();
        }

        //on android, move player to tap location
        if (_android)
        {
            if(Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                player.GetComponent<playerMovementAndroid>().moveToPoint(new Vector2(pos.x, pos.y));

            }
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_inAirObject.gameObject.activeSelf == true)
        {
            _inAirObject.setHoveringOver(gameObject);
        }

        _detailsPanel.gameObject.SetActive(false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class gameAnalytics : MonoBehaviour
{

    private float _lastMessageTime;

    private int _walkMessageCounter = 0;
    private void Awake()
    {
        uiPanel.panelOpened += windowOpened;
        PlayerScript.hasColided += colided;
        PlayerScript.playerAwake += buildStartup;
        equipSlot.equippedItem += equipped;
        playerInventory.itemAddedToInventory += pickedUp;
        playerMovement.walkedTenUnits += walked;
        playerMovementAndroid.walkedTenUnits += walked;
        inventorySlot.consumedItem += consumedItem;

        _lastMessageTime = 0;
        
    }
    public void buildStartup()
    {
        AnalyticsResult res = Analytics.CustomEvent("build startup", new Dictionary<string, object> { { "Platform : ", Application.platform }, { "Time : ", DateTime.Now } });
        _lastMessageTime = Time.time;
        Debug.Log("AnalyticsResult -build startup- " + res);
    }
    public void windowOpened(string panelName)
    {
        AnalyticsResult res = Analytics.CustomEvent("Opened window", new Dictionary<string, object> { { "Window : ", panelName } });
        _lastMessageTime = Time.time;
        Debug.Log("AnalyticsResult -Opened window- " + res);
    }
    public void colided()
    {
        if (Time.time >= _lastMessageTime + 5)
        {
            AnalyticsResult res = Analytics.CustomEvent("colided");
            Debug.Log("AnalyticsResult -colided- " + res);
            _lastMessageTime = Time.time;
        }
    }

    public void walked()
    {
        if(_walkMessageCounter <= 5)
        {
            AnalyticsResult res = Analytics.CustomEvent("Walked 10 units");
            Debug.Log("AnalyticsResult -Walked 10 units- " + res);
            _walkMessageCounter++;
        }
    }

    public void equipped(Item item)
    {
        AnalyticsResult res = Analytics.CustomEvent("Equipped item", new Dictionary<string, object> { { "Item name : ", item.itemName }, { "item slot : ", item.eupipmentType } });
        Debug.Log("AnalyticsResult -Equipped Item- " + res);
        _lastMessageTime = Time.time;
    }

    public void pickedUp(Item item)
    {
        AnalyticsResult res = Analytics.CustomEvent("Picked up item", new Dictionary<string, object> { { "Item name : ", item.itemName }, { "item type : ", item.itemType } });
        Debug.Log("AnalyticsResult -Picked up Item- " + res);
        _lastMessageTime = Time.time;
    }

    public void consumedItem(Item item)
    {
        AnalyticsResult res = Analytics.CustomEvent("Consumed item", new Dictionary<string, object> { { "Item name : ", item.itemName }, { "item type : ", item.itemType } });
        Debug.Log("AnalyticsResult -Consumed Item- " + res);
        _lastMessageTime = Time.time;
    }
}

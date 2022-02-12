using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class playerUI : MonoBehaviour
{
    public GameObject playerCamera;

    public GameObject playerOverlay;
    public GameObject inventory;
    public GameObject equipment;
    public GameObject attributes;

    public GameObject inAirItem;
    public GameObject message;
	public GameObject detailsPanel;

	

	private bool _disableInput;
	private GameObject _panel;
	private enum _windowType {inventory, equipment, attributes};

    private void Awake()
    {
		_panel = playerOverlay.transform.GetChild(0).gameObject;
		_disableInput = false;
		if (playerOverlay == null) { Debug.LogError("playerUI cant find playerOverlay"); }
		if (playerCamera == null) { Debug.LogError("playerUI cant find playerCamera"); }
		if (inventory == null) { Debug.LogError("playerUI cant find inventory"); }
		if (equipment == null) { Debug.LogError("playerUI cant find equipment"); }
		if (attributes == null) { Debug.LogError("playerUI cant find attributes"); }

	}

    private void Update()
    {
		
        if (!_disableInput)
        {
			if (Input.GetKeyDown(KeyCode.I))
			{
				if (inventory.activeSelf)
				{
					closeInventory();
				}
				else
				{
					
					openInventory();
				}
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (equipment.activeSelf)
				{
					closeEquipScreen();
				}
				else
				{
					
					openEquipScreen();
				}
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				if (attributes.activeSelf)
				{
					closeAttributeScreen();
				}
				else
				{
					
					openAttributeScreen();
				}
			}
		}	
	}
	public void openInventory()
	{
		sendAnalytics(_windowType.inventory);
		inventory.SetActive(true);
		_panel.transform.GetChild(0).gameObject.SetActive(false);

	}
	public void openEquipScreen()
	{
		sendAnalytics(_windowType.equipment);
		equipment.SetActive(true);
		_panel.transform.GetChild(1).gameObject.SetActive(false);
	}

	public void closeInventory()
	{
		inventory.SetActive(false);
		_panel.transform.GetChild(0).gameObject.SetActive(true);
	}
	public void closeEquipScreen()
	{
		equipment.SetActive(false);
		_panel.transform.GetChild(1).gameObject.SetActive(true);
	}
	public void openAttributeScreen()
	{
		sendAnalytics(_windowType.attributes);
		attributes.SetActive(true);
		_panel.transform.GetChild(2).gameObject.SetActive(false);
	}

	public void closeAttributeScreen()
	{
		attributes.SetActive(false);
		_panel.transform.GetChild(2).gameObject.SetActive(true);
	}

	public void seDisableInput(bool val)
    {
		_disableInput = val;
    }
	private void sendAnalytics(_windowType type)
	{
		AnalyticsResult res = Analytics.CustomEvent("Opened window", new Dictionary<string, object> { { "Window : ", type} });
		Debug.Log("AnalyticsResult -Opened window- " + res);
	}
}

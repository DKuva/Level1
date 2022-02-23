using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
	public GameObject uiRoot;
	

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

		playerInventory.sendMessage += showMessage;
		inventorySlot.sendMessage += showMessage;

	}

    private void Update()
    {
		
        if (!_disableInput)
        {
			uiRoot.SetActive(true);
			if (Input.GetKeyDown(KeyCode.I))
			{
				inventory.GetComponent<uiPanel>().toggle();
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				equipment.GetComponent<uiPanel>().toggle();
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				attributes.GetComponent<uiPanel>().toggle();
			}
		}	
	}

	public void seDisableInput(bool val)
    {
		_disableInput = val;
		uiRoot.SetActive(false);
    }

	public void showMessage(string message)
    {
		GetComponent<playerUI>().message.SetActive(true);
		GetComponent<playerUI>().message.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = message;
	}
}

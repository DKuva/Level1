using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class PlayerScript : MonoBehaviour
{
    
	public float moveSpeed = 5f;
	public Rigidbody2D rigidBody;
	public Animator animator;

	public GameObject lootObjectPrefab;
	public Item itemToSpawn;
	private Vector2 _movement;
	private int _nextUpdate;
	private int _analyticsLastUpdate = 0;

	private int _lastStep;
	public bool android = true;

	[HideInInspector]
	public bool actionButton = false;

	private playerAttributes _attributes;
	private playerEquipment _equipment;
	private playerMovementAndroid _movementAndroid;
	private playerMovement _movementPc;
	public GameObject androidUI;
    private void Awake()
    {
		_nextUpdate = 0;
		_lastStep=0;

		_attributes = GetComponent<playerUI>().attributes.GetComponent<playerAttributes>();
		_equipment = GetComponent<playerUI>().equipment.GetComponent<playerEquipment>();
		_movementAndroid = GetComponent<playerMovementAndroid>();
		_movementPc = GetComponent<playerMovement>();

		if (!android)
        {
			_movementAndroid.enabled = false;
        }
        else
        {
			_movementPc.enabled = false;
        }

        if (android)
        {
			androidUI.SetActive(true);
        }
        else
        {
			androidUI.SetActive(false);
        }
	
		AnalyticsResult res = Analytics.CustomEvent("build startup", new Dictionary<string, object> { {"Platform : ", Application.platform}, { "Time : ", DateTime.Now} });
		Debug.Log("AnalyticsResult -build startup- " + res);

	}
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			spawnItem();
		}

		if (Time.time >= _nextUpdate)
		{
			_attributes.resolveBuffs();
			_nextUpdate += 1;
		}

        if (android)
        {
			if (_movementAndroid.getStepsWalked() != _lastStep)
			{
				_lastStep = _movementAndroid.getStepsWalked();

				if (_lastStep % 5 == 0)
				{
					_equipment.updateDurability();
				}
			}
		}
        else
        {
			if (_movementPc.getStepsWalked() != _lastStep)
			{
				_lastStep = _movementPc.getStepsWalked();

				if (_lastStep % 5 == 0)
				{
					_equipment.updateDurability();
				}
			}
		}
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		_movementAndroid.moveToPoint(new Vector2(transform.position.x, transform.position.y)); //stop moving at collision - android
    }
    public void OnTriggerStay2D(Collider2D collision)
    {

		if(_nextUpdate >= _analyticsLastUpdate + 5)
        {
			AnalyticsResult res = Analytics.CustomEvent("colided");
			Debug.Log("AnalyticsResult -colided- " + res);
			_analyticsLastUpdate = _nextUpdate;
		}
		
		var obj = collision.GetComponent<lootableObject>();
        if (obj.pickupOnPress)
        {
			if (actionButton)
			{

				if (GetComponent<playerUI>().inventory.GetComponent<playerInventory>().addToInventory(obj.item))
				{
					Destroy(collision.gameObject);
				}
				else
				{
					GetComponent<playerUI>().message.SetActive(true);
					GetComponent<playerUI>().message.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Inventory full";
					Debug.Log("Inventory full");
				}
			}
        }
        else
        {
			if (GetComponent<playerUI>().inventory.GetComponent<playerInventory>().addToInventory(obj.item))
			{
				Destroy(collision.gameObject);
			}
			else
			{
				GetComponent<playerUI>().message.SetActive(true);
				GetComponent<playerUI>().message.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Inventory full";
				Debug.Log("Inventory full");
			}
		}
        
	}

	public void pickUpItemProximity(GameObject lootObject)
    {
		var obj = lootObject.GetComponent<lootableObject>();
		if (obj.pickupOnPress)
		{
			if (actionButton)
			{
				if (GetComponent<playerUI>().inventory.GetComponent<playerInventory>().addToInventory(obj.item))
				{
					Destroy(lootObject.gameObject);
				}
				else
				{
					GetComponent<playerUI>().message.SetActive(true);
					GetComponent<playerUI>().message.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Inventory full";
					Debug.Log("Inventory full");
				}
			}
        }
        else
        {
			if (GetComponent<playerUI>().inventory.GetComponent<playerInventory>().addToInventory(obj.item))
			{
				Destroy(lootObject.gameObject);
			}
			else
			{
				GetComponent<playerUI>().message.SetActive(true);
				GetComponent<playerUI>().message.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Inventory full";
				Debug.Log("Inventory full");
			}
		}
	}


	public void spawnItem()
    {

		Vector2 v = new Vector2(UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1);
		v.Normalize();

		float d = 2f;
		Vector3 newPos = new Vector3(transform.position.x + v.x * d, transform.position.y + v.y * d, transform.position.z);

		var i = Instantiate(lootObjectPrefab, newPos, Quaternion.identity);
		i.GetComponent<lootableObject>().setItem(itemToSpawn);
		i.transform.parent = null;

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    
	public float moveSpeed = 5f;
	public Rigidbody2D rigidBody;
	public Animator animator;

	private GameObject _inventory;
	private GameObject _equipment;
	private GameObject _attributes;

	public GameObject lootObjectPrefab;
	public Item itemToSpawn;
	private Vector2 _movement;
	private void Awake()
    {
		_inventory = GameObject.Find("Player/Camera/UI/Inventory");
		_equipment = GameObject.Find("Player/Camera/UI/Equipment");
		_attributes = GameObject.Find("Player/Camera/UI/Attributes");


		_inventory.SetActive(false);
		_equipment.SetActive(false);
		_attributes.SetActive(false);
	}
    void Update()
    {
		
		_movement.x = Input.GetAxisRaw("Horizontal");
		_movement.y = Input.GetAxisRaw("Vertical");
		_movement.Normalize();


		animator.SetFloat("Vertical", _movement.y);
		animator.SetFloat("Horizontal", _movement.x);
		animator.SetBool("IsMoving", false);
	

		if (_movement.magnitude != 0)
        {
			animator.SetBool("IsMoving", true);
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
			if (_inventory.activeSelf)
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
			if (_equipment.activeSelf)
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
			if (_attributes.activeSelf)
			{
				closeAttributeScreen();
			}
			else
			{
				openAttributeScreen();
			}
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			spawnItem();
		}

	}
	
	void FixedUpdate(){

		rigidBody.MovePosition(rigidBody.position + _movement*moveSpeed*Time.fixedDeltaTime);
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
		var obj = collision.GetComponent<lootableObject>();
		if (_inventory.GetComponent<playerInventory>().addToInventory(obj.item))
		{
			Destroy(collision.gameObject);
		}
		else
		{
			GameObject.Find("Player/Camera/UI/Message").SetActive(true);
			GameObject.Find("Player/Camera/UI/Message/Text").GetComponent<UnityEngine.UI.Text>().text = "Inventory full.";
			Debug.Log("Inventory full");
		}
	}

	public void pickUpItemProximity(GameObject lootObject)
    {
		var obj = lootObject.GetComponent<lootableObject>();
		if (_inventory.GetComponent<playerInventory>().addToInventory(obj.item))
		{
			Destroy(lootObject.gameObject);
		}
		else
		{
			GameObject.Find("Player/Camera/UI/Message").SetActive(true);
			GameObject.Find("Player/Camera/UI/Message/Text").GetComponent<UnityEngine.UI.Text>().text = "Inventory full.";
			Debug.Log("Inventory full");
		}
	}

	public void openInventory()
    {
		_inventory.SetActive(true);
		GameObject.Find("Player/Camera/UI/PlayerOverlay/Panel/OpenInventory").gameObject.SetActive(false);

    }
	public void openEquipScreen()
    {
		_equipment.SetActive(true);
		GameObject.Find("Player/Camera/UI/PlayerOverlay/Panel/OpenEquipment").gameObject.SetActive(false);
	}

	public void closeInventory()
    {
		_inventory.SetActive(false);
		GameObject.Find("Player/Camera/UI/PlayerOverlay/Panel/OpenInventory").gameObject.SetActive(true);
	}
	public void closeEquipScreen()
    {
		_equipment.SetActive(false);
		GameObject.Find("Player/Camera/UI/PlayerOverlay/Panel/OpenEquipment").gameObject.SetActive(true);
	}
	public void openAttributeScreen()
	{
		_attributes.SetActive(true);
		GameObject.Find("Player/Camera/UI/PlayerOverlay/Panel/OpenAttributes").gameObject.SetActive(false);
	}

	public void closeAttributeScreen()
	{
		_attributes.SetActive(false);
		GameObject.Find("Player/Camera/UI/PlayerOverlay/Panel/OpenAttributes").gameObject.SetActive(true);
	}

	public void spawnItem()
    {

		Vector2 v = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
		v.Normalize();

		float d = 2f;
		Vector3 newPos = new Vector3(transform.position.x + v.x * d, transform.position.y + v.y * d, transform.position.z);

		var i = Instantiate(lootObjectPrefab, newPos, Quaternion.identity);
		i.GetComponent<lootableObject>().setItem(itemToSpawn);
		i.transform.parent = null;

	}
}

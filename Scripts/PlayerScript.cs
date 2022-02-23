using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class PlayerScript : MonoBehaviour
{
    
	public float moveSpeed = 5f;
	public Rigidbody2D rigidBody;
	public Animator animator;

	public GameObject lootObjectPrefab;
	public Item itemToSpawn;
	private Vector2 _movement;
	private int _nextUpdate;

	public bool android = true;

	[HideInInspector]
	public bool actionButton = false;

	private playerMovementAndroid _movementAndroid;
	private playerMovement _movementPc;
	public GameObject androidUI;

	public delegate void tickOne();
	public static event tickOne oneSecondTick;

	public delegate void colide();
	public static event colide hasColided;

	public static event tickOne playerAwake;
	private void Awake()
    {
		Debug.Log("playerscript");
		_nextUpdate = 0;
	
		_movementAndroid = GetComponent<playerMovementAndroid>();
		_movementPc = GetComponent<playerMovement>();

		if (!android)
        {
			_movementAndroid.enabled = false;
			_movementPc.enabled = true;
			androidUI.SetActive(false);
		}
        else
        {
			_movementAndroid.enabled = true;
			_movementPc.enabled = false;
			androidUI.SetActive(true);
		}
	}

    private void Start()
    {
		if (playerAwake != null)
		{
			playerAwake();
		}
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			spawnItem();
		}

		if (Time.time >= _nextUpdate)
		{
			if(oneSecondTick != null)
            {
				oneSecondTick();
			}
			_nextUpdate += 1;
		}		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		_movementAndroid.moveToPoint(new Vector2(transform.position.x, transform.position.y)); //stop moving at collision - android
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
		
		if (hasColided != null)
        {
			hasColided();
		}
		var obj = collision.GetComponent<lootableObject>();
		obj.lootItem();        
	}

	public void pickUpItemProximity(GameObject lootObject)
    {
		var obj = lootObject.GetComponent<lootableObject>();
		obj.lootItem();
	}

	public void spawnItem()
    {

		var i = Instantiate(lootObjectPrefab);
		i.GetComponent<lootableObject>().setItem(itemToSpawn);
		i.GetComponent<lootableObject>().placeNearPlayer();
		i.transform.parent = null;

	}
}

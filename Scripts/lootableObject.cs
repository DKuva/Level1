using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class lootableObject : MonoBehaviour
{
    public enum InteractMethod { proximity, collision, overlap};
    public InteractMethod interactMethod = InteractMethod.collision;

    private Transform _playerLocation;
    public int detectionDistance = 10;
    public Item dropItem;
    [HideInInspector]
    private Item _item;
    private float _sway;
    private int _swayDirection;
    public bool pickupOnPress;
    public GameObject player;

    void Awake()
    {

        if(player == null)
        {
            player = FindObjectOfType<PlayerScript>().gameObject;
        }
        _playerLocation = player.transform;
        

        if (interactMethod == InteractMethod.collision)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        _swayDirection = 1;
        _sway = Random.value*0.005f;

        if(dropItem != null)
        {
            setNewItem(dropItem);
        }
    }
    public void placeNearPlayer()
    {
        Vector2 v = new Vector2(UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1);
        v.Normalize();

        float d = 2f;
        Vector3 newPos = new Vector3(_playerLocation.position.x + v.x * d, _playerLocation.position.y + v.y * d, _playerLocation.position.z);
        transform.position = newPos;
    }
    public void setItem(Item item)
    {
        _item = item;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
    public void setNewItem(Item item)
    {
        _item = Object.Instantiate(item);
        GetComponent<SpriteRenderer>().sprite = _item.sprite;
    }
    void FixedUpdate()
    {
        sway();

        if (interactMethod == InteractMethod.proximity)
        {
            if (Vector2.Distance(transform.position, _playerLocation.position) < detectionDistance)
            {
                player.GetComponent<PlayerScript>().pickUpItemProximity(gameObject);
                
            }
        }else if(interactMethod == InteractMethod.overlap)
        {
            if (Vector2.Distance(transform.position, _playerLocation.position) < (detectionDistance+3))
            {
                Collider2D[] collider = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), detectionDistance);
                foreach (Collider2D c in collider)
                {
                    if (c.name == "Player Android")
                    {
                        player.GetComponent<PlayerScript>().pickUpItemProximity(gameObject);
                    }
                }
            }
        }
    }
    private void sway()
    {
        float maxsway = 0.005f;

        if (Mathf.Abs(_sway) >= maxsway)
        {
            this._swayDirection *= -1;
        }
        _sway += 0.0001f * this._swayDirection;
        transform.position = new Vector3(transform.position.x, transform.position.y + _sway, transform.position.z);
    }
    public void lootItem()
    {
        if (pickupOnPress)
        {
            if (player.GetComponent<PlayerScript>().actionButton)
            {
                if (player.GetComponent<playerUI>().inventory.GetComponent<playerInventory>().addToInventory(_item))
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (player.GetComponent<playerUI>().inventory.GetComponent<playerInventory>().addToInventory(_item))
            {
                Destroy(gameObject);
            }
        }
    }
}

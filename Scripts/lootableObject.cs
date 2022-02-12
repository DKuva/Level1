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
    public Item item;
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
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        _swayDirection = 1;
        _sway = Random.value*0.005f;

        if(this.item != null)
        {
            this.item = Object.Instantiate(this.dropItem);
            GetComponent<SpriteRenderer>().sprite = item.sprite;
        }
    }

    public void setItem(Item item)
    {
        this.item = item;
        GetComponent<SpriteRenderer>().sprite = item.sprite;

    }
    void FixedUpdate()
    {

        float maxsway = 0.005f;

        if (Mathf.Abs(_sway) >= maxsway)
        {
           this._swayDirection *= -1;
        }
        _sway += 0.0001f*this._swayDirection;       
        transform.position = new Vector3(transform.position.x, transform.position.y + _sway, transform.position.z);

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
                    if (c.name == "Player")
                    {
                        player.GetComponent<PlayerScript>().pickUpItemProximity(gameObject);
                    }
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootableObject : MonoBehaviour
{
    public enum InteractMethod { proximity, collision, overlap, casting };
    public InteractMethod interactMethod = InteractMethod.collision;

    private Transform _playerLocation;
    public int detectionDistance = 10;

    public Item item;
    private float _sway;
    private int _swayDirection;

    void Start()
    {
        _playerLocation = (Transform)GameObject.Find("Player").GetComponent(typeof(Transform));
        GetComponent<SpriteRenderer>().sprite = item.sprite;

        if (interactMethod == InteractMethod.collision)
        {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        _swayDirection = 1;
        _sway = 0.0f;

    }

    public void setItem(Item item)
    {
        this.item = item;
        GetComponent<SpriteRenderer>().sprite = item.sprite;

    }
    void Update()
    {

        float maxsway = 0.001f;

        if (Mathf.Abs(_sway) >= maxsway)
        {
           this._swayDirection *= -1;
        }
        _sway += 0.00001f*this._swayDirection;       
        transform.position = new Vector3(transform.position.x, transform.position.y + _sway, transform.position.z);

        if (interactMethod == InteractMethod.proximity)
        {
            if (Vector2.Distance(transform.position, _playerLocation.position) < detectionDistance)
            {
                GameObject.Find("Player").GetComponent<PlayerScript>().pickUpItemProximity(gameObject);
                
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
                        GameObject.Find("Player").GetComponent<PlayerScript>().pickUpItemProximity(gameObject);
                    }
                }
            }
        }
    }

}

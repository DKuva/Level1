using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookTrigger : MonoBehaviour
{

    public GameObject lookTarget;
    public GameObject player;

    public int detectionDistance = 5;
    public int lookTime= 5;

    public delegate void lookEvent(GameObject target, int lookTIme);
    public static event lookEvent look;
    private void Awake()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerScript>().gameObject;
        }
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < detectionDistance)
        {
            if(look != null)
            {
                look(lookTarget, lookTime);
            }
            Destroy(gameObject);

        }
    }
}

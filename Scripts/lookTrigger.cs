using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookTrigger : MonoBehaviour
{

    public GameObject lookTarget;
    public GameObject player;

    public int detectionDistance = 5;
    public int lookTime= 5;
    private cameraMovement _cam;
    private void Awake()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerScript>().gameObject;
        }
        _cam = player.GetComponent<cameraMovement>();
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < detectionDistance)
        {

            _cam.lookAtTarget(lookTarget,lookTime);
            Destroy(gameObject);

        }
    }
}

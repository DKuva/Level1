using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZoom : MonoBehaviour
{
    public int minZoom = 2;
    public int maxZoom = 8;

    private float _touchDistance;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch tA = Input.GetTouch(0);
            Touch tB = Input.GetTouch(1);
            float dis = Vector2.Distance(tA.position, tB.position);
            if (_touchDistance == 0)
            {
                _touchDistance = dis;
            }
            else
            {
                zoom(_touchDistance - dis);
            }
        }
        else
        {
            _touchDistance = 0;
        }
    }
    public void zoom(float zoom)
    {
        Camera.main.orthographicSize += zoom * 0.001f;
        if (Camera.main.orthographicSize < minZoom)
        {
            Camera.main.orthographicSize = minZoom;

        }
        if (Camera.main.orthographicSize > maxZoom)
        {
            Camera.main.orthographicSize = maxZoom;

        }
    }
}

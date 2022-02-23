using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMessage : MonoBehaviour
{

    private int timer = 0;

    void Update()
    {
        int closeAfter = 200;
        timer++;
        if(timer > closeAfter)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }
}

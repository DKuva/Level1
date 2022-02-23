using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiPanel : MonoBehaviour
{
    public GameObject openButton;
    public GameObject closeButton;

    public delegate void opened(string panelName);
    public static event opened panelOpened;
    private void Awake()
    {
       openButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { open(); } );
       closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { close(); });
       

    }
    public void  open()
    {
        //sendAnalytics(_windowType.inventory);
        gameObject.SetActive(true);
        openButton.SetActive(false);
        panelOpened(gameObject.name);
    }

    public void close()
    {
        gameObject.SetActive(false);
        openButton.SetActive(true);
    }

    public void toggle()
    {
        if (gameObject.activeSelf)
        {
            close();
        }
        else
        {

            open();
        }
    }
}

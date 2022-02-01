using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerOverlay : MonoBehaviour, IPointerDownHandler
{
    public GameObject lootObjectPrefab;
    private GameObject _inAirObject;

    private void Awake()
    {

        _inAirObject = GameObject.Find("Player/Camera/UI/InAirItem");
        if (_inAirObject == null)
        {
            Debug.LogWarning("failed to find inAirItem");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Take inair item, and spawn it near player
        if (_inAirObject.gameObject.activeSelf)
        {
            for(int j = 0;j < _inAirObject.GetComponent<inAirItem>().stack; j++)
            {
                Vector2 v = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
                v.Normalize();

                Transform pos = GameObject.Find("Player").transform;

                float d = 2f;
                Vector3 newPos = new Vector3(pos.position.x + v.x * d, pos.position.y + v.y * d, pos.position.z);

                var i = Instantiate(lootObjectPrefab, newPos, Quaternion.identity);
                i.GetComponent<lootableObject>().setItem(_inAirObject.GetComponent<inAirItem>().item);
                i.transform.parent = null;
            }

            _inAirObject.GetComponent<inAirItem>().item = null;
            _inAirObject.gameObject.SetActive(false);

        }
    }
  

}

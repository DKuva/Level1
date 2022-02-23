using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private GameObject _camera;
    private Vector2 _cameraPosition;
    private bool _looking = false;
    private int _lookTimer = 0;
    private Vector2 _lookTarget;
    private int _lookTime;
    private int _lastSec;

    private bool _android;
    private playerMovement _movementPc;
    private playerMovementAndroid _movementAndroid;
    private playerUI _ui;
    private playerOverlay _playerOverlay;

    private void Awake()
    {
        _camera = GetComponent<playerUI>().playerCamera;
        _cameraPosition = new Vector2(0, 0);
        _camera.transform.position = _cameraPosition;
        _lookTimer = 0;
        _lookTime = 0;
        _lookTarget = new Vector2(transform.position.x, transform.position.y);

        _android = GetComponent<PlayerScript>().android;
        _movementPc = GetComponent<playerMovement>();
        _movementAndroid = GetComponent<playerMovementAndroid>();
        _ui = GetComponent<playerUI>();
        _playerOverlay = _ui.playerOverlay.GetComponent<playerOverlay>();

        lookTrigger.look += lookAtTarget;
    }

    private void Update()
    {
        
       
        if (_looking)
        {
            if((int)Time.time > _lastSec)
            {
                _lastSec = (int)Time.time;
                _lookTimer += 1;
            }
            

            if (_lookTimer >= _lookTime)
            {
                _looking = false;
                _lookTimer = 0;
                _lookTarget = new Vector2(transform.position.x, transform.position.y);
                if (_android)
                {
                    _movementAndroid.setDontMove(false);
                }
                else
                {
                    _movementPc.setDontMove(false);
                }
                _ui.seDisableInput(false);
                _playerOverlay.gameObject.SetActive(true);
            }

            hoverToTarget(_lookTarget, Time.deltaTime*2f);
        }
        else
        {
            _lookTarget = new Vector2(transform.position.x, transform.position.y);
            hoverToTarget(_lookTarget, Time.deltaTime*2f);
        }
        
    }
    private void setCameraWorldPos(Vector2 pos)
    {
        Vector3 newPos = new Vector3(pos.x, pos.y, -10);
        _cameraPosition = pos;
        _camera.transform.position = newPos;
        
    }
    //Move the camera twords a target by the speedFactor
    private void hoverToTarget(Vector2 target, float speedFactor)
    {

        //float hoverSpeed = Vector2.Distance(_cameraPosition, target) * speedFactor ; //Alternative hoverspeed dependent on distance from target
        float hoverSpeed = 1f *speedFactor;
        setCameraWorldPos(_cameraPosition + (target- _cameraPosition) * hoverSpeed);

    }

    //Sets a target to move the camera to
    public void lookAtTarget(GameObject obj, int lookTime)
    {
        if (_android)
        {
            _movementAndroid.setDontMove(true);
        }
        else
        {
            _movementPc.setDontMove(true);
        }

        _ui.seDisableInput(true);
        _playerOverlay.gameObject.SetActive(false);
        Vector2 target = new Vector2(obj.transform.position.x, obj.transform.position.y);
        _lookTarget = target;
        _lookTime = lookTime;
        _looking = true;
    }

}

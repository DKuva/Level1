using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class playerMovement : MonoBehaviour
{
	public int moveSpeed = 5;
	private Vector2 _movement;
	private Rigidbody2D _rigidBody;
	private Animator _animator;

	private float _stepDistance;
	private int _stepsWalked;
	private bool _dontMove = false;
	[HideInInspector]
	public bool isMoving =  false;
	private bool _sentFiveTimes = false;
	private int _sendTimes = 0;
	private bool _sent = false;
	private PlayerScript _playerScript;

	private void Awake()
	{
		_stepDistance = 0;
		_stepsWalked = 0;
		_rigidBody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_playerScript = GetComponent<PlayerScript>(); 
	}

	void Update()
	{


		if (!_dontMove)
		{
			_movement.x = Input.GetAxisRaw("Horizontal");
			_movement.y = Input.GetAxisRaw("Vertical");
			_movement.Normalize();
		}
		else
		{
			_movement.x = 0;
			_movement.y = 0;
		}

		if (_movement.magnitude != 0)
		{
			isMoving = true;
        }
        else
        {
			isMoving = false;
        }
		_animator.SetFloat("Vertical", _movement.y);
		_animator.SetFloat("Horizontal", _movement.x);
		_animator.SetBool("IsMoving", isMoving);

        if (Input.GetKey(KeyCode.F))
        {
			_playerScript.actionButton = true;
        }
        else
        {
			_playerScript.actionButton = false;
        }
	}

	void FixedUpdate()
	{
		_rigidBody.MovePosition(_rigidBody.position + _movement * moveSpeed * Time.fixedDeltaTime);

		float dist = _movement.magnitude * moveSpeed * Time.fixedDeltaTime;
		_stepDistance += dist;
		if (_stepDistance >= 1)
		{
			_stepDistance = 0;
			_stepsWalked += 1;
			_sent = false;
		}
        if (!_sentFiveTimes)
        {
			if (_stepsWalked % 10 == 0 && _stepsWalked != 0 && !_sent)
			{
				sendAnalytics();
				_sent = true;
				_sendTimes++;
				if(_sendTimes >= 5)
                {
					_sentFiveTimes = true;
                }
			}

		}

	}
	private void sendAnalytics()
    {
		AnalyticsResult res = Analytics.CustomEvent("Walked 10 units");
		Debug.Log("AnalyticsResult -Walked 10 units- " + res);
	}
	public int getStepsWalked()
    {
		return _stepsWalked;
    }
	public void setDontMove(bool walk)
    {
		this._dontMove = walk;
    }
	public Vector2 getMovement()
    {
		return _movement;
    }

}

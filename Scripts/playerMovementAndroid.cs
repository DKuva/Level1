using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class playerMovementAndroid : MonoBehaviour
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


	public Joystick _joystic;

	private Vector3 _moveTarget = new Vector2();
	private bool _activeMoveT = false;

	public delegate void walkEvent();
	public static event walkEvent walkedTenUnits;
	public static event walkEvent walkedFiveUnits;

	private void Awake()
	{
		_stepDistance = 0;
		_stepsWalked = 0;
		_moveTarget = transform.position;
		_rigidBody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (!_dontMove)
		{

			if(!_activeMoveT)
            {
				_movement.x = _joystic.Horizontal;
				_movement.y = _joystic.Vertical;
				_movement.Normalize();
			}
            else
            {
				Vector3 t = _moveTarget-transform.position;
				Vector2 t2 = new Vector2(t.x, t.y);
				t2.Normalize();
				_movement.x = t2.x;
				_movement.y = t2.y;
				if (Vector3.Distance(transform.position, _moveTarget) <= 0.1f)
				{
					_activeMoveT = false;
				}
			}

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


			if (_stepsWalked % 5 == 0)
			{
				walkedFiveUnits();
			}
			if (_stepsWalked % 10 == 0)
			{
				walkedTenUnits();
			}
		}

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

	public void actionButtonDown()
    {
		GetComponent<PlayerScript>().actionButton = true;
    }
	public void actionButtonUp()
    {
		GetComponent<PlayerScript>().actionButton = false;
	}
	public void moveToPoint(Vector2 position)
    {
		_moveTarget = new Vector3(position.x, position.y, transform.position.z);
		_activeMoveT = true;
    }

}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AStarPathfinding;
using UnityEngine.Events;

public class GhostController : MonoBehaviour 
{    
    public enum MovementDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    
    
    public Vector2 ReturnLocation = new Vector2(0, 0);

	private Animator _animator;
    public PacmanController PacMan;
	public Vector2 moveToLocation;
	public float speed;
    public float currSpeed;

    public bool mDebugPath;
    public Color mPathColor;

    [HideInInspector]
    public bool mIsChasing = true;
    [HideInInspector]
    public MovementDirection mDirection = MovementDirection.Left;


    int mWaveTimerIx = 0;
    float mCurrentTimer = 0.0f;

    [Header("FSM Trigger Names")]
    public string mChase;
    public string mScatter;
    public string mRestart;
    public string mToHouse;
    public string mFright;

    [Header("Animator Bool Names")]
    public string mIsGhost;
    public string mIsDead;


	void Start()
	{
		_animator = GetComponent<Animator>();
		GameDirector.Instance.GameStateChanged.AddListener(GameStateChanged);
        GameDirector.Instance.Ghosts.Add(this);
        mIsChasing = true;
        currSpeed = speed;
    }

    void Update()
    {
        if(mDebugPath)
        {
            Debug.DrawLine(transform.position, moveToLocation, mPathColor);
        }
        
        
        if (GameDirector.Instance.state == GameDirector.States.enState_PacmanInvincible)
        {
            return;
        }

        if (mWaveTimerIx >= GameDirector.Instance.mWaveTimers.Length && !mIsChasing)
        {
            mIsChasing = true;
            return;
        }
        mCurrentTimer += Time.deltaTime;
        if(mCurrentTimer >= GameDirector.Instance.mWaveTimers[mWaveTimerIx])
        {
            mWaveTimerIx++;
            mCurrentTimer = 0.0f;
            mIsChasing = !mIsChasing;
        }
    }

    private bool pathCompleted = false;
    public UnityEvent pathCompletedEvent = new UnityEvent();


    public void Move()
	{
		List<Vector3> _path = new List<Vector3>();
		PathFinding.Instance.generatePath(transform.position, moveToLocation, _path);

        if (_path.Count >= 2)
		{
            pathCompleted = false;
			iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(_path[1].x, _path[1].y, 0),
													"speed", currSpeed,
													"easetype", "linear",
													"oncomplete", "moveComplete"));
            if(_path[1].x > transform.position.x)
            {
                mDirection = MovementDirection.Right;
            }
            else if(_path[1].y > transform.position.y)
            {
                mDirection = MovementDirection.Up;
            }
            else if(_path[1].x < transform.position.x)
            {
                mDirection = MovementDirection.Left;
            }
            else if(_path[1].y < transform.position.y)
            {
                mDirection = MovementDirection.Down;
            }
		}
		else
		{
            if (pathCompleted == false)
            {
                pathCompleted = true;
                pathCompletedEvent.Invoke();
            }

            StartCoroutine(WaitToMove());
		}
	}

	IEnumerator WaitToMove()
	{
		yield return new WaitForSeconds(1);
		Move();
	}

	public void moveComplete()
	{
        StopAllCoroutines();
        Move();
	}

	public void Kill()
	{
		_animator.SetBool(mIsDead, true);
	}

	public void GameStateChanged(GameDirector.States _state)
	{
		switch (_state)
		{
			case GameDirector.States.enState_Normal:
				_animator.SetBool(mIsGhost, false);
				break;

			case GameDirector.States.enState_PacmanInvincible:
				_animator.SetBool(mIsGhost, true);
				break;

			case GameDirector.States.enState_GameOver:
				break;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class EnemyPatrolMovement : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _speed;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [Min(0)]
    [SerializeField] private float _destinationDistanceTreshold;
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;
    [SerializeField] private Transform _visual;

    private Rigidbody2D _rigidBody;
    private Vector3 _currentDestination;
    private Animator animator;

    private EnemyAnimationState _animationState;

    public bool isFacingLeft = true;

    private enum EnemyAnimationState
    {
        Moving,
        Standing,
        Attacking
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _currentDestination = _pointB.position;
        animator = GetComponent<Animator>();
        FlipToFaceDirection();
        IdleStand(RandomTime());
    }

    private void Update()
    {
        if (_animationState == EnemyAnimationState.Moving)
        {
            if (CheckIfDestinationReached())
            {
                IdleStand(RandomTime());
            }
            else
            {
                MoveTowardsDestination();
            }
        }
    }

    private float RandomTime() => Random.Range(_minWaitTime, _maxWaitTime);

    private bool CheckIfDestinationReached()
    {
        float xDistanceToDestination =
            Mathf.Abs(transform.position.x - _currentDestination.x);

        if (xDistanceToDestination <= _destinationDistanceTreshold)
        {
            return true;
        }

        return false;
    }

    private void ChangeDestination()
    {
        if (_currentDestination == _pointA.position)
        {
            _currentDestination = _pointB.position;
            FlipToFaceDirection();
            return;
        }

        _currentDestination = _pointA.position;
        FlipToFaceDirection();
    }

    private void MoveTowardsDestination()
    {
        int xMovementDirection = -1;

        if (transform.position.x < _currentDestination.x)
        {
            xMovementDirection = 1;
        }

        _rigidBody.velocity =
            new Vector2(_speed * xMovementDirection, 0);
    }

    private void IdleStand(float time)
    {
        _animationState = EnemyAnimationState.Standing;
        RunAnimation();
        _rigidBody.velocity = Vector2.zero;

        StartCoroutine(Wait(time));
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResumeMovementAfterWait();
    }

    private void ResumeMovementAfterWait()
    {
        _animationState = EnemyAnimationState.Moving;
        ChangeDestination();
        RunAnimation();
        MoveTowardsDestination();
    }

    private void RunAnimation()
    {
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsWalking", false);

        switch (_animationState)
        {
            case EnemyAnimationState.Attacking:
                animator.SetBool("IsAttacking", true);
                break;
            case EnemyAnimationState.Standing:
                animator.SetBool("IsIdle", true);

                break;
            case EnemyAnimationState.Moving:
                animator.SetBool("IsWalking", true);
                break;
        }
    }

    private void FlipToFaceDirection()
    {
        Vector3 visualScale = _visual.localScale;
        float xScaleSign = Mathf.Sign(visualScale.x);

        float xDifference = _currentDestination.x - transform.position.x;
        float xDifferenceSign = Mathf.Sign(xDifference);

        if (xDifferenceSign == xScaleSign) return;

        visualScale.x *= -1;
        _visual.localScale = visualScale;

        if (visualScale.x < 0)
        {
            isFacingLeft = false;
        }
        else
        {
            isFacingLeft = true;
        }
    }
}

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
    [SerializeField] private bool StablePosition = true;
    [Min(0)]
    [SerializeField] private float _destinationDistanceTreshold;

    private Rigidbody2D _rigidBody;
    private Vector3 _currentDestination;
    private Animator animator;

    private EnemyAnimationState _animationState;

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
        IdleStand(RandomTime());
    }

    private float RandomTime() => Random.Range(5, 7);

    private void Update()
    {
        if (_animationState == EnemyAnimationState.Moving)
        {
            if(CheckIfDestinationReached())
            {
                IdleStand(RandomTime());
            }
            else
            {
                MoveTowardsDestination();
            }
        }
    }

    private bool CheckIfDestinationReached()
    {
        float xDistanceToDestination =
            Mathf.Abs(transform.position.x - _currentDestination.x);

        if (xDistanceToDestination <= _destinationDistanceTreshold)
        {
            Debug.Log("Destination reached.");
            return true;
        }

        Debug.Log("Destination not reached  Distance = " + xDistanceToDestination);
        return false;
    }

    private void ChangeDestination()
    {
        if (_currentDestination == _pointA.position)
        {
            _currentDestination = _pointB.position;
            return;
        }

        _currentDestination = _pointA.position;
        _animationState = EnemyAnimationState.Moving;
        FlipVisual();
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
        StartCoroutine(Wait(time));
    }

    IEnumerator Wait(float seconds)
    {
        _animationState = EnemyAnimationState.Standing;
        RunAnimation();
        _rigidBody.velocity = Vector2.zero;

        Debug.Log("Standing for " + seconds);

        yield return new WaitForSeconds(seconds);

        Debug.Log("Finished standing");

        _animationState = EnemyAnimationState.Moving;
        ChangeDestination();
        FlipVisual();
        RunAnimation();
        MoveTowardsDestination();
    }

    private void RunAnimation()
    {
        switch (_animationState)
        {
            case EnemyAnimationState.Attacking:
                animator.SetTrigger("Attack");
                break;
            case EnemyAnimationState.Standing:
                animator.SetTrigger("Idle");
                break;
            case EnemyAnimationState.Moving:
                animator.SetTrigger("Walk");
                break;
        }
    }

    private void FlipVisual()
    {
        float x = transform.GetChild(0).transform.localScale.x;
        float y = transform.GetChild(0).transform.localScale.y;
        float z = transform.GetChild(0).transform.localScale.z;

        x *= -1;
        transform.GetChild(0).transform.localScale.Set(x, y, z);
    }
}

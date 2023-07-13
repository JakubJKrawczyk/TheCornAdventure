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
    private Timer timer;

    private bool _isMoving = false;
    private bool _isStanding = false;
    private bool _isAttacking = false;

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
        SetTimer(500);
    }

    private double RandomTime() => Random.Range(1500, 3000);

    private void Update()
    {
        if (CheckIfDestinationReached() == true && !timer.Enabled)
        {
            SetTimer(RandomTime());
        }

        if (!timer.Enabled) MoveTowardsDestination();
    }

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
            return;
        }

        _currentDestination = _pointA.position;
        _animationState = EnemyAnimationState.Moving;
        ChangeEnemyFacing();
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

    private void SetTimer(double time)
    {
        timer = new Timer(time);
        timer.Elapsed += OnTimeElapsed;
        timer.AutoReset = true;
        timer.Enabled = true;
        _animationState = EnemyAnimationState.Standing;
        RunAnimation();
    }

    private void OnTimeElapsed(object source, ElapsedEventArgs args)
    {
        ChangeDestination();
        timer.Enabled = false;
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

    private void ChangeEnemyFacing()
    {
        float x = transform.GetChild(0).transform.localScale.x;
        float y = transform.GetChild(0).transform.localScale.y;
        float z = transform.GetChild(0).transform.localScale.z;

        x *= -1;
        transform.GetChild(0).transform.localScale.Set(x, y, z);
    }
}

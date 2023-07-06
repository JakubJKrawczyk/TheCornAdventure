using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    [Min(0)]
    [SerializeField] private float _speed;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;

    [Min(0)]
    [SerializeField] private float _destinationDistanceTreshold;

    private Rigidbody2D _rigidBody;
    private Vector3 _currentDestination;

    protected override void Start()
    {
        base.Start();
        _rigidBody = GetComponent<Rigidbody2D>();
        _currentDestination = _pointB.position;
        StartMovingTowardsDestination();
    }

    private void Update()
    {
        if (CheckIfDestinationReached() == true)
        {
            ChangeDestination();
            StartMovingTowardsDestination();
        }
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
    }

    private void StartMovingTowardsDestination()
    {
        int xMovementDirection = -1;

        if(transform.position.x < _currentDestination.x)
        {
            xMovementDirection = 1;
        }
        
        _rigidBody.velocity = new Vector2(_speed * xMovementDirection, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class EnemyPatrolMovementBat : MonoBehaviour
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

    public LayerMask groundLayer;

    public float attackRange = 10f;
    public LayerMask groundLayerMask;


    public float flyForce = 500f;
    [Range(0, 5)] public float BaseGroundDetectionDistance = 0.5f;

    private float groundDetectionDistance = 0;

    public GameObject player;

    // Maximum distance to detect the player
    [Range(0.5f, 10)] public float playerDetectionDistance = 2f;

    private Rigidbody2D _rigidBody;
    private Vector3 _currentDestination;

    private bool isFacingLeft = false;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _currentDestination = _pointB.position;
        FlipToFaceDirection();
        IdleStand(RandomTime());

        groundDetectionDistance = BaseGroundDetectionDistance;
        StartCoroutine(UpdateDetectionDistance());
    }

    private void FixedUpdate()
    {
        // Cast a ray downwards to detect the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDetectionDistance, groundLayer);

        // Log the raycast information for debugging
        Debug.DrawRay(transform.position, Vector2.down * groundDetectionDistance, Color.red, 0.1f);

        // If the raycast hits the ground, fly up
        if (hit.collider != null)
        {
            _rigidBody.AddForce(Vector2.up * flyForce * Time.fixedDeltaTime);
        }
    }

    private bool isMoving = false;          // Track whether the enemy is currently moving or idle
    private bool isAtDestination = false;

    private void Update()
    {
        if (DetectPlayer() && !isAtDestination)
        {
            // Stop the enemy from moving
            _rigidBody.velocity = Vector2.zero;

            // Get the direction to the player
            Vector2 directionToPlayer = player.transform.position - transform.position;

            // Move the bat towards the player
            MoveTowardsPlayer(directionToPlayer);

            // You can add additional behavior here, like attacking the player or any other action

            // You may want to set isMoving to false here if you don't want the enemy to continue moving
            // after it detects the player

            return;
        }

        if (isAtDestination)
        {
            IdleStand(RandomTime());
        }
        else if (CheckIfDestinationReached() && isMoving)
        {
            isAtDestination = true;
            MoveTowardsDestination(); // Call MoveTowardsDestination() one last time to set the rigidbody velocity to 0.
        }
        else if (isMoving)
        {
            MoveTowardsDestination();
        }
    }

    private bool DetectPlayer()
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float distance = directionToPlayer.magnitude;

        // Check if the player is in front of the enemy
        float dotProduct = Vector2.Dot(transform.right, directionToPlayer.normalized);

        if (!isFacingLeft)
        {
            dotProduct = Vector2.Dot(transform.right * -1, directionToPlayer.normalized);
        }

        bool playerInFront = dotProduct > 0f;

        // Check if the player is within detection range and in front of the enemy
        if (distance < playerDetectionDistance && playerInFront)
        {
            // Check if there are no obstacles (ground) between the enemy and the player
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, directionToPlayer.normalized, hits, distance, groundLayerMask);
            return hitCount == 0;
        }

        return false;
    }

    private void MoveTowardsPlayer(Vector2 directionToPlayer)
    {
        // Normalize the direction vector to get a unit vector in the same direction
        Vector2 normalizedDirection = directionToPlayer.normalized;

        // Set the velocity of the bat to move towards the player
        _rigidBody.velocity = new Vector2(_speed * normalizedDirection.x, _speed * normalizedDirection.y*2);

        // Flip the bat's visual based on the movement direction
        Vector3 visualScale = _visual.localScale;
        if (_rigidBody.velocity.x > 0)
        {
            visualScale.x = Mathf.Abs(visualScale.x);
        }
        else if (_rigidBody.velocity.x < 0)
        {
            visualScale.x = -Mathf.Abs(visualScale.x);
        }
        _visual.localScale = visualScale;
    }



    private float RandomTime() => Random.Range(_minWaitTime, _maxWaitTime);

    private float RandomHeight() => Random.Range(BaseGroundDetectionDistance - 0.15f, BaseGroundDetectionDistance + 0.15f);

    private IEnumerator UpdateDetectionDistance()
    {
        while (true)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Update the ground detection distance randomly
            groundDetectionDistance = RandomHeight();
            if (groundDetectionDistance < 0.5f)
            {
                groundDetectionDistance = 0.5f;
            }
            Debug.Log(groundDetectionDistance);
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
            FlipToFaceDirection();
            return;
        }

        _currentDestination = _pointA.position;
        FlipToFaceDirection();
    }

    private void MoveTowardsDestination()
    {
        // Only move if not currently idle
        if (!isMoving)
        {
            return;
        }

        int xMovementDirection = -1;

        if (transform.position.x < _currentDestination.x)
        {
            xMovementDirection = 1;
        }

        _rigidBody.velocity = new Vector2(_speed * xMovementDirection, _rigidBody.velocity.y);
    }

    private void IdleStand(float time)
    {
        _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
        isMoving = false;           // Set the isMoving flag to false when idle
        isAtDestination = false;    // Reset the isAtDestination flag when idle

        StartCoroutine(Wait(time));
    }


    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResumeMovementAfterWait();
    }


    private void ResumeMovementAfterWait()
    {
        isMoving = true;            // Set the isMoving flag to true when resuming movement
        ChangeDestination();
        MoveTowardsDestination();
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


    private void OnDrawGizmos() // visualize radius
    {
        DrawOverlapCircle(transform.position, playerDetectionDistance);
    }

    private void DrawOverlapCircle(Vector2 center, float radius)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, radius);
    }

}

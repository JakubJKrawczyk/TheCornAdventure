using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] public GameObject Projectile;
    [SerializeField] public Transform ProjectilePos;
    [SerializeField] public LayerMask groundLayerMask;

    private GameObject player;
    private bool isAttacking;
    

    private EnemyPatrolMovement EnemyPatrolMovement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isAttacking = false;
        EnemyPatrolMovement = GetComponent<EnemyPatrolMovement>();
    }


    void Update()
    {
        // Calculate direction towards the player
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float distance = directionToPlayer.magnitude;

        // Check if the player is in front of the enemy
        float dotProduct = Vector2.Dot(transform.right, directionToPlayer.normalized);

        if (!EnemyPatrolMovement.isFacingLeft)
        {
            dotProduct = Vector2.Dot(transform.right * -1, directionToPlayer.normalized);
        }

        bool playerInFront = dotProduct > 0f;

        // Check if the player is within attack range and in front of the enemy
        if (distance < 20f && playerInFront)
        {
            // Check if there are no obstacles (ground) between the enemy and the player
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, directionToPlayer.normalized, hits, distance, groundLayerMask);
            if (hitCount == 0)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f); 
        ThrowProjectile();
        yield return new WaitForSeconds(1f); 
        isAttacking = false;

    }
    public void SetProjectilePos(Transform pos)
    {
        ProjectilePos = pos;
    }
    void ThrowProjectile()
    {
        Instantiate(Projectile, ProjectilePos.position, Quaternion.identity);
    }

}

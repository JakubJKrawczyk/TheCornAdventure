using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject Projectile;
    public Transform ProjectilePos;
    public Animator animator;

    private GameObject player;
    private bool isAttacking;

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isAttacking = false;
    }

    
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);

        if (distance < 10)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("IsAttacking");

        yield return new WaitForSeconds(1f); 

        ThrowProjectile();

        yield return new WaitForSeconds(0.5f); 

        animator.ResetTrigger("IsAttacking");
        isAttacking = false;
    }

    void ThrowProjectile()
    {
        Instantiate(Projectile, ProjectilePos.position, Quaternion.identity);
    }
}

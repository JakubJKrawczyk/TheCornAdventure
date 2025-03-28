using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 10; // Amount of damage the enemy deals

    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_enemyHealth.IsAlive == false)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.gameObject.TryGetComponent<HealthController>(out var playerHealth))
            {
                playerHealth.RemoveHealth(_damageAmount);
            }
        }
    }
}
    


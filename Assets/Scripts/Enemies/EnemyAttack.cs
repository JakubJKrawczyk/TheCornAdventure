using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage the enemy deals

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            
            if (collision.gameObject.TryGetComponent<HealthController>(out var playerHealth))
            {
                playerHealth.RemoveHealth(damageAmount);
            }
        }
    }
}
    


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage the enemy deals

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthController playerHealth = collision.gameObject.GetComponent<HealthController>();

            if (playerHealth != null)
            {
                playerHealth.RemoveHealth(damageAmount);
                Debug.Log("Damage dealt to the player");
            }
        }
    }
}
    


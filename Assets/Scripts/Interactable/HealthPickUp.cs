using Assets.Scripts.Base_Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUp
{
    [Range(-1, 5)] public int HealingAmount = 1;
    
    

    private bool isPickedUp = false;

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthController playerHealth = collision.gameObject.GetComponent<HealthController>();

            if (playerHealth != null)
            {
                bool healthAdded = playerHealth.AddHealth(HealingAmount); // Try to add health
                if (healthAdded)
                {
                    isPickedUp = true;
                    gameObject.SetActive(false); // Deactivate the health pickup object
                    //Destroy(gameObject); // Destroy the health pickup object
                }
            }
        }
    }



}

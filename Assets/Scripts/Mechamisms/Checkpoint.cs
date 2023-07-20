using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int hitCount = 0;
    private bool activated = false;
    private Vector3 checkpointPosition;
    public int playerHP;
    public int[] ammoCounts;
    public bool[] pickupStates;
    [SerializeField]
    private Animator Amin;
    private void Start()
    {
        checkpointPosition = transform.position;
        //pickupStates = new bool[transform.childCount];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Projectile"))
        {
            hitCount++;
            if (hitCount >= 3)
            {
                ActivateCheckpoint();
                Amin.SetTrigger("Active");
            }
        }
    }

    public void ActivateCheckpoint()
    {
        activated = true;
        // Zapisz aktualny stan sceny
        SaveGameState();
        Debug.Log("Checkpoint activated!");
    }

    public void SaveGameState()
    {
        // Zapisz stan gracza
        HealthController playerHealth = FindObjectOfType<HealthController>();
        if (playerHealth != null)
        {
            playerHP = playerHealth.GetHealth();
            Debug.Log("Saving player health: " + playerHP);
        }

        // Zapisz stan amunicji gracza
        AmmoStorage ammoStorage = FindObjectOfType<AmmoStorage>();
        if (ammoStorage != null)
        {
            ammoCounts = ammoStorage.GetAmmoCounts();
            Debug.Log("Saving ammo counts: " + string.Join(", ", ammoCounts));
        }

        // Zapisz stan wszystkich pickup�w
        pickupStates = new bool[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pickupStates[i] = transform.GetChild(i).gameObject.activeSelf;
            Debug.Log("Saving pickup at index " + i + ": " + pickupStates[i]);
        }
        FindObjectOfType<CheckpointManager>().SetLastCheckpoint(this);
    }


    // Funkcja zwracaj�ca pozycj� checkpointu
    public Vector3 GetCheckpointPosition()
    {
        return checkpointPosition;
    }

    // Funkcja zwracaj�ca stan gracza
    public int GetPlayerHP()
    {
        return playerHP;
    }

    // Funkcja zwracaj�ca stan amunicji gracza
    public int[] GetAmmoCounts()
    {
        return ammoCounts;
    }

    // Funkcja zwracaj�ca stan pickupa o danym indeksie
    public bool IsPickupActive(int pickupIndex)
    {
        return pickupStates[pickupIndex];
    }
}

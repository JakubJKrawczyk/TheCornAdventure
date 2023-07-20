using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int hitCount = 0;
    private bool activated = false;
    private Vector3 checkpointPosition;
    private int playerHP;
    private int[] ammoCounts;
    private bool[] pickupStates;

    private void Start()
    {
        checkpointPosition = transform.position;
        pickupStates = new bool[transform.childCount];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            hitCount++;
            if (hitCount >= 3)
            {
                ActivateCheckpoint();
            }
        }
    }

    private void ActivateCheckpoint()
    {
        activated = true;
        // Zapisz aktualny stan sceny
        SaveGameState();
        Debug.Log("Checkpoint activated!");
    }

    private void SaveGameState()
    {
        // Zapisz stan gracza
        HealthController playerHealth = FindObjectOfType<HealthController>();
        if (playerHealth != null)
        {
            playerHP = playerHealth.GetHealth();
        }

        // Zapisz stan amunicji gracza
        AmmoStorage ammoStorage = FindObjectOfType<AmmoStorage>();
        if (ammoStorage != null)
        {
            ammoCounts = ammoStorage.GetAmmoCounts();
        }

        // Zapisz stan wszystkich pickupów
        for (int i = 0; i < transform.childCount; i++)
        {
            pickupStates[i] = transform.GetChild(i).gameObject.activeSelf;
        }
    }

    // Funkcja zwracająca pozycję checkpointu
    public Vector3 GetCheckpointPosition()
    {
        return checkpointPosition;
    }

    // Funkcja zwracająca stan gracza
    public int GetPlayerHP()
    {
        return playerHP;
    }

    // Funkcja zwracająca stan amunicji gracza
    public int[] GetAmmoCounts()
    {
        return ammoCounts;
    }

    // Funkcja zwracająca stan pickupa o danym indeksie
    public bool IsPickupActive(int pickupIndex)
    {
        return pickupStates[pickupIndex];
    }
}

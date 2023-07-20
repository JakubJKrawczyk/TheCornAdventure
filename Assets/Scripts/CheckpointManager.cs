using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint lastCheckpoint;
    private bool activated = false;

    // Przyciski do menu pauzy lub œmierci, które umo¿liwi¹ powrót do ostatniego aktywowanego checkpointu

    public void SetLastCheckpoint(Checkpoint checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    public void LoadLastCheckpoint()
    {
        if (lastCheckpoint != null)
        {
            // Za³aduj scenê i ustaw gracza na pozycji ostatniego aktywowanego checkpointu
            string sceneName = SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            CharacterController2D player = FindObjectOfType<CharacterController2D>();
            if (player != null)
            {
                player.transform.position = lastCheckpoint.GetCheckpointPosition();
            }
            Debug.Log("Player position set to last checkpoint: " + lastCheckpoint.GetCheckpointPosition());
            // Przywróæ stan sceny z ostatniego aktywowanego checkpointu
            RestoreGameState(lastCheckpoint);
        }
    }
    /*private void ActivateCheckpoint()
    {
        activated = true;
        // Zapisz aktualny stan sceny
        SaveGameState();

        // Odœwie¿ indeksy pickupów
        RefreshPickupIndexes();

        Debug.Log("Checkpoint activated!");
    }*/
    public void RefreshIndex(int newIndex)
    {
        transform.SetSiblingIndex(newIndex);
    }
    private void RefreshPickupIndexes()
    {
        PickupController[] pickups = FindObjectsOfType<PickupController>();
        for (int i = 0; i < pickups.Length; i++)
        {
            pickups[i].RefreshIndex(i);
        }
    }

    private void RestoreGameState(Checkpoint checkpoint)
    {
        // Przywróæ stan gracza
        HealthController playerHealth = FindObjectOfType<HealthController>();
        if (playerHealth != null)
        {
            playerHealth.SetHealth(checkpoint.playerHP);
            Debug.Log("Restoring player health: " + checkpoint.playerHP);
        }

        // Przywróæ stan amunicji gracza
        AmmoStorage ammoStorage = FindObjectOfType<AmmoStorage>();
        if (ammoStorage != null)
        {
            ammoStorage.RestoreAmmo(checkpoint.ammoCounts);
            Debug.Log("Restoring ammo counts: " + string.Join(", ", checkpoint.GetAmmoCounts()));
        }

        // Przywróæ stan wszystkich pickupów
        PickupController[] pickups = FindObjectsOfType<PickupController>();
        foreach (PickupController pickup in pickups)
        {
            int pickupIndex = pickup.transform.GetSiblingIndex();
            if (pickupIndex >= 0 && pickupIndex < checkpoint.pickupStates.Length) // SprawdŸ, czy indeks jest prawid³owy
            {
                bool pickupActive = checkpoint.IsPickupActive(pickupIndex);
                if (pickupActive)
                {
                    pickup.ActivatePickup(); // W³¹cz pickup, jeœli by³ aktywny w momencie zapisu checkpointu
                }
                else
                {
                    pickup.DeactivatePickup(); // Wy³¹cz pickup, jeœli nie by³ aktywny w momencie zapisu checkpointu
                }
                Debug.Log("Restoring pickup at index " + pickupIndex + ": " + pickupActive);
            }
            else
            {
                Debug.LogError("Invalid pickup index: " + pickupIndex);
            }
        }

    }
}
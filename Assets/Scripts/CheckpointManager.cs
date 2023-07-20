using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint lastCheckpoint;
    private bool activated = false;

    // Przyciski do menu pauzy lub �mierci, kt�re umo�liwi� powr�t do ostatniego aktywowanego checkpointu

    public void SetLastCheckpoint(Checkpoint checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    public void LoadLastCheckpoint()
    {
        if (lastCheckpoint != null)
        {
            // Za�aduj scen� i ustaw gracza na pozycji ostatniego aktywowanego checkpointu
            string sceneName = SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            CharacterController2D player = FindObjectOfType<CharacterController2D>();
            if (player != null)
            {
                player.transform.position = lastCheckpoint.GetCheckpointPosition();
            }
            Debug.Log("Player position set to last checkpoint: " + lastCheckpoint.GetCheckpointPosition());
            // Przywr�� stan sceny z ostatniego aktywowanego checkpointu
            RestoreGameState(lastCheckpoint);
        }
    }
    /*private void ActivateCheckpoint()
    {
        activated = true;
        // Zapisz aktualny stan sceny
        SaveGameState();

        // Od�wie� indeksy pickup�w
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
        // Przywr�� stan gracza
        HealthController playerHealth = FindObjectOfType<HealthController>();
        if (playerHealth != null)
        {
            playerHealth.SetHealth(checkpoint.playerHP);
            Debug.Log("Restoring player health: " + checkpoint.playerHP);
        }

        // Przywr�� stan amunicji gracza
        AmmoStorage ammoStorage = FindObjectOfType<AmmoStorage>();
        if (ammoStorage != null)
        {
            ammoStorage.RestoreAmmo(checkpoint.ammoCounts);
            Debug.Log("Restoring ammo counts: " + string.Join(", ", checkpoint.GetAmmoCounts()));
        }

        // Przywr�� stan wszystkich pickup�w
        PickupController[] pickups = FindObjectsOfType<PickupController>();
        foreach (PickupController pickup in pickups)
        {
            int pickupIndex = pickup.transform.GetSiblingIndex();
            if (pickupIndex >= 0 && pickupIndex < checkpoint.pickupStates.Length) // Sprawd�, czy indeks jest prawid�owy
            {
                bool pickupActive = checkpoint.IsPickupActive(pickupIndex);
                if (pickupActive)
                {
                    pickup.ActivatePickup(); // W��cz pickup, je�li by� aktywny w momencie zapisu checkpointu
                }
                else
                {
                    pickup.DeactivatePickup(); // Wy��cz pickup, je�li nie by� aktywny w momencie zapisu checkpointu
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
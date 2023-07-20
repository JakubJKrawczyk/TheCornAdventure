using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint lastCheckpoint;

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
            Debug.Log("Restoring ammo counts: " + string.Join(", ", checkpoint.ammoCounts));
        }

        // Przywróæ stan wszystkich pickupów
        PickupController[] pickups = FindObjectsOfType<PickupController>();
        foreach (PickupController pickup in pickups)
        {
            // Aktywuj lub dezaktywuj pickupy w zale¿noœci od ich stanu w checkpointcie
            bool pickupActive = checkpoint.IsPickupActive(pickup.transform.GetSiblingIndex());
            pickup.RestorePickup(pickupActive);
            Debug.Log("Restoring pickup at index " + pickup.transform.GetSiblingIndex() + ": " + pickupActive);

        }
    }
}
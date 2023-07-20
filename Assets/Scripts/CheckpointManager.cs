using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint lastCheckpoint;

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

            // Przywr�� stan sceny z ostatniego aktywowanego checkpointu
            RestoreGameState(lastCheckpoint);
        }
    }

    private void RestoreGameState(Checkpoint checkpoint)
    {
        // Przywr�� stan gracza
        HealthController playerHealth = FindObjectOfType<HealthController>();
        if (playerHealth != null)
        {
            playerHealth.SetHealth(checkpoint.GetPlayerHP());
        }

        // Przywr�� stan amunicji gracza
        AmmoStorage ammoStorage = FindObjectOfType<AmmoStorage>();
        if (ammoStorage != null)
        {
            ammoStorage.RestoreAmmo(checkpoint.GetAmmoCounts());
        }

        // Przywr�� stan wszystkich pickup�w
        PickupController[] pickups = FindObjectsOfType<PickupController>();
        foreach (PickupController pickup in pickups)
        {
            // Aktywuj lub dezaktywuj pickupy w zale�no�ci od ich stanu w checkpointcie
            pickup.RestorePickup(checkpoint.IsPickupActive(pickup.transform.GetSiblingIndex()));
        }
    }
}
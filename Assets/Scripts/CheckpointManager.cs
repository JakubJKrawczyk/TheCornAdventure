using System;
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
        Time.timeScale= 1.0f;
        if (lastCheckpoint != null)
        {
            
            CharacterController2D player = FindObjectOfType<CharacterController2D>();
            if (player != null)
            {
                // Set the player position to the last activated checkpoint position
                player.transform.position = lastCheckpoint.GetCheckpointPosition();
                Debug.Log("Player position set to last checkpoint: " + lastCheckpoint.GetCheckpointPosition());

                // Przywróæ stan sceny z ostatniego aktywowanego checkpointu
                RestoreGameState(lastCheckpoint);
            }
        }
    }

    public void RefreshIndex(int newIndex)
    {
        transform.SetSiblingIndex(newIndex);
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
        int pickupLayer = LayerMask.NameToLayer("Pickup");
        GameObject[] allPickups = GameObject.FindObjectsOfType<GameObject>();
        foreach (var pickupState in checkpoint.pickupStateList)
        {
            GameObject pickupObject = pickupState.gameObject;
            bool isEnabled = pickupState.isEnabled;
            Vector3 initialPosition = pickupState.initialPosition;

            string pickupName = pickupObject.name;
            GameObject pickupInScene = Array.Find(allPickups, obj => obj.name == pickupName);

            // If the pickup is found in the scene, set its active state based on the checkpoint's stored state.
            if (pickupInScene != null)
            {
                pickupInScene.SetActive(isEnabled);
                pickupInScene.transform.position = initialPosition;
                Debug.Log(pickupInScene + " " + isEnabled);
                if (pickupInScene.CompareTag("Ammo"))
                {
                    if (pickupInScene.transform.position != initialPosition)
                    {
                        Destroy(pickupInScene);
                    }
                }
            }

        }



    }

    private struct GameObjectState
    {
        public GameObject gameObject;
        public bool isEnabled;
        public Vector3 initialPosition;
    }
}
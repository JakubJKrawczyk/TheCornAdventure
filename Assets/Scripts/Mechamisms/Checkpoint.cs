using System;
using System.Collections.Generic;
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
        
    }
    private List<GameObjectState> initialPickupStates = new List<GameObjectState>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Projectile"))
        {
            hitCount++;
            if (hitCount >= 3)
            {
                StoreInitialPickupStates();
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

        // Zapisz stan wszystkich pickupów
        pickupStateList = new List<GameObjectState>();

        FindObjectOfType<CheckpointManager>().SetLastCheckpoint(this);
    }


    public List<GameObjectState> pickupStateList = new List<GameObjectState>();

    public struct GameObjectState
    {
        public GameObject gameObject;
        public bool isEnabled;
        public Vector3 initialPosition;
    }

    public void PickupStates()
    {
        
        int pickupLayer = LayerMask.NameToLayer("Pickup");
        GameObject[] pickupObjects = GameObject.FindObjectsOfType<GameObject>();

        List<GameObjectState> pickupStateList = new List<GameObjectState>();

        foreach (GameObject pickupObject in pickupObjects)
        {
            if (pickupObject.layer == pickupLayer)
            {
                pickupStateList.Add(new GameObjectState
                {
                    gameObject = pickupObject,
                    isEnabled = pickupObject.activeSelf,
                    initialPosition = pickupObject.transform.position
                });
            }
        }
        
    }
    private void StoreInitialPickupStates()
    {
        int pickupLayer = LayerMask.NameToLayer("Pickup");
        GameObject[] pickupObjects = GameObject.FindObjectsOfType<GameObject>();

        initialPickupStates.Clear();

        foreach (GameObject pickupObject in pickupObjects)
        {
            if (pickupObject.layer == pickupLayer)
            {
                initialPickupStates.Add(new GameObjectState
                {
                    gameObject = pickupObject,
                    isEnabled = pickupObject.activeSelf,
                    initialPosition = pickupObject.transform.position
                });
            }
        }
    }

    public void ResetPickupStates()
    {
        foreach (var pickupState in initialPickupStates)
        {
            GameObject pickupObject = pickupState.gameObject;
            bool isEnabled = pickupState.isEnabled;
            Vector3 initialPosition = pickupState.initialPosition;

            pickupObject.SetActive(isEnabled);
            pickupObject.transform.position = initialPosition;
        }
    }
    // Funkcja zwracaj¹ca pozycjê checkpointu
    public Vector3 GetCheckpointPosition()
    {
        return checkpointPosition;
    }

    // Funkcja zwracaj¹ca stan gracza
    public int GetPlayerHP()
    {
        return playerHP;
    }

    // Funkcja zwracaj¹ca stan amunicji gracza
    public int[] GetAmmoCounts()
    {
        return ammoCounts;
    }

    // Funkcja zwracaj¹ca stan pickupa o danym indeksie
    public bool IsPickupActive(int pickupIndex)
    {
        return pickupStates[pickupIndex];
    }
}

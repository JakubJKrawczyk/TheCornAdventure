using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public KeyCode pickupKey = KeyCode.F;
    public KeyCode dropKey = KeyCode.G;
    public float pickupDistance = 0.5f;
    public LayerMask itemLayer;
    public ItemHolder itemHolder;
    [SerializeField] private SceneManager sceneManager;
    private Transform playerTransform;

    public AmmoStorage AmmoStorage;
    public HealthController HealthController;

    private void Start()
    {
        playerTransform = sceneManager.Player.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            CheckForPickup();
        }

        if (Input.GetKeyDown(dropKey))
        {
            DropAllItems();
        }
    }

    private void CheckForPickup()
    {
        Collider2D[] itemsInRange = Physics2D.OverlapCircleAll(playerTransform.position, pickupDistance, itemLayer);

        if (itemsInRange.Length > 0)
        {
            // Find the closest item
            float minDistance = float.MaxValue;
            Collider2D closestItem = null;

            foreach (Collider2D item in itemsInRange)
            {
                float distance = Vector2.Distance(playerTransform.position, item.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestItem = item;
                }
            }

            // Pick up the closest item
            if (closestItem != null)
            {
                PickUpItem(closestItem.gameObject);
            }
        }
    }

    private void PickUpItem(GameObject item)
    {
        if (item.tag == "Ammo")
        {
            int AmmoType = item.GetComponent<AmmoPickUp>().AmmoType;
            int AmmoAmount = item.GetComponent<AmmoPickUp>().AmmoAmount;

            bool AmmoAdded = AmmoStorage.AddAmmo(AmmoType, AmmoAmount); // Check if ammo can be added
            if (AmmoAdded)
            {
                item.SetActive(false);
            }
        }
        else if (item.tag == "Health")
        {
            int HealingAmount = item.GetComponent<HealthPickUp>().HealingAmount;
            bool HealthAdded = HealthController.AddHealth(HealingAmount);   // Check if health can be added/subtracted
            if (HealthAdded)
            {
                item.SetActive(false);
            }
        }
        else
        {
            itemHolder.AddItem(item);
        }
    }

    private void DropAllItems()
    {
        itemHolder.DropAllItems(playerTransform);
        AmmoStorage.DiscardFirstAmmo(); 
    }
}
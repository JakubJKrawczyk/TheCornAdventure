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
        itemHolder.AddItem(item);
    }

    private void DropAllItems()
    {
        itemHolder.DropAllItems(playerTransform);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private KeyCode pickupKey = KeyCode.F;
    [SerializeField] private KeyCode dropKey = KeyCode.G;
    [SerializeField] private float pickupDistance = 0.5f;
    [SerializeField] private LayerMask itemLayer;
    [Header("Dependencies")]
    [SerializeField] private InGameSceneManager sceneManager;
    private Transform playerTransform;

    //private script variables
    private Transform _playerTransform;

    private ItemHolder _itemHolder;
    private AmmoStorage _ammoStorage;
    private HealthController _healthController;

    private void Start()
    {
        _playerTransform = sceneManager.Player.transform;
        _ammoStorage = GetComponent<AmmoStorage>();
        _healthController = GetComponent<HealthController>();
        _itemHolder = sceneManager.GetComponent<ItemHolder>();
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
    public void DeactivatePickup()
    {
        gameObject.SetActive(false);
    }

    public void ActivatePickup()
    {
        gameObject.SetActive(true);
    }
    public void RefreshIndex(int newIndex)
    {
        transform.SetSiblingIndex(newIndex);
    }
    private void CheckForPickup()
    {
        Collider2D[] itemsInRange = Physics2D.OverlapCircleAll(_playerTransform.position, pickupDistance, itemLayer);

        if (itemsInRange.Length > 0)
        {
            // Find the closest item
            float minDistance = float.MaxValue;
            Collider2D closestItem = null;

            foreach (Collider2D item in itemsInRange)
            {
                float distance = Vector2.Distance(_playerTransform.position, item.transform.position);
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
        if (item.CompareTag("Ammo"))
        {
            int AmmoType = item.GetComponent<AmmoPickUp>().AmmoType;
            int AmmoAmount = item.GetComponent<AmmoPickUp>().AmmoAmount;

            bool AmmoAdded = _ammoStorage.AddAmmo(AmmoType, AmmoAmount); // Check if ammo can be added
            if (AmmoAdded)
            {
                Destroy(item);
            }
        }
        else if (item.CompareTag("Health"))
        {
            int HealingAmount = item.GetComponent<HealthPickUp>().HealingAmount;
            bool HealthAdded = _healthController.AddHealth(HealingAmount);   // Check if health can be added/subtracted
            if (HealthAdded)
            {
                item.SetActive(false);
            }
        }

    }
    public void RestorePickup(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void DropAllItems()
    {
        _itemHolder.DropAllItems(_playerTransform);
        _ammoStorage.DiscardFirstAmmo(); 
    }
}

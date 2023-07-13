using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private readonly float spacing = 3f; // Spacing between dropped items
    [Header("Dependencies")]
    [SerializeField] private readonly WeightController WeightController;


    //private script Variables
    private List<GameObject> items = new();

    public void AddItem(GameObject item)
    {
        items.Add(item);
        item.transform.SetParent(transform); // Set the item's parent to the ItemHolder for organization.
    }

    public void DropAllItems(Transform dropPosition)
    {
        Vector3 dropOffset = Vector3.zero;
        foreach (GameObject item in items)
        {
            item.transform.SetParent(null); // Set the item's parent to null to remove it from the ItemHolder.
            item.SetActive(true);
            item.transform.position = dropPosition.position + dropOffset;
            dropOffset += new Vector3(spacing, 0f, 0f); // Increment the drop offset for the next item.
        }
        items.Clear();
    }
}
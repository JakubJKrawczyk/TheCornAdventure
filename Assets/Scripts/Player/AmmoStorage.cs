using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoStorage : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] private Sprite[] AmmoSprite;
    [SerializeField] private GameObject[] AmmoPrefabs;
    [SerializeField] private GameObject UI;

    //private script variables
    private Stack<Ammo> ammoList;
    private List<GameObject> AmmoPanelSlots;
    private GameObject DefaultGrainPanel;
    private WeightController WeightController;

    
    public void Start()
    {
        ammoList = new Stack<Ammo>();
        AmmoPanelSlots = new List<GameObject>();
        GameObject ammoPanel = UI.transform.GetChild(1).gameObject;
        Debug.Log(ammoPanel.transform.childCount);
        foreach (Transform ammo in ammoPanel.transform)
        {
            AmmoPanelSlots.Add(ammo.gameObject);
        }

        DefaultGrainPanel = AmmoPanelSlots[AmmoPanelSlots.Count-1];
        Debug.Log(DefaultGrainPanel.gameObject.name);
        AmmoPanelSlots.RemoveAt(AmmoPanelSlots.Count-1);
        Debug.Log(AmmoPanelSlots.Count);
        WeightController = GetComponent<WeightController>();

        RefreshAmmo();
    }

    public void DiscardFirstAmmo()
    {
        if (ammoList.Count > 0)
        {
            Ammo discardedAmmo = ammoList.Pop();
            RefreshAmmo();

            if (AmmoPrefabs.Length > 0)
            {
                GameObject ammoPrefab = AmmoPrefabs[discardedAmmo.type];

                GameObject spawnedAmmo = Instantiate(ammoPrefab, transform.position + new Vector3(0.75f, 0), Quaternion.identity);
                AmmoPickUp ammoPickup = spawnedAmmo.GetComponent<AmmoPickUp>();
                if (ammoPickup != null)
                {
                    ammoPickup.AmmoAmount = discardedAmmo.amount;
                    ammoPickup.AmmoType = discardedAmmo.type;

                    ammoPickup.ShouldFloat = false;

                    Rigidbody2D spawnedAmmoRB = spawnedAmmo.GetComponent<Rigidbody2D>();
                    spawnedAmmoRB.mass = 0.5f + WeightController.CalculateAmmoWeight(discardedAmmo.type, discardedAmmo.amount);
                    spawnedAmmoRB.gravityScale = 1f;

                    spawnedAmmoRB.AddForce(new Vector2(1f, 1.5f), ForceMode2D.Impulse); // Apply force in a curved path

                    spawnedAmmo.GetComponent<PolygonCollider2D>().enabled = true;
                }

                WeightController.RemoveAmmoWeight(discardedAmmo.type, discardedAmmo.amount);
            }
        }
    }


    public int UseFirstAmmo()
    {
        if (ammoList.Count > 0)
        {
            Ammo firstAmmo = ammoList.Peek();
            firstAmmo.amount--;
         
            if (firstAmmo.amount <= 0)
            {
                // If the amount reaches zero, remove the ammo from the list
                ammoList.Pop();
            }
            RefreshAmmo();
            WeightController.RemoveAmmoWeight(firstAmmo.type, 1);
            return firstAmmo.type;
        }
        else
        {
            return -1;
        }
    }

    internal Ammo GetAmmo(int type)
    {
        return ammoList.ToList().Find(ammo => ammo.type == type);
    }


    public bool AddAmmo(int type, int amount)
    {
        Ammo existingAmmo = GetAmmo(type);
        if (existingAmmo != null)
        {
            // Ammo exists, check if it can be added
            if (existingAmmo.amount >= 99)
            {
                return false; // Cannot be added, return false to PickUpController, so object won't be destroyed
            }
            existingAmmo.amount += amount;
            if (existingAmmo.amount > 99)
            {
                existingAmmo.amount = 99;
            }
            RefreshAmmo();
            WeightController.AddAmmoWeight(type, amount);
            return true; // Return true to PickUpController - destroy object
        }
        else
        {
            // Add a new ammo to the beginning of the list
            Ammo newAmmo = new Ammo(type, amount);
            ammoList.Push(newAmmo);
            RefreshAmmo();
            WeightController.AddAmmoWeight(type, amount);
            return true; // Return true to PickUpController - destroy object
        }
    }

    public void RemoveAmmo(int type, int amount)
    {
        Ammo existingAmmo = GetAmmo(type);
        if (existingAmmo != null)
        {
            existingAmmo.amount -= amount;
            if (existingAmmo.amount <= 0)
            {
                ammoList.ToList().Remove(existingAmmo);
            }
            RefreshAmmo();
            WeightController.AddAmmoWeight(type, amount);
        }
    }

   

    public void RefreshAmmo()
    {
        for (int i = 0; i < AmmoPanelSlots.Count; i++)
        {
            GameObject panel = AmmoPanelSlots[i];
            TextMeshProUGUI amountText = panel.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
            Image ammoImage = panel.transform.Find("Image").GetComponent<Image>();

            if (i < ammoList.Count)
            {
                Ammo ammo = ammoList.ToList()[i];
                int amount = ammo.amount;

                if (amount > 0)
                {
                    amountText.text = amount.ToString();
                    ammoImage.sprite = AmmoSprite[(int)ammo.type];

                    panel.SetActive(true);
                }
                else
                {
                    panel.SetActive(false);
                }
            }
            else
            {
                panel.SetActive(false);
            }
        }

        if (ammoList.Count == 0)
        {
           DefaultGrainPanel.SetActive(true);
        }
        else
        {
            DefaultGrainPanel.SetActive(false);
        }
    }

}




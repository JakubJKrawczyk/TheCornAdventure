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
    public LayerMask groundLayer;

    //private script variables
    private Stack<Ammo> ammoList;
    private List<GameObject> AmmoPanelSlots;
    private GameObject DefaultGrainPanel;
    private WeightController WeightController;
    private List<DiscardedAmmoData> discardedAmmoDataList;

    //TODO: Zamienić Listę na Stos i pozmieniać funkcję pod stos
    private struct DiscardedAmmoData
    {
        public int type;
        public int amount;
        public Vector3 position;
    }
    public void Start()
    {
        ammoList = new Stack<Ammo>();
        AmmoPanelSlots = new List<GameObject>();
        GameObject ammoPanel = UI.transform.GetChild(1).gameObject;
        foreach (Transform ammo in ammoPanel.transform)
        {
            AmmoPanelSlots.Add(ammo.gameObject);
        }

        DefaultGrainPanel = AmmoPanelSlots[^1];
        AmmoPanelSlots.RemoveAt(AmmoPanelSlots.Count - 1);
        WeightController = GetComponent<WeightController>();
        discardedAmmoDataList = new List<DiscardedAmmoData>();
    }
    public void DiscardFirstAmmo()
    {

        float Direction = 1f; // so ammo will spawn from correct side
        float raycastDistance = 1f;
        Vector2 raycastDirection = Vector2.right;

        if (transform.localScale.x < 0)
        {
            Direction = -1;
            raycastDirection = Vector2.left;
        }

        Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastDistance, groundLayer); // check if there's ground

        if (ammoList.Count > 0 && hit.collider == null) //if there's no ground in front of player
        {
            Ammo discardedAmmo = ammoList.Pop();
            RefreshAmmo();

            if (AmmoPrefabs.Length > 0)
            {
                GameObject ammoPrefab = AmmoPrefabs[discardedAmmo.type];
                GameObject spawnedAmmo = Instantiate(ammoPrefab, transform.position + new Vector3(Direction * 0.75f, 0), Quaternion.identity);
                AmmoPickUp ammoPickup = spawnedAmmo.GetComponent<AmmoPickUp>();
                if (ammoPickup != null)
                {
                    ammoPickup.AmmoAmount = discardedAmmo.amount;
                    ammoPickup.AmmoType = discardedAmmo.type;

                    ammoPickup.ShouldFloat = false;

                    Rigidbody2D spawnedAmmoRB = spawnedAmmo.GetComponent<Rigidbody2D>();
                    spawnedAmmoRB.mass = 0.5f + (WeightController.CalculateAmmoWeight(discardedAmmo.type, discardedAmmo.amount) * 3);
                    spawnedAmmoRB.gravityScale = 1f;

                    spawnedAmmoRB.AddForce(new Vector2(Direction * 1, 1.5f), ForceMode2D.Impulse); // Apply force in a curved path

                    spawnedAmmo.GetComponent<PolygonCollider2D>().enabled = true;
                }
                DiscardedAmmoData ammoData = new DiscardedAmmoData
                {
                    type = discardedAmmo.type,
                    amount = discardedAmmo.amount,
                    position = spawnedAmmo.transform.position
                };
                discardedAmmoDataList.Add(ammoData);
                WeightController.RemoveAmmoWeight(discardedAmmo.type, discardedAmmo.amount);


            }
        }
    }
    public void RestoreAmmoPositions()
    {
        foreach (var ammoData in discardedAmmoDataList)
        {
            Ammo ammo = GetAmmo(ammoData.type);
            if (ammo != null)
            {
                GameObject ammoObject = AmmoPrefabs[ammo.type];
                GameObject[] spawnedAmmoObjects = GameObject.FindGameObjectsWithTag("Ammo");
                foreach (var spawnedAmmoObject in spawnedAmmoObjects)
                {
                    AmmoPickUp ammoPickup = spawnedAmmoObject.GetComponent<AmmoPickUp>();
                    if (ammoPickup != null && ammoPickup.AmmoType == ammo.type)
                    {
                        spawnedAmmoObject.transform.position = ammoData.position;
                        break;
                    }
                }
            }
        }
        discardedAmmoDataList.Clear();
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
        if (ammoList.Count >= 5)
        {
            return false; // All slots are full, return false to PickUpController, so object won't be destroyed
        }

        Ammo newAmmo = new Ammo(type, amount, amount);
        ammoList.Push(newAmmo);
        RefreshAmmo();
        WeightController.AddAmmoWeight(type, amount);
        return true; // Return true to PickUpController - destroy object
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
    public void RemoveAmmoFromGround(GameObject ammoObject)
    {
        if (ammoObject != null && ammoObject.CompareTag("Ammo"))
        {
            Destroy(ammoObject);
        }
    }

    public void RefreshAmmo()
    {
        for (int i = 0; i < AmmoPanelSlots.Count; i++)
        {
            GameObject panel = AmmoPanelSlots[i];
            TextMeshProUGUI amountText = panel.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
            Image ammoImage = panel.transform.Find("Image").GetComponent<Image>();
            Image ammoImageBG = panel.transform.Find("ImageBG").GetComponent<Image>();
            if (i < ammoList.Count)
            {
                Ammo ammo = ammoList.ToList()[i];
                float amount = ammo.amount;

                if (amount > 0)
                {
                    amountText.text = amount.ToString();
                    ammoImage.sprite = AmmoSprite[ammo.type];
                    ammoImageBG.sprite = AmmoSprite[ammo.type];
                    
                    if (amount / ammo.Maxamount == 1)
                    {
                        ammoImage.fillAmount = amount / ammo.Maxamount;
                    }
                    else
                    {
                        int fillAmountSteps = Mathf.RoundToInt(amount / ammo.Maxamount * 10f);
                        float newFillAmount = fillAmountSteps / 10f;
                        ammoImage.fillAmount = newFillAmount + 0.05f;
                    }
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
    public int[] GetAmmoCounts()
    {
        int[] ammoCounts = new int[AmmoPrefabs.Length];
        for (int i = 0; i < ammoList.Count; i++)
        {
            Ammo ammo = ammoList.ElementAt(i);
            ammoCounts[ammo.type] = ammo.amount;
        }
        return ammoCounts;
    }
    public void RestoreAmmo(int[] ammoCounts)
    {
        for (int i = 0; i < ammoCounts.Length; i++)
        {
            Ammo ammo = GetAmmo(i);
            if (ammo != null)
            {
                ammo.amount = ammoCounts[i];
            }
        }
        RefreshAmmo();
    }

}







using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoStorage : MonoBehaviour
{
    private List<Ammo> ammoList;

    public GameObject[] AmmoPanel;
    public GameObject DefaultGrainPanel;
    public Sprite[] AmmoSprite;


    public WeightController WeightController;

    public AmmoStorage()
    {
        ammoList = new List<Ammo>();
    }
    public void Start()
    {
        RefreshAmmo();
    }

    public void DiscardFirstAmmo()
    {
        if (ammoList.Count > 0)
        {
            ammoList.RemoveAt(0);
        }
        RefreshAmmo();
    }


    public int UseFirstAmmo()
    {
        if (ammoList.Count > 0)
        {
            Ammo firstAmmo = ammoList[0];
            firstAmmo.amount--;
         
            if (firstAmmo.amount <= 0)
            {
                // If the amount reaches zero, remove the ammo from the list
                ammoList.RemoveAt(0);
            }
            RefreshAmmo();
            return firstAmmo.type;
        }
        else
        {
            return -1;
        }
    }

    public Ammo GetAmmo(int type)
    {
        return ammoList.Find(ammo => ammo.type == type);
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
            return true; // Return true to PickUpController - destroy object
        }
        else
        {
            // Add a new ammo to the beginning of the list
            Ammo newAmmo = new Ammo(type, amount);
            ammoList.Insert(0, newAmmo);
            RefreshAmmo();
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
                ammoList.Remove(existingAmmo);
            }
            RefreshAmmo();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddAmmo(0, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddAmmo(1, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddAmmo(2, 10);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RemoveAmmo(0, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RemoveAmmo(1, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            RemoveAmmo(2, 10);
        }
    }

    public void RefreshAmmo()
    {
        for (int i = 0; i < AmmoPanel.Length; i++)
        {
            GameObject panel = AmmoPanel[i];
            TextMeshProUGUI amountText = panel.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
            Image ammoImage = panel.transform.Find("Image").GetComponent<Image>();

            if (i < ammoList.Count)
            {
                Ammo ammo = ammoList[i];
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
public class Ammo
{
    public int type;
    public int amount;

    public Ammo(int type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }
}
public enum AmmoType
{
    Type0,
    Type1,
    Type2,
}


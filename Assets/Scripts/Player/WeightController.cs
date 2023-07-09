using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightController : MonoBehaviour
{
    public float CurrentWeight = 1.0f;
    public float MinWeight = 1.75f;
    public float MaxWeight = 3f;
    private Rigidbody2D rb;

    public float[] GrainsWeight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = CurrentWeight;
    }

    public void AddWeight(float weight)
    {
        CurrentWeight += weight;
        RefreshWeight();
    }
    public void AddAmmoWeight(int ammoType, int ammoamount)
    {
        CurrentWeight += (GrainsWeight[ammoType] * ammoamount);
        Debug.Log(CurrentWeight);
        RefreshWeight();
    }

    public void RemoveAmmoWeight(int ammoType, int ammoamount)
    {
        CurrentWeight -= (GrainsWeight[ammoType] * ammoamount);
        RefreshWeight();
    }

    public float CalculateAmmoWeight(int ammoType, int ammoamount)
    {
        return GrainsWeight[ammoType] * ammoamount;
    }

    public void RefreshWeight()
    {
        rb.mass = CurrentWeight;
        if (rb.mass > MaxWeight)
        {
            rb.mass = MaxWeight;
        }
        if (rb.mass < MinWeight)
        {
            rb.mass = MinWeight;
        }
    }
}

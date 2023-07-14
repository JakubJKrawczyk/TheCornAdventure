using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeightController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float MinWeight = 1.75f;
    [SerializeField] private float MaxWeight = 3f;
    [SerializeField] private float[] GrainsWeight;

    //private script variables
    private Rigidbody2D rb;
    private float CurrentWeight;


    private void Start()
    {
        CurrentWeight = MinWeight;
        rb = GetComponent<Rigidbody2D>();
        rb.mass = CurrentWeight;
    }

    public void AddWeight(float weight)
    {
        CurrentWeight += weight;
        RefreshWeight();
    }
    public void AddAmmoWeight(int ammoType, int ammo_amount)
    {
        CurrentWeight += (GrainsWeight[ammoType] * ammo_amount);
        RefreshWeight();
    }

    public void RemoveAmmoWeight(int ammoType, int ammo_amount)
    {
        CurrentWeight -= (GrainsWeight[ammoType] * ammo_amount);
        RefreshWeight();
    }

    public float CalculateAmmoWeight(int ammoType, int ammo_amount)
    {
        return GrainsWeight[ammoType] * ammo_amount;
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

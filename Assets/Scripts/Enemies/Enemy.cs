using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Min(0)]
    [field: SerializeField] public float InitialHP;
    [Min(0)]
    [field: SerializeField] public float DamageAmount;

    private float CurrentHP;

    private void Start()
    {
        CurrentHP = InitialHP;
    }

    private void ReceiveDamage(float damageReceived)
    {
        CurrentHP -= damageReceived;

        if(CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

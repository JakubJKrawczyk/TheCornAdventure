using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Min(0)]
    [field: SerializeField] protected float InitialHP;
    [Min(0)]
    [field: SerializeField] protected float DamageAmount;

    protected float CurrentHP;

    protected void Start()
    {
        CurrentHP = InitialHP;
    }

    protected void ReceiveDamage(float damageReceived)
    {
        CurrentHP -= damageReceived;

        if(CurrentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

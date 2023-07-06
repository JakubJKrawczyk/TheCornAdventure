using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public float InitialHP { get; private set; }
    [field: SerializeField] public float DamageAmount { get; private set; }

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

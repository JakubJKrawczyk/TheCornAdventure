using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Min(0)]
    [field: SerializeField] protected float _initialHP;
    [Min(0)]
    [field: SerializeField] protected float _damageAmount;

    protected float _currentHP;

    protected void Start()
    {
        _currentHP = _initialHP;
    }

    public void ReceiveDamage(float damageReceived)
    {
        _currentHP -= damageReceived;

        if(_currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

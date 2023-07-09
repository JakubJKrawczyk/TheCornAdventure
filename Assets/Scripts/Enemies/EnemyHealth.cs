using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Min(0)]
    [field: SerializeField] protected float _initialHP;
    [Min(0)]
    [field: SerializeField] protected float _damageAmount;

    protected float _currentHP;

    protected virtual void Start()
    {
        _currentHP = _initialHP;
    }

    public virtual void ReceiveDamage(float damageReceived)
    {
        _currentHP -= damageReceived;

        if(_currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Min(0)]
    [SerializeField] protected float _initialHP;

    public bool IsAlive { get; private set; } = true;

    protected float _currentHP;

    protected virtual void Start()
    {
        _currentHP = _initialHP;
    }

    public virtual void ReceiveDamage(float damageReceived)
    {
        _currentHP -= damageReceived;

        if (_currentHP <= 0)
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}

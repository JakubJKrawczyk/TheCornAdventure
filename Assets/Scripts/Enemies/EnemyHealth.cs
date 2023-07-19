using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _initialHP;

    public bool IsAlive { get; private set; } = true;

    internal float _currentHP;

    private void Start()
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

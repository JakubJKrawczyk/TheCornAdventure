using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Min(0)]
    [SerializeField] protected float _initialHP;

    protected float _currentHP;

    protected virtual void Start()
    {
        _currentHP = _initialHP;
    }

    public virtual void ReceiveDamage(float damageReceived)
    {
        _currentHP -= damageReceived;
        Debug.Log("Player dealt " + damageReceived + " damage to the enemy.");

        if (_currentHP <= 0)
        {
            Debug.Log("Enemy has been defeated.");
            Destroy(gameObject);
        }
    }
}

/*public class EnemyHealth : MonoBehaviour
{
    [Min(0)]
    [SerializeField] protected float _initialHP;
    [Min(0)]
    [SerializeField] protected float _damageAmount;

    protected float _currentHP;

    protected virtual void Start()
    {
        _currentHP = _initialHP;
    }

    public virtual void ReceiveDamage(float damageReceived)
    {
        float actualDamage = _damageAmount / damageReceived;
        _currentHP -= actualDamage;

        if (_currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}


*/
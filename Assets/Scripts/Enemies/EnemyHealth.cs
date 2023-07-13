using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _initialHP;

    public float _currentHP;

    private void Start()
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

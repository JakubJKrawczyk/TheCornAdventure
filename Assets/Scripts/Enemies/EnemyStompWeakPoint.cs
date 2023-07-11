using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStompWeakPoint : MonoBehaviour
{
    [SerializeField] private EnemyHealth _enemyHealth;

    public void ReceiveDamage(float damageReceived)
    {
        _enemyHealth.ReceiveDamage(damageReceived);
    }
}

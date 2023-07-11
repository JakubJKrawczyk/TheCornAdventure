using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlamAttack : MonoBehaviour
{
    [SerializeField] private float _damage;

    private WeightController _weightController;
    private CharacterController2D _characterController;

    private void Start()
    {
        _weightController = GetComponent<WeightController>();
        _characterController = GetComponent<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent(out EnemyStompWeakPoint enemyStompWeakPoint))
        {
            enemyStompWeakPoint.ReceiveDamage(_damage);
        }
    }
}

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyStompWeakPoint enemyStompWeakPoint))
        {
            enemyStompWeakPoint.ReceiveDamage(_damage);
        }
    }
}

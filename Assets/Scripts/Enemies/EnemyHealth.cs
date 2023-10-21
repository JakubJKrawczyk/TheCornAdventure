using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _initialHP;

    public bool IsAlive { get; private set; } = true;

    internal float _currentHP;

    [SerializeField] private SpriteRenderer sprite;

    private void Start()
    {
        _currentHP = _initialHP;
    }

    public virtual void ReceiveDamage(float damageReceived)
    {
        _currentHP -= damageReceived;

        if (sprite != null)
        {
            sprite.color = Color.red;
            Invoke("ResetColor", 0.25f);
        }


        Debug.Log("E Damage taken: " + damageReceived + " left: " + _currentHP);

        if (_currentHP <= 0)
        {
            IsAlive = false;
            Destroy(gameObject);
        }

    }

    private void ResetColor()
    {
        sprite.color = Color.white;
    }
}

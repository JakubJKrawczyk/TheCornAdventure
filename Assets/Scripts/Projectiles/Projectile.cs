using Assets.Scripts.BasicClassExtensions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected ProjectileSO _projectileSO;
    [SerializeField] private Transform HitPrefab;

    private float _timeOnCreation;
    protected Vector3 _direction = new(1f, 0f, 0f);

    private void Start()
    {
        _timeOnCreation = Time.time;
        
    }

    protected virtual void Update()
    {
        CheckLifetime();
    }

    private void CheckLifetime()
    {
        if (Time.time > _timeOnCreation + _projectileSO.Lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_projectileSO.CollidesWith == (_projectileSO.CollidesWith | (1 << collider.gameObject.layer)))
        {
            if (collider.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.ReceiveDamage(_projectileSO.Damage);
                Debug.Log($"Przeciwnik otrzyma³ {_projectileSO.Damage} obra¿eñ i ma {enemyHealth._currentHP} ¿ycia");
            }

            
            //Wywo³anie efektu trafienia ziarna w miejscu trafienia
            Transform hit = Instantiate(HitPrefab, transform.position, Quaternion.identity);
            hit.GetComponent<Animator>().Play("GrainHitEffect");
            Destroy(hit.gameObject, 0.7f);

            Collider2D[] _enemies_hit = Physics2D.OverlapCircleAll(transform.position, 5, _projectileSO.EnemyMask);
            Debug.Log($"Znaleziono {_enemies_hit.Length} przeciwników");

            foreach (Collider2D enemy in _enemies_hit)
            {
                // Wywo³anie eksplozji w miejscu trafienia ziarna
                enemy.gameObject.GetComponent<Rigidbody2D>().AddExplosionForce(_projectileSO.KnockBackForce, transform.position, 10);
            }

            Destroy(gameObject);
        }
      
        
        
        if (collider.gameObject.layer == _projectileSO.EnemyMask) Destroy(gameObject);
        
        
    }
}

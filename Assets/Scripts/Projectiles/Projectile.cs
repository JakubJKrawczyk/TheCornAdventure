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
            }

            Destroy(gameObject);
        }

        if(collider.gameObject.layer == _projectileSO.EnemyMask) Destroy(gameObject);

        Transform hit = Instantiate(HitPrefab, transform.position, Quaternion.identity);
        hit.GetComponent<Animator>().Play("GrainHitEffect");
        Destroy(hit, 0.35f);
    }
}

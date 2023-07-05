using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected ProjectileSO _projectileSO;

    private float _timeOnCreation;
    protected Vector3 _direction = new Vector3(1f, 0f, 0f);

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
        if(Time.time > _timeOnCreation + _projectileSO.Lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }
}

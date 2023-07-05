using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected ProjectileSO _projectileSO;

    private float _timeOnCreation;

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
}

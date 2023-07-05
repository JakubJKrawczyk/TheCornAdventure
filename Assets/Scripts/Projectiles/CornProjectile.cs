using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornProjectile : Projectile
{
    [SerializeField] private float movementSpeed;

    protected override void Update()
    {
        base.Update();
        Move();
    }

    private void Move()
    {
        transform.position += _direction.normalized * movementSpeed * Time.deltaTime;
    }
}

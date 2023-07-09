using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornProjectile : Projectile
{
    protected override void Update()
    {
        base.Update();
        Move();
    }

    private void Move()
    {
        transform.position += _direction.normalized * _projectileSO.ProjectileSpeed * Time.deltaTime;
    }

}
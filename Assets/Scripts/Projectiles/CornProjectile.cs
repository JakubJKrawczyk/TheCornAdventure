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
        transform.position += _projectileSO.ProjectileSpeed * Time.deltaTime * _direction.normalized;
    }

}
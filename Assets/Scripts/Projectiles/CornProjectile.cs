using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornProjectile : Projectile
{
    [SerializeField] private float movementSpeed;

    private Vector3 movementDirection = new Vector3(1f, 0f, 0f);

    protected override void Update()
    {
        base.Update();
        Move();
    }

    private void Move()
    {
        transform.position += movementDirection.normalized * movementSpeed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private ProjectileSO _projectileSO;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private CharacterController2D controller;
    private float _timeOfLastShot = 0f;

    private void Update()
    {
        if(Input.GetKey(KeyCode.E) && !controller._isRolling)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if(Time.time > _timeOfLastShot + _projectileSO.TimeBetweenShots)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Transform projectileTransform =
            Instantiate(_projectileSO.Prefab, _shootingPoint.position, Quaternion.identity);

        Projectile projectile = projectileTransform.GetComponent<Projectile>();

        SetProjectileDirection(projectile);

        _timeOfLastShot = Time.time;
    }

    private void SetProjectileDirection(Projectile projectile)
    {
        if (transform.localScale.x < 0)
        {
            projectile.SetDirection(new Vector3(-1f, 0f, 0f));
        }
        else
        {
            projectile.SetDirection(new Vector3(1f, 0f, 0f));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private ProjectileSO _projectileSO;
    [SerializeField] private ProjectileSO _projectileSO2;
    [SerializeField] private Transform _shootingPoint;

    [SerializeField] private AmmoStorage AmmoStorage;

    private float _timeOfLastShot = 0f;
    private bool ShootingProgress = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if ((Time.time > _timeOfLastShot + _projectileSO.TimeBetweenShots) || (Time.time > _timeOfLastShot + _projectileSO2.TimeBetweenShots) && !ShootingProgress)
        {
            ShootingProgress = true;
            Shoot();
        }
    }

    private void Shoot()
    {
        int UsedAmmo = AmmoStorage.UseFirstAmmo();

        Transform projectileTransform = null;
        if (UsedAmmo == -1)
        {
            projectileTransform = Instantiate(_projectileSO.Prefab, _shootingPoint.position, Quaternion.identity);


        }
        else
        {
            projectileTransform = Instantiate(_projectileSO2.Prefab, _shootingPoint.position, Quaternion.identity);
        }
        Projectile projectile = projectileTransform.GetComponent<Projectile>();

        SetProjectileDirection(projectile);

        _timeOfLastShot = Time.time;
        ShootingProgress = false;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ProjectileSO projectileSO;
    [SerializeField] private ProjectileSO riceProjectileSO;
    [SerializeField] private Transform shootingPoint;

    //private script variables
    private AmmoStorage ammoStorage;
    private float _timeOfLastShot = 0f;
    private bool _shootingProgress = false;

    private void Start()
    {
        ammoStorage = GetComponent<AmmoStorage>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if ((Time.time > _timeOfLastShot + projectileSO.TimeBetweenShots) || (Time.time > _timeOfLastShot + riceProjectileSO.TimeBetweenShots) && !_shootingProgress)
        {
            _shootingProgress = true;
            Shoot();
        }
    }

    private void Shoot()
    {
        int UsedAmmo = ammoStorage.UseFirstAmmo();

        Transform projectileTransform = null;
        Projectile projectile = null;
        if (UsedAmmo == -1 || UsedAmmo == 0)
        {
            projectileTransform = Instantiate(projectileSO.Prefab, shootingPoint.position, Quaternion.identity);


        }
        else if (UsedAmmo == 1)
        {
            projectileTransform = Instantiate(riceProjectileSO.Prefab, shootingPoint.position, Quaternion.identity);
        }
        if (projectileTransform is not null)
        {
            projectile = projectileTransform.GetComponent<Projectile>();
        }
        if (projectile is not null) SetProjectileDirection(projectile);

        _timeOfLastShot = Time.time;
        _shootingProgress = false;
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

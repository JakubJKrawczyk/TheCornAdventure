using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootAttack : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ProjectileSO projectileSO;
    [SerializeField] private ProjectileSO riceProjectileSO;
    [SerializeField] private ProjectileSO cornProjectileSO;
    [SerializeField] private Transform shootingPoint;

    //private script variables
    private AmmoStorage ammoStorage;
    private float _timeOfLastShot = 0f;
    internal bool _shootingProgress = false;
    private float _timeToWait;
    private CharacterController2D _movement;
    private Animator _animator;

    private void Start()
    {
        ammoStorage = GetComponent<AmmoStorage>();
        _movement = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && !_movement._isRolling)
        {
            TryShoot();
            

        }

    }

    private void TryShoot()
    {
        if (Time.time > _timeToWait && !_shootingProgress)
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
        _timeOfLastShot = Time.time;

        if (UsedAmmo == -1 || UsedAmmo == 0)
        {
            projectileTransform = Instantiate(projectileSO.Prefab, shootingPoint.position, Quaternion.identity);
            _timeToWait = _timeOfLastShot + projectileSO.TimeBetweenShots;

        }
        else if (UsedAmmo == 1)
        {
            projectileTransform = Instantiate(riceProjectileSO.Prefab, shootingPoint.position, Quaternion.identity);
            _timeToWait = _timeOfLastShot + riceProjectileSO.TimeBetweenShots;
        }
        else if(UsedAmmo == 2)
        {
            projectileTransform = Instantiate(cornProjectileSO.Prefab, shootingPoint.position, Quaternion.identity);
            _timeToWait = _timeOfLastShot + cornProjectileSO.TimeBetweenShots;
        }
        if (projectileTransform is not null)
        {
            projectile = projectileTransform.GetComponent<Projectile>();
        }
        if (projectile is not null) SetProjectileDirection(projectile);

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

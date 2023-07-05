using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private ProjectileSO _projectileSO;
    [SerializeField] private Transform _shootingPoint;

    private float _timeOfLastShot = 0f;

    private void Update()
    {
        if(Input.GetKey(KeyCode.E))
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
        else
        {
            Debug.Log("Nope");
        }
    }

    private void Shoot()
    {
        Instantiate(_projectileSO.Prefab, _shootingPoint.position, Quaternion.identity);
        _timeOfLastShot = Time.time;
    }
}

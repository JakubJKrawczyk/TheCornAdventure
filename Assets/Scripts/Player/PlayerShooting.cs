using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private ProjectileSO _projectileSO;
    [SerializeField] private Transform _shootingPoint;

    private void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(_projectileSO.Prefab, _shootingPoint.position, Quaternion.identity);
    }
}

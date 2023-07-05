using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private Transform _cornPrefab;
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
        Instantiate(_cornPrefab, _shootingPoint.position, Quaternion.identity);
    }
}

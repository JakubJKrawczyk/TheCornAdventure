using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornProjectile : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Vector3 movementDirection = new Vector3(1f, 0f, 0f);

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += movementDirection.normalized * movementSpeed * Time.deltaTime;
    }
}

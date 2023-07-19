using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileBlock : MonoBehaviour
{
    public bool DestructibleByCorn = true;
    public int MinimumDamage = 20;
    [SerializeField] private GameObject ParticlesPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") && other.TryGetComponent(out Projectile projectile))
        {
            if (projectile.Damage >= MinimumDamage && DestructibleByCorn)
            {
                BlockDestroyed();
            }
        }
    }


    //todo: reference in SlamAttack
    public void BlockDestroyed()
    {
        gameObject.SetActive(false);

        GameObject hit = Instantiate(ParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(hit, 1f);
    }
}

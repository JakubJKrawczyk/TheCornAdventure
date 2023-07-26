using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForkProjectile : MonoBehaviour
{

    [SerializeField] public float force;   
    [SerializeField] public int damageAmount = 1;
    private float timer;
    private GameObject player;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Calculate the direction towards the player
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Apply the calculated direction and force to the fork projectile
        rb.velocity = direction * force;
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer>10)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            HealthController healthController = other.gameObject.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.RemoveHealth(damageAmount);
                Debug.Log("Damage dealt to the player");
            }
            Destroy(gameObject);
        }
    }
}

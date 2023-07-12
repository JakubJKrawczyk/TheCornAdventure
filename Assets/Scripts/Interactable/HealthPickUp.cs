using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [Range(-1, 5)] public int HealingAmount = 1;
    private float starty = 0;
    private float endy = 0;
    private float speed = 0.1f;
    private bool goingup = true;
    public bool ShouldFloat = true;
    private Rigidbody2D rb;

    private bool isPickedUp = false;

    void Start()
    {
        speed = Random.Range(0.2f, 0.4f);
        rb = GetComponent<Rigidbody2D>();
        if (ShouldFloat)
        {
            speed = 0.4f;
            starty = transform.position.y - 0.5f;
            endy = transform.position.y + 0.5f;
            rb.velocity = new Vector2(0, speed);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthController playerHealth = collision.gameObject.GetComponent<HealthController>();

            if (playerHealth != null)
            {
                bool healthAdded = playerHealth.AddHealth(HealingAmount); // Try to add health
                if (healthAdded)
                {
                    isPickedUp = true;
                    gameObject.SetActive(false); // Deactivate the health pickup object
                    Destroy(gameObject); // Destroy the health pickup object
                }
            }
        }
    }

    private void Update()
    {
        if (ShouldFloat)
        {
            if ((rb.position.y - endy >= -0.05) && goingup)
            {
                GoDown();
                goingup = false;
            }
            else if ((rb.position.y - starty <= 0.05) && !goingup)
            {
                GoUp();
                goingup = true;
            }
        }
    }
    void OnDisable()
    {

    }
    private void GoUp()
    {
        rb.velocity = new Vector2(0, speed);
    }
    private void GoDown()
    {
        rb.velocity = new Vector2(0, -speed);
    }
}
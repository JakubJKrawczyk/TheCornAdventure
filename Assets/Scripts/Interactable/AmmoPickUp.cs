using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private float starty = 0;
    private float endy = 0;
    private float speed = 0.1f;
    private bool goingup = true;
    public bool ShouldFloat = true;
    private Rigidbody2D rb;

    public int AmmoType = 0;
    [Range(0, 20)] public int AmmoAmount = 0;


    void Start()
    {
        speed = Random.Range(0.2f, 0.4f);
        rb = GetComponent<Rigidbody2D>();
        if (ShouldFloat)
        {
            starty = transform.position.y - 0.5f;
            endy = transform.position.y + 0.5f;
            rb.velocity = new Vector2(0, speed);
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

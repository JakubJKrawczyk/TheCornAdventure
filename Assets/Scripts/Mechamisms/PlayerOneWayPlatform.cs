using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private bool isColliding;
    [SerializeField] public PlatformEffector2D platform;
    [SerializeField] private LayerMask platformLayer;

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Ignore collisions between player and platform layers
                Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer.value, true);
                StartCoroutine(EnableCollisionAfterDelay());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == platformLayer)
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == platformLayer)
        {
            isColliding = false;
        }
    }

    public IEnumerator EnableCollisionAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);

        // Re-enable collisions between player and platform layers after a delay
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer.value, false);
        platform.surfaceArc = 140f;
    }
}

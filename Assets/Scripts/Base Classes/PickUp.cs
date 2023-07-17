using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Base_Classes
{
    public class PickUp: MonoBehaviour
    {
        protected float starty = 0;
        protected float startx = 0;
        protected float endy = 0;
        protected float speed = 0.4f;
        protected bool goingup = true;
        public bool ShouldFloat = true;
        protected Rigidbody2D rb;

        public int MinimumY = -5;
        protected int TimesReturned = 0;

        protected void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (ShouldFloat)
            {
                starty = transform.position.y - 0.5f;
                startx = transform.position.x;
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
            if (transform.position.y < MinimumY)
            {
                // Bring the pickup back to a correct position
                if (TimesReturned <= 3)
                {
                    Vector3 newPosition = GetValidPosition();
                    transform.position = newPosition;
                    TimesReturned++;
                }
                else // Returned more than 3 times - something's wrong
                {
                    transform.position = new Vector3(startx, starty, 0f);
                }
            }
        }

        protected void GoUp()
        {
            rb.velocity = new Vector2(0, speed);
        }
        protected void GoDown()
        {
            rb.velocity = new Vector2(0, -speed);
        }


        Vector3 GetValidPosition()
        {

            Vector3 raycastStartPosition = new Vector3(transform.position.x, -3f, transform.position.z);

            // Raycast to find ground
            RaycastHit2D hit = Physics2D.Raycast(raycastStartPosition, Vector2.down);

            if (hit.collider != null)
            {
                Vector3 newPosition = hit.point + new Vector2(0f, transform.localScale.y);
                return newPosition;
            }

            // no position found - return a default
            return new Vector3(startx, starty, 0f);
        }

    }
}

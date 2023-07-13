using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Base_Classes
{
    public class PickUp: MonoBehaviour
    {
        protected float starty = 0;
        protected float endy = 0;
        protected float speed = 0.4f;
        protected bool goingup = true;
        public bool ShouldFloat = true;
        protected Rigidbody2D rb;



        protected void Start()
        {
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

        protected void GoUp()
        {
            rb.velocity = new Vector2(0, speed);
        }
        protected void GoDown()
        {
            rb.velocity = new Vector2(0, -speed);
        }

    }

    
}

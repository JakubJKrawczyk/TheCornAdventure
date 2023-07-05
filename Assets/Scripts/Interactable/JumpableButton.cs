using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class JumpableButton : MonoBehaviour
{
    
    public bool CheckForVelocity = false;        // false = check for mass
    [Range(0, 10)] public float Timer = 0f;     // 0 = pressed will hold    0 < will disable after time

    [Range(0, 10)] public float minimumJumpVelocity = 2f;
    [Range(0, 25)] public float minimumMass = 0.75f;

    public GameObject[] Recievers;

    [SerializeField] GameObject Button;
    [SerializeField] GameObject ButtonCover;


    private bool isActive = false;
    private Color DefaultColor = new Color(0, 0, 0, 255);
    private void Start()
    {
        //set default color depending on button type
        if (Timer == 0)
        {
            DefaultColor = new Color(0, 0.5f, 0, 255);
        }
        else
        {

            DefaultColor = new Color(0.5f, 0.4f, 0, 255);
        }
        Button.GetComponent<SpriteRenderer>().color = DefaultColor;
    }

    private int isPressed = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has Rigidbody2D and if button isn't already active/pressed
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && !isActive && !ButtonCover.activeInHierarchy && isPressed == 0)
        {
            //what to check and if it's above mimimum
            if ((CheckForVelocity && rb.velocity.y >= minimumJumpVelocity) || (!CheckForVelocity && rb.mass >= minimumMass))
            {
                isActive = true;
                isPressed = 2;
                //move down - visual
                Button.transform.position = new Vector2(transform.position.x, transform.position.y - 0.125f);
                if (Timer == 0)
                {
                    Button.GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0, 255);
                }
                else
                {
                    Button.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 255);

                    //set speed to timer
                    float animationSpeed = 1f / Timer;
                    ButtonCover.GetComponent<Animator>().speed = animationSpeed - 0.0015f;
                }
                foreach (var reciever in Recievers)
                {
                    reciever.SendMessage("Switch");
                }

            }
        }
    }
    private void ResetButton()
    {
        if (isPressed == 0)
        {
            Button.GetComponent<SpriteRenderer>().color = DefaultColor;
            Button.transform.position = new Vector2(transform.position.x, transform.position.y + 0.125f);
            isActive = false;
            ButtonCover.SetActive(false);
            foreach (var reciever in Recievers)
            {
                reciever.SendMessage("Switch");
            }
        }
        else
        {
            Invoke("ResetButton", Timer);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Timer == 0)
        {
            isActive = false;
        }
        else
        {
            isPressed--;
            if (isPressed == 0)
            {
                ButtonCover.SetActive(true);
                Invoke("ResetButton", Timer);
            }
        }
    }
}

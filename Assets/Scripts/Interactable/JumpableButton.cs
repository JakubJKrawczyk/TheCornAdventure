using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpableButton : MonoBehaviour
{

    [Range(0, 10)] public float Timer = 0f;     // 0 = pressed will hold    0 < will disable after time

    [Range(0, 10)] public float minimumJumpVelocity = 2f;
    [Range(0, 25)] public float minimumMass = 0.75f;

    public UnityEvent OnInteractionEnabled;
    public UnityEvent OnInteractionDisabled;

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

        if (OnInteractionEnabled == null)
        {
            Debug.Log("No interaction assigned!");
        }
        if (Timer != 0 && OnInteractionDisabled == null)
        {
            Debug.Log("No disabled interaction assigned!");
        }
    }

    private bool isPressed = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the triggering object has Rigidbody2D and if the button isn't already active/pressed
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && !isActive && !ButtonCover.activeInHierarchy && !isPressed)
        {
            // Check velocity or mass depending on the condition
            if ((minimumJumpVelocity != 0 && rb.velocity.y >= minimumJumpVelocity) || (minimumMass != 0 && rb.mass >= minimumMass))
            {
                isActive = true;
                isPressed = true;

                // Move down - visual
                Button.transform.position = new Vector2(transform.position.x, transform.position.y - 0.125f);

                if (Timer == 0)
                {
                    Button.GetComponent<SpriteRenderer>().color = new Color(0, 1f, 0, 255);
                }
                else
                {
                    Button.GetComponent<SpriteRenderer>().color = new Color(1, 0.6f, 0, 255);

                    // Set speed to timer
                    float animationSpeed = 1f / Timer;
                    ButtonCover.GetComponent<Animator>().speed = animationSpeed - 0.0015f;
                }

                if (OnInteractionEnabled != null)
                {
                    OnInteractionEnabled.Invoke();
                }
            }
        }
    }

    public void ResetButton()
    {      
        if (!isPressed && isActive)
        {
            Button.GetComponent<SpriteRenderer>().color = DefaultColor;
            Button.transform.position = new Vector2(transform.position.x, transform.position.y + 0.125f);
            isActive = false;
            ButtonCover.SetActive(false);
            if (OnInteractionDisabled != null)
            {
                OnInteractionDisabled.Invoke();
            }
        }
        else
        {
            Invoke("ResetButton", Timer);
        }
    }

    private void OnTriggerExit2D()
    {
        if (Timer == 0)
        {
            isActive = false;
        }
        else
        {
            if (isPressed)
            {
                isPressed = false;
                ButtonCover.SetActive(true);
                Invoke("ResetButton", Timer);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [Header("Main Properties")]
    [SerializeField] private float activationDistance = 0.75f;
    
    [Header("Dependencies")]
    [SerializeField] private GameObject player;
    
    [Header("Events")]
    public UnityEvent OnInteractionEnabled;
    public UnityEvent OnInteractionDisabled;
    
    //private script variables
    private Animator leverAnimator;
    private bool Active = false;
    private bool isLeverUp = true;
    private GameObject InteractionHelpSquare;


    


    private void Start()
    {
        leverAnimator = GetComponent<Animator>();
        InteractionHelpSquare = transform.parent.Find("Square").gameObject;

        //check if events are null
        if (OnInteractionEnabled == null)
        {
            Debug.Log("No enabled interaction assigned!");
        }
        if (OnInteractionDisabled == null)
        {
            Debug.Log("No disabled interaction assigned!");
        }
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance <= activationDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {

                Active = !Active;
                if (Active)
                {
                    Activated();
                }
                else
                {
                    Deactivated();
                }

            }
            InteractionHelpSquare.SetActive(true);
        }
        else
        {
            InteractionHelpSquare.SetActive(false);
        }

        if (Active && isLeverUp)
        {
            LeverDown();
        }
        else if (!Active && !isLeverUp){
            LeverUp();
        }
    }
    public void Activated()
    {
        Active = true;
        if (OnInteractionEnabled != null)
        {
            OnInteractionEnabled.Invoke();
        }
    }

    public void Deactivated()
    {
        Active= false;
        if (OnInteractionDisabled != null)
        {
            OnInteractionDisabled.Invoke();
        }
    }

    private void LeverUp()
    {
        leverAnimator.SetTrigger("LeverMoveUp");
        isLeverUp= true;
    }
    private void LeverDown()
    {
        leverAnimator.SetTrigger("LeverMoveDown");
        isLeverUp= false;
    }
}

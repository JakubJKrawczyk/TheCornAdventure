using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

     private Animator animator;
    [SerializeField] private bool isGateUp;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(isGateUp )
        {
            OpenGate();
        }
        else
        {

            CloseGate();
        }
    }
    public void OpenGate()
    {
        animator.Play("Open", 0);
    }

    public void CloseGate()
    {
        animator.Play("Close", 0);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

     private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CloseGate();
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

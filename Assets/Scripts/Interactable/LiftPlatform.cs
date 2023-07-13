using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class LiftPlatform : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool isUp = true;
    [Header("Dependencies")]
    [SerializeField] private GameObject Player;

    //private script variables
    private Animator m_Animator;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void LiftDown()
    {
        if (isUp)
        {
            m_Animator.Play("LiftDown", 0);
            isUp = false;
            Debug.Log(isUp);
        }
    }

    public void LiftUp()
    {
        if(!isUp)
        {
            m_Animator.Play("LiftUp", 0);
            isUp = true;
            Debug.Log(isUp);

        }
    }

   
}

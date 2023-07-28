using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndLevel0Trigger : MonoBehaviour
{
    public UnityEvent OnTriggerColide;
    private void Awake()
    {
        OnTriggerColide ??= new UnityEvent();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {


        OnTriggerColide.Invoke();

        this.gameObject.SetActive(false);
    }
}

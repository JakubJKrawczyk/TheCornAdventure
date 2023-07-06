using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class SimpleLight : MonoBehaviour
{
    private bool IsActive = false;

    public UnityEvent OnInteractionEnabled;
    public UnityEvent OnInteractionDisabled;

    public void Switch()
    {
        if (IsActive)
        {
            TurnOFF();
        }
        else
        {
            TurnOn();
        }
    }

    public void TurnOFF()
    {
        GetComponent<Light2D>().enabled = false;
        IsActive = false;
    }
    public void TurnOn()
    {
        GetComponent<Light2D>().enabled = true;
        IsActive = true;
    }

}
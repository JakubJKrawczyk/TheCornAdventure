using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SimpleLight : MonoBehaviour
{
    private bool IsActive = false;

    public void Switch()
    {
        Debug.Log("SWITCH");
        if (IsActive)
        {
            GetComponent<Light2D>().enabled = false;
            IsActive = false;
        }
        else
        {
            GetComponent<Light2D>().enabled = true;
            IsActive = true;
        }
    }
}
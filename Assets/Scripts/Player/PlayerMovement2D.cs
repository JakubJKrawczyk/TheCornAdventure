using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{

    public CharacterController2D controller;


    private void FixedUpdate()
    {
        float XInput = Input.GetAxisRaw("Horizontal");
        float YInput = Input.GetAxisRaw("Vertical");
        bool Edown = Input.GetKeyDown(KeyCode.E);
        bool Fdown = Input.GetKeyDown(KeyCode.F);
        controller.Move(XInput, YInput == -1 ? true : false, YInput == 1 ? true : false);
        controller.IsWPressed = YInput == 1 ? true : false;
        Debug.Log($"YInput: {YInput}");

    }
}

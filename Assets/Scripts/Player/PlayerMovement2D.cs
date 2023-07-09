using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{

    public CharacterController2D controller;
    private Animator animator;
    private Rigidbody2D _rigidbody2D;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        float XInput = Input.GetAxisRaw("Horizontal");
        float YInput = Input.GetAxisRaw("Vertical");
        bool Edown = Input.GetKeyDown(KeyCode.E);
        bool Fdown = Input.GetKeyDown(KeyCode.F);
        controller.Move(XInput, YInput == -1 ? true : false, YInput == 1 ? true : false);
        controller.IsWPressed = YInput == 1 ? true : false;

        if (XInput != 0 && YInput <= 0) controller.isWalking = true;
        else controller.isWalking = false;

       // Debug.Log($"isRolling: {controller._isRolling}");
        // Obs³uga animacji
        if (controller.IsWPressed && _rigidbody2D.velocity.y > 1) animator.Play("player_jump_up", 0);
        else if (controller._jumpCount == 0 && _rigidbody2D.velocity.y < -1) animator.Play("player_jump_down", 0);
        else if (controller.isWalking && !controller._isRolling && !controller.isCrouching && !Input.GetKey(KeyCode.E)) animator.Play("player_walk", 0);
        else if (controller._isRolling) animator.Play("player_roll", 0);
        else if (Input.GetKey(KeyCode.E) && !controller.isCrouching) animator.Play("player_shoot", 0);
        else if (Input.GetKey(KeyCode.E) && controller.isCrouching) animator.Play("player_crouch_shoot", 0);
        else if (controller.isCrouching && !controller.isWalking && !controller._isRolling) animator.Play("player_crouch", 0);
        else if (controller.isCrouchWalking && controller.isWalking) animator.Play("player_crawl", 0);
        else animator.Play("Idle", 0);
        // koniec obs³ugi animacji

    }
}

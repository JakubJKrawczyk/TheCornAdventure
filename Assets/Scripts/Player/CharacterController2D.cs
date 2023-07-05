using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CharacterController2D : MonoBehaviour
{
    [Header("Basic Properties")]
    [SerializeField] private float _jumpForce = 400f;
    [Range(0,1)] [SerializeField] private float _crouchSpeed = .36f;
    [Range(0,.3f)] [SerializeField] private float _movementSmoothing = .05f;
    [Range(1f,15f)][SerializeField] private float _characterSpeed = 10f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Collider2D _standingCollider;

    private Rigidbody2D _rigidbody2D;
    private bool _facingRight = true;
    const float _groundedRadius = .2f;
    private bool _grounded;
    const float _ceilingRadius = .2f;
    private Vector3 _velocity = Vector3.zero;
    private float _jumpCount = 1;
    [Header("Events")]
    [Space]


    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool _wasCrouching = false;



    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null) OnLandEvent = new UnityEvent();
        if (OnCrouchEvent == null) OnCrouchEvent = new BoolEvent();
    }
    private void FixedUpdate()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        

        

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);

        foreach(Collider2D collider in colliders)
        {
            if(collider.gameObject != gameObject)
            {
                _grounded = true;
                if (!wasGrounded)
                {
                    _jumpCount = 1;
                    OnLandEvent.Invoke();
                    
                }
            }
        }

       

    }

    public void Move(float move, bool crouch, bool jump)
    {
        if(!crouch)
        {
            if(Physics2D.OverlapCircle(_ceilingCheck.position,_ceilingRadius, _whatIsGround))
            {
                crouch = true;
            }
        }

       
            if(crouch)
            {
                if (!_wasCrouching)
                {
                    _wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                move *= _crouchSpeed;

                if(_standingCollider != null)
                {
                    _standingCollider.enabled = false;
                }
            }
            else
            {
                if (_standingCollider != null)
                {
                    _standingCollider.enabled = true;
                }

                if (_wasCrouching)
                {
                    _wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            Vector3 targetVelocity = new Vector2(move * _characterSpeed, _rigidbody2D.velocity.y);

            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);

            if(move > 0 && !_facingRight)
            {
                Flip();
            }else if(move < 0 && _facingRight)
            {
                Flip();
            }

            if (jump && (_jumpCount > 0))
            {
                _grounded = false;
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
                _jumpCount--;
            }

        


    }

    public void CrouchAnimation(bool crouch)
    {
        Animator animator = GetComponent<Animator>();
        if(crouch)
        {
            animator.Play("PlayerCrouch", 0);
        }
        else
        {
            animator.Play("PlayerStandUp", 0);
        }
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.Events;
public class CharacterController2D : MonoBehaviour
{
    //props
    public bool IsWPressed { get; set; }
    [Header("Character Information")]
    [Range(0,100)] public float HP;
    

    [Header("Basic Properties")]
    [SerializeField] private float _jumpForce = 400f;
    [Range(0, 1)][SerializeField] private float _crouchSpeed = .36f;
    [Range(0, .3f)][SerializeField] private float _movementSmoothing = .05f;
    [Range(1f, 15f)][SerializeField] private float _characterSpeed = 5f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Collider2D _groundCheck;
    [SerializeField] private Collider2D _ceilingCheck;
    [SerializeField] private Collider2D _standingCollider;
    [SerializeField] private float _rollSpeed = 10f;
    [Range(0,2f)][SerializeField] private float _jumpWindow = 0.07f;
    private bool _triedRolling = false;
    private bool _isRolling = false;
    private float _actualCharacterSpeed;
    private float _jumpWindowTimer = 0f;
    private Rigidbody2D _rigidbody2D;
    private bool _facingRight = true;
    const float _groundedRadius = .1f;
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
        _actualCharacterSpeed = _characterSpeed;
    }
    private void FixedUpdate()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        if (!IsWPressed && _rigidbody2D.velocity.y > 4)
        {
            Debug.Log("Zmniejszanie pr�dko�ci");
            _rigidbody2D.velocity /= new Vector2(1f, 3f);
            
        }
        

         //obs�uga okienka skoku
        if(!_groundCheck.IsTouchingLayers(_whatIsGround.value)) {
            _jumpWindowTimer -= Time.deltaTime;
        }
        if(_jumpWindowTimer != _jumpWindow && _groundCheck.IsTouchingLayers(_whatIsGround.value)) _jumpWindowTimer = _jumpWindow;

        if(_jumpWindowTimer < 0) _jumpCount = 0;

        //Sprawdzam czy jestem na ziemi
        
            if(_groundCheck.IsTouchingLayers(_whatIsGround.value))
            {
                _grounded = true;
                if (!wasGrounded)
                {
                    _jumpCount = 1;
                    OnLandEvent.Invoke();
                    
                }
            }
        

       

    }

    /// <summary>
    /// Funkcja poruszaj�ca postaci�.
    /// </summary>
    /// <param name="move">Nadaje kierunek ruchu postaci; -1 w lewo; 1 w prawo; 0 st�j</param>
    /// <param name="crouch">Informuje kontroler, �e posta� chce kucn��</param>
    /// <param name="jump">Informuje kontroler, �e posta� chce podskoczy�</param>
    public void Move(float move, bool crouch, bool jump)
    {
        //sprawdzam czy nad g�ow� znajduje si� przeszkoda i nie pozwalam postaci wsta�
        if(!crouch)
        {
            if(_ceilingCheck.IsTouchingLayers(_whatIsGround.value))
            {
                crouch = true;
            }
        }

        // obs�uga kucania   
        if(crouch)
        {
            if (!_wasCrouching)
            {
                if (move != 0) _triedRolling = true; // je�eli podczas ruchu zacz�� kuca�

                _wasCrouching = true;
                OnCrouchEvent.Invoke(true);
            }

            if(!_isRolling) move *= _crouchSpeed;


            if (_standingCollider != null)
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
                _triedRolling = false;
            }
        }

        // nadanie pr�dko�ci postaci
        Vector3 targetVelocity = new Vector2(move * _actualCharacterSpeed, _rigidbody2D.velocity.y);

        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
         // Korekcja kierunku patrzenia postaci
        if(move > 0 && !_facingRight)
        {
            Flip();
        }else if(move < 0 && _facingRight)
        {
            Flip();
        }
         //Obs�uga skakania
        if (jump && (_jumpCount > 0))
        {
            _grounded = false;
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            _jumpCount--;
        }

        // Obs�uga toczenia si�
        if (_triedRolling && move == 0) _triedRolling = false; // je�eli w czasie toczenia pr�dko�� ruchu spad�a do 0

        if(move != 0 && crouch && _triedRolling)
        {
            Roll(true);
        }
        else
        {
            Roll(false);
        }
        if(_velocity.x >-0.5 && _velocity.x < 0.5) 
        {
            Roll(false);
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
    // funkcja pozwalaj�ca na toczenie postaci
    private void Roll(bool isRolling)
    {
        if (isRolling)
        {
            _isRolling = true;
            _actualCharacterSpeed = _rollSpeed;
        }
        else
        {
            _isRolling = false;
            _actualCharacterSpeed = _characterSpeed;
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

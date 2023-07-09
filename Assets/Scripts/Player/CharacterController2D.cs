using System;
using System.Collections.Generic;
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
    private Animator animator;
    private Rigidbody2D _rigidbody2D;
    private float _actualCharacterSpeed;
    private float _jumpWindowTimer = 0f;
    private bool _facingRight = true;
    private bool _grounded;
    private Vector3 _velocity = Vector3.zero;
    internal float _jumpCount = 1;

    //stany postaci

    internal bool _isRolling = false;
    internal bool isWalking = false;
    internal bool isCrouchWalking = false;
    internal bool isCrouching = false;

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
        animator = GetComponent<Animator>();

        if (OnLandEvent == null) OnLandEvent = new UnityEvent();
        if (OnCrouchEvent == null) OnCrouchEvent = new BoolEvent();
        _actualCharacterSpeed = _characterSpeed;
    }
    private void FixedUpdate()
    {
        bool wasGrounded = _grounded;
        _grounded = false;
         //obs³uga przetrzymanego skoku
        if (!IsWPressed && _rigidbody2D.velocity.y > 4)
        {
            _rigidbody2D.velocity /= new Vector2(1f, 3f);
            
        }
        

        //obs³uga okienka skoku
        if (!_groundCheck.IsTouchingLayers(_whatIsGround.value)) {
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
    /// Funkcja poruszaj¹ca postaci¹.
    /// </summary>
    /// <param name="move">Nadaje kierunek ruchu postaci; -1 w lewo; 1 w prawo; 0 stój</param>
    /// <param name="crouch">Informuje kontroler, ¿e postaæ chce kucn¹æ</param>
    /// <param name="jump">Informuje kontroler, ¿e postaæ chce podskoczyæ</param>
    public void Move(float move, bool crouch, bool jump)
    {
        //sprawdzam czy nad g³ow¹ znajduje siê przeszkoda i nie pozwalam postaci wstaæ
        if(!crouch)
        {
            if(_ceilingCheck.IsTouchingLayers(_whatIsGround.value))
            {
                crouch = true;
            }
        }

        // obs³uga kucania   
        if(crouch)
        {
            if (!_wasCrouching)
            {
                if (move != 0) _triedRolling = true; // je¿eli podczas ruchu zacz¹³ kucaæ

                _wasCrouching = true;
                isCrouching = true;
                OnCrouchEvent.Invoke(true);


                
            }

            if(!_isRolling) move *= _crouchSpeed;


            if (_standingCollider != null)
            {
                _standingCollider.enabled = false;
            }

            if (move != 0) isCrouchWalking = true;
            else isCrouchWalking = false;
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
                isCrouching = false;
                OnCrouchEvent.Invoke(false);
                _triedRolling = false;

                
            }
            if (move != 0) isWalking = true;
            else isWalking = false;
        }

        // nadanie prêdkoœci postaci
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
         //Obs³uga skakania
        if (jump && (_jumpCount > 0))
        {
            _grounded = false;
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            _jumpCount--;
        }

        // Obs³uga toczenia siê
        if (_triedRolling && move == 0) _triedRolling = false; // je¿eli w czasie toczenia prêdkoœæ ruchu spad³a do 0

        if(move != 0 && crouch && _triedRolling)
        {
            Roll(true);
        }
        else if(move == 0 || (_rigidbody2D.velocity.x > -1 || _rigidbody2D.velocity.x < 1))
        {
            Roll(false);
        }

        
    }

    
    // funkcja pozwalaj¹ca na toczenie postaci
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

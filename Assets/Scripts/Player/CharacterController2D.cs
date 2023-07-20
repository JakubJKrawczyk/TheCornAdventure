using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CharacterController2D : MonoBehaviour
{
    //props
    

    
    [Header("Basic Properties")]
    [SerializeField] private float _jumpForce = 400f;
    [Range(0, 1)][SerializeField] private float _crouchSpeed = .36f;
    [Range(0, .3f)][SerializeField] private float _movementSmoothing = .05f;
    [Range(1f, 15f)][SerializeField] private float _characterSpeed = 5f;
    [SerializeField] private float _rollSpeed = 10f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Collider2D _groundCheck;
    [SerializeField] private Collider2D _ceilingCheck;
    [SerializeField] private Collider2D _standingCollider;
    [Range(0,2f)][SerializeField] private float _jumpWindow = 0.025f;
    [SerializeField] private PlayerOneWayPlatform playerOneWayPlatform;


    //private script variables
    private bool _triedRolling = false;
    private Rigidbody2D _rigidbody2D;
    private float _actualCharacterSpeed;
    private float _jumpWindowTimer = 0f;
    private bool _facingRight = true;
    private bool _grounded;
    private Vector3 _velocity = Vector3.zero;
    internal float _jumpCount = 1;
    private bool _wasCrouching = false;
    //public script variables
    public bool IsWPressed { get; set; }
    public bool fallThrough;

     public PlayerStompAttack PlayerStompAttack;

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



    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
      //  PlayerStompAttack = GetComponent<PlayerStompAttack>();

        OnLandEvent ??= new UnityEvent();
        OnCrouchEvent ??= new BoolEvent();
        _actualCharacterSpeed = _characterSpeed;
    }
    private void FixedUpdate()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        //Sprawdzam czy jestem na ziemi
        CheckIfOnGround(wasGrounded);
       
        //Sprawdź czy jest w trakcie skoku
        CheckForJump();
            
       

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            fallThrough = true;
            playerOneWayPlatform = FindOneWayPlatform();
        }
        else
        {
            fallThrough = false;
            playerOneWayPlatform = null;
        }
    }
    /// <summary>
    /// Funkcja poruszająca postacią.
    /// </summary>
    /// <param name="move">Nadaje kierunek ruchu postaci; -1 w lewo; 1 w prawo; 0 stój</param>
    /// <param name="crouch">Informuje kontroler, że postać chce kucnąć</param>
    /// <param name="jump">Informuje kontroler, że postać chce podskoczyć</param>
    public void Move(float move, bool crouch, bool jump)
    {
        //sprawdzam czy nad głową znajduje się przeszkoda i nie pozwalam postaci wstać
        if(!crouch)
        {
            if(_ceilingCheck.IsTouchingLayers(_whatIsGround.value) && _jumpCount != 0)
            {
                crouch = true;
            }
        }

        // obsługa kucania   
        if(crouch)
        {
            if (!_wasCrouching)
            {
                if (move != 0) _triedRolling = true; // jeżeli podczas ruchu zaczął kucać

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

            if (playerOneWayPlatform != null && playerOneWayPlatform.IsPlayerOnPlatform() && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                playerOneWayPlatform.AllowFallThrough(true);
            }
            else
            {
                playerOneWayPlatform?.AllowFallThrough(false); 
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
                isCrouching = false;
                OnCrouchEvent.Invoke(false);
                _triedRolling = false;

                
            }
            if (move != 0) isWalking = true;
            else isWalking = false;

            playerOneWayPlatform?.AllowFallThrough(false);
        }

        // nadanie prędkości postaci
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

        //Obsługa skakania
        if (!_ceilingCheck.IsTouchingLayers(_whatIsGround.value))
        {
            Make_jump(jump);
        }

        // Obsługa toczenia się
        if (_triedRolling && move == 0) _triedRolling = false; // jeżeli w czasie toczenia prędkość ruchu spadła do 0

        if(move != 0 && crouch && _triedRolling)
        {
            Roll(true);
        }
        else if(move == 0 || (_rigidbody2D.velocity.x > -1 || _rigidbody2D.velocity.x < 1))
        {
            Roll(false);
        }

        
    }

    // funkcja pozwalająca na toczenie postaci
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

    private void CheckForJump()
    {
        
        //obsługa przetrzymanego skoku
        if (!IsWPressed && _rigidbody2D.velocity.y > 4)
        {
            _rigidbody2D.velocity /= new Vector2(1f, 3f);

        }


        //obsługa okienka skoku
        if (!_groundCheck.IsTouchingLayers(_whatIsGround.value))
        {
            _jumpWindowTimer -= Time.deltaTime;
        }
        if (_jumpWindowTimer != _jumpWindow && _groundCheck.IsTouchingLayers(_whatIsGround.value)) _jumpWindowTimer = _jumpWindow;

        if (_jumpWindowTimer < 0) _jumpCount = 0;
    }

    private void Make_jump(bool jump)
    {
        //Obsługa skakania
        if (jump && (_jumpCount > 0))
        {
            _grounded = false;
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            _jumpCount--;
        }
    }

    private void CheckIfOnGround(bool wasGrounded)
    {
        if (_groundCheck.IsTouchingLayers(_whatIsGround.value))
        {
            _grounded = true;
            if (!wasGrounded)
            {
                _jumpCount = 1;
                OnLandEvent.Invoke();

            }
        }
    }
    private PlayerOneWayPlatform FindOneWayPlatform()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_groundCheck.bounds.center, _groundCheck.bounds.size, 0f, _whatIsGround);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"))
            {
                return collider.gameObject.GetComponent<PlayerOneWayPlatform>();
            }
        }
        return null;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //passing velocity for Threshold
        //and collided object - can be later used for enemies or blocks to be destroyed
        PlayerStompAttack.Slam(collision.relativeVelocity.y, collision.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerStompAttack : MonoBehaviour
{
    [Header("Basic Properties")]
    [Space]
    [SerializeField] private float _damage;
    [Range(15, 30)] public int SlamThreshold = 20;


    [Header("Events")]
    [Space]
    public PlayerMovement2D _PlayerMovement2D;
    public GameObject SlamParticles;
    private WeightController _weightController;
    private CharacterController2D _characterController;


    private void Start()
    {
        _weightController = GetComponent<WeightController>();
        _characterController = GetComponent<CharacterController2D>();
      //  _PlayerMovement2D = GetComponent<PlayerMovement2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out EnemyStompWeakPoint enemyStompWeakPoint))
        {
            enemyStompWeakPoint.RelayDamage(_damage);
        }
    }

    private GameObject particles;


    //Called from CharacterController
    public void Slam(float speed, GameObject CollidedObject)
    {
        if (speed >= SlamThreshold)
        {
            bool CanSlam = _PlayerMovement2D.SlamAnimation(); //if true slam isn't being performed
            if (CanSlam) // Particles won't be spawned multiple times
            {
                particles = Instantiate(SlamParticles, transform.position + new Vector3(0f, 0f), Quaternion.identity);
                Invoke("DestroyParticles", 0.5f);
            }

            if (CollidedObject.tag == "Fragile")
            {
                CollidedObject.SetActive(false);
            }
        }
    }

    private void DestroyParticles()
    {
        Destroy(particles);
    }

}

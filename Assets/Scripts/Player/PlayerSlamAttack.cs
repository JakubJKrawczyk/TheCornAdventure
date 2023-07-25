using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.ParticleSystem;

public class PlayerStompAttack : MonoBehaviour
{
    [Header("Basic Properties")]
    [Space]
    public float MaxDamage = 50;
    [Range(15, 50)] public int SlamThreshold = 20; // minimum speed to begin slam
    [Range(0.5f, 10)] public float BasicSlamRadius = 2f;
    [Range(10, 150)] public int SlamPushForce = 100;

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
    }

    private GameObject particles;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out EnemyStompWeakPoint enemyStompWeakPoint))
        {
            enemyStompWeakPoint.RelayDamage(1000);
        }
    }

    //Called from CharacterController
    public void Slam(float speed, GameObject CollidedObject)
    {

        if (speed >= SlamThreshold)
        {
            bool CanSlam = _PlayerMovement2D.SlamAnimation();// Particles won't be spawned multiple times
            if (CanSlam)
            {
                particles = Instantiate(SlamParticles, transform.position + new Vector3(0f, 0f), Quaternion.identity);
                Invoke("DestroyParticles", 0.5f);

                if (CollidedObject.tag == "Fragile")
                {
                    CollidedObject.GetComponent<FragileBlock>().BlockDestroyed();
                    return;
                }


                // ============ Enemy damage handling ============


                float finalradius = BasicSlamRadius;

                if (speed > 35) // makes radius bigger if player is falling really fast
                {
                    finalradius += 0.75f;
                }

                // Check for objects with Enemy layer in radius
                Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, finalradius, LayerMask.GetMask("Enemy"));

                // Loop through found objects and apply damage if they have EnemyStompWeakPoint
                foreach (Collider2D enemyCollider in enemyColliders)
                {
                    EnemyStompWeakPoint enemyStompWeakPoint = enemyCollider.GetComponent<EnemyStompWeakPoint>();

                    if (enemyStompWeakPoint != null)
                    {
                        float distance = Vector2.Distance(transform.position, enemyCollider.transform.position);
                        float damageTEMP = speed;
                        float PushForceTEMP = SlamPushForce;

                        if (distance < finalradius * 0.5f) // damage dependend on how close is the enemy
                        {
                            damageTEMP = speed * 1.25f;
                        }
                        else
                        {
                            damageTEMP = speed * 0.8f;
                        }

                        if (speed > 35)
                        {
                            PushForceTEMP += 15;
                        }
                        Debug.Log("Found, distance: " + distance + " dealt damage: " + damageTEMP + " at speed: " + speed);
                        enemyStompWeakPoint.RelayDamage(damageTEMP);

                        // Get the parent GameObject (Enemy) and apply force to push away the enemy
                        GameObject enemyParent = enemyStompWeakPoint.transform.parent.gameObject;
                        Rigidbody2D enemyRigidbody = enemyParent.GetComponent<Rigidbody2D>();
                        if (enemyRigidbody != null)
                        {
                            Vector2 direction = (enemyParent.transform.position - transform.position).normalized;
                            enemyRigidbody.AddForce(direction * PushForceTEMP, ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }
    }


    private void OnDrawGizmos() // visualize radius
    {
        DrawOverlapCircle(transform.position, BasicSlamRadius);
        DrawOverlapCircleR(transform.position, BasicSlamRadius + 0.75f);
    }

    private void DrawOverlapCircle(Vector2 center, float radius)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, radius);
    }
    private void DrawOverlapCircleR(Vector2 center, float radius)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, radius);
    }

    private void DestroyParticles()
    {
        Destroy(particles);
    }

}

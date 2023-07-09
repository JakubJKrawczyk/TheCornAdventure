using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class ProjectileSO : ScriptableObject
{
    [SerializeField] private int _damage;
    public int Damage => _damage;

    [field: SerializeField] public Transform Prefab { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
    [field: SerializeField] public float TimeBetweenShots { get; private set; }
    [field: SerializeField] public float Lifetime { get; private set; }
    [field: SerializeField] public LayerMask CollidesWith { get; private set; }


}
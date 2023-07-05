using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ProjectileSO : ScriptableObject
{
    [field: SerializeField] public Transform Prefab { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public float ProjectileSpeed { get; private set; }
    [field: SerializeField] public float TimeBetweenShots { get; private set; }
    [field: SerializeField] public float Lifetime { get; private set; }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlamAttack : MonoBehaviour
{
    private WeightController _weightController;
    private CharacterController2D _characterController;

    private void Start()
    {
        _weightController = GetComponent<WeightController>();
        _characterController = GetComponent<CharacterController2D>();
    }
}

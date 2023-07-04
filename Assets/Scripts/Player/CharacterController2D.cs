using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0,1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0,.3f)] [SerializeField] private float m_MovementSmoothing = .05f;




}

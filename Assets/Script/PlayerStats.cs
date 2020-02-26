using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float Speed = 5;
    public float CrouchSpeed = 2;
    public float JumpForce = 5;
    public float RayCastLength = 0.3f;
    public float CeilingLength = 0.5f;

    public LayerMask Layers;

    [Header("Variables")]
    public bool CanMove = true;
    public bool Grounded;
    public bool Crouched;
    public Rigidbody2D Rb2d;
    public Animator Anim;

    [Header("Points")]
    public int Health = 5;
    public int Lives = 3;


}

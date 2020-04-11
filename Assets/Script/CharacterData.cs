using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour {

    [Header("References")]
    public Rigidbody2D rb2d;
    public SpriteRenderer Sprite;
    public Animator Anim;

    [Header("Options")]
    public bool CanMove = true;

    [Header("Vitality")]
    public int HealthMax;
    public int HealthCurrent;

    [Header("Stats")]
    public float Speed;
    public float CrouchSpeed;
    public float JumpForce;
    public bool Crouched;

    [Header("Ground Check")]
    public float GroundRayLength;
    public float CeilingLength;
    public LayerMask GroundLayers;

    [Header("Weapon")]
    public float ReloadSpeed;
    public float GunRadius = 0.5f;
    public GameObject Weapon;
    public GameObject Projectile;
    public LayerMask GunLayers;

    [Header("Projectile")]
    public float ProjectileSpeed;
    public float ProjectileDamage;
    public float ProjectileLifeTime;
    public float ProjectileGravity;
}

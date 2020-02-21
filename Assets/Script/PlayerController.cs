using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Stats
    [Header("Stats")]
    public float Speed = 5;
    public float JumpForce = 5;
    public float RayCastLength = 0.3f;

    public LayerMask Layers;

    //Variables
    private bool _canMove = true;
    private bool _grounded;
    private Rigidbody2D _rb2d;
    private Animator _anim;

    private void Awake(){
        _rb2d = GetComponent<Rigidbody2D>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update() {
        if (_canMove) {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }

        _grounded = CheckIfGrounded();
        PlayAnimations();
    }

    private void Movement(float hor, float ver) {
        //Moves left and right
        transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = Mathf.Abs(hor) > 0.1f ? hor < 0.1f : transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
        _rb2d.velocity = new Vector2(Speed * hor, _rb2d.velocity.y);

        //Stops if the player stops pressing stuff
        if(Mathf.Abs(hor) < 0.1f)
            _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);

        //Handles Jumping
        if(Input.GetButtonDown("Jump") && _grounded)
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, JumpForce);
    }

    //Plays the Animations
    private void PlayAnimations() {
        _anim.SetFloat("Speed", Mathf.Abs(_rb2d.velocity.x));
        _anim.SetBool("Grounded", _grounded);
    }

    private bool CheckIfGrounded() {
        return Physics2D.Raycast(transform.position, Vector2.down, RayCastLength, Layers);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    private PlayerStats _playerStats;
    private int _jumpCounter;

    private void Awake(){
        //Gets and Sets all the references
        _playerStats = GetComponent<PlayerStats>();
        _playerStats.Rb2d = GetComponent<Rigidbody2D>();
        _playerStats.Anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update() {
        //Moves the player if they are able to move
        if (_playerStats.CanMove) {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }

        //Sets grounded and plays animations
        _playerStats.Grounded = CheckIfGrounded();
        PlayAnimations();
    }

    //Handles movement
    private void Movement(float hor, float ver) {
        //Handles Crouching and adjusting speed
        Vector2 center = (Vector2)transform.position + new Vector2(0, (GetComponent<CircleCollider2D>().radius)) + GetComponent<CircleCollider2D>().offset;
        bool checkTop = Physics2D.Raycast(center, Vector2.up, _playerStats.CeilingLength, _playerStats.Layers);
        _playerStats.Crouched = ver < -0.1f && _playerStats.Grounded ? true : _playerStats.Grounded ? checkTop : false;
        GetComponent<CircleCollider2D>().enabled = _playerStats.Crouched;
        GetComponent<CapsuleCollider2D>().enabled = !_playerStats.Crouched;


        //Moves left and right
        transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = Mathf.Abs(hor) > 0.1f ? hor < 0.1f : transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
        _playerStats.Rb2d.velocity = new Vector2((_playerStats.Crouched ? _playerStats.CrouchSpeed : _playerStats.Speed) * hor, _playerStats.Rb2d.velocity.y);

        //Stops if the player stops pressing stuff
        if(Mathf.Abs(hor) < 0.1f)
            _playerStats.Rb2d.velocity = new Vector2(0, _playerStats.Rb2d.velocity.y);

        //Handles Jumping
        if(Input.GetButtonDown("Jump") && _playerStats.JumpTimes > _jumpCounter)
            _playerStats.Rb2d.velocity = new Vector2(_playerStats.Rb2d.velocity.x, _playerStats.JumpForce);
        if(_playerStats.Grounded) _jumpCounter = 0;
    }

    //Plays the Animations
    private void PlayAnimations() {
        _playerStats.Anim.SetFloat("Speed", Mathf.Abs(_playerStats.Rb2d.velocity.x));
        _playerStats.Anim.SetBool("Grounded", _playerStats.Grounded);
        _playerStats.Anim.SetBool("Crouch", _playerStats.Crouched);
    }

    //Checks to see if it's grounded or not
    private bool CheckIfGrounded() {
        return Physics2D.Raycast(transform.position, Vector2.down, _playerStats.RayCastLength, _playerStats.Layers);
    }
}

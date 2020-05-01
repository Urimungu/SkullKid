using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof (Movement))]
[RequireComponent(typeof (CharacterData))]
public class SkullKid : MonoBehaviour{

    //References
    private Movement _movement;
    private CharacterData _data;
    private GunController _gun;

    private int _jumpCounter;

    #region Properties
    private GunController Gun{
        get {
            //Creates the reference if it's missing
            if(_gun == null)
                _gun = Instantiate(_data.Weapon, transform.position, Quaternion.identity).GetComponent<GunController>();
            return _gun;
        }
    }
    #endregion

    private void Awake(){ 
        //Sets the References
        _movement = GetComponent<Movement>();
        _data = GetComponent<CharacterData>();
        _movement.SetData(_data);
        GameManager.Manager.Player = gameObject;
    }

    private void Update() {
        if (_data.CanMove) {
            //Handles Movement
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            _movement.Move(horizontal, vertical);

            //Handles Jumping
            if(Input.GetButtonDown("Jump") && _data.JumpTimes > _jumpCounter) {
                _jumpCounter++;
                _movement.Jump(); 
            }
            if(_movement.CheckIfGrounded()) 
                _jumpCounter = 0;

            //Controls the Gun
            Gun.FollowTarget(transform, _data);
            if(Input.GetKeyDown(KeyCode.Mouse0)) Gun.Shoot(gameObject, _data);
        }

        //Plays the animations for the player
        PlayAnimations();
    }

    //Plays the Animations
    private void PlayAnimations() {
        _data.Anim.SetFloat("Speed", Mathf.Abs(_data.rb2d.velocity.x));
        _data.Anim.SetBool("Grounded", _movement.CheckIfGrounded());
        _data.Anim.SetBool("Crouch", _data.Crouched);
    }
}

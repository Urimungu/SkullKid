using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Movement))]
[RequireComponent(typeof (CharacterData))]
public class SkullKid : MonoBehaviour{

    //References
    private Movement movement;
    private CharacterData _data;

    private void Awake(){
        //Sets the References
        movement = GetComponent<Movement>();
        _data = GetComponent<CharacterData>();
        movement.SetData(_data);
        GameManager.Manager.SetPlayer(gameObject);

        //Spawns in the Gun
        SpawnGun();
    }

    private void Update() {
        if (_data.CanMove) {
            movement.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if(Input.GetButtonDown("Jump"))
                movement.Jump();
        }

        PlayAnimations();
    }

    //Plays the Animations
    private void PlayAnimations() {
        _data.Anim.SetFloat("Speed", Mathf.Abs(_data.rb2d.velocity.x));
        _data.Anim.SetBool("Grounded", movement.CheckIfGrounded());
        _data.Anim.SetBool("Crouch", _data.Crouched);
    }

    //Spawns in the Gun
    private void SpawnGun() {
        GameObject gun = Instantiate(_data.Weapon, transform.position, Quaternion.identity);
        gun.GetComponent<GunController>().SetTarget(transform, _data);
    }
}

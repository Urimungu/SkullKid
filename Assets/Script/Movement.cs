using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour{

    //References
    private CharacterData _data;

    //Sets Data
    public void SetData(CharacterData data) { _data = data; }

    //Moves the Character
    public void Move(float hor, float ver) {
        //Crouching
        Crouch(ver);

        //Moves left and right
        _data.Sprite.flipX = Mathf.Abs(hor) > 0.1f ? hor < 0.1f : _data.Sprite.flipX;
        _data.rb2d.velocity = new Vector2((_data.Crouched ? _data.CrouchSpeed : _data.Speed) * hor, _data.rb2d.velocity.y);

        //Stops if the player stops pressing stuff
        if(Mathf.Abs(hor) < 0.1f) _data.rb2d.velocity = new Vector2(0, _data.rb2d.velocity.y);
    }

    //Jumps
    public void Jump() {
            _data.rb2d.velocity = new Vector2(_data.rb2d.velocity.x, _data.JumpForce);
    }

    //Crouches
    public void Crouch(float ver) {
        //Handles Crouching and adjusting speed
        Vector2 center = (Vector2)transform.position + new Vector2(0, (GetComponent<CircleCollider2D>().radius)) + GetComponent<CircleCollider2D>().offset;
        bool checkTop = Physics2D.Raycast(center, Vector2.up, _data.CeilingLength, _data.GroundLayers);
        _data.Crouched = ver < -0.1f && CheckIfGrounded() ? true : CheckIfGrounded() ? checkTop : false;
        GetComponent<CircleCollider2D>().enabled = _data.Crouched;
        GetComponent<CapsuleCollider2D>().enabled = !_data.Crouched;
    }

    //Checks to see if the player is on the ground by using a circle cast
    public bool CheckIfGrounded() {
        float radius = GetComponent<CircleCollider2D>().radius;
        bool grounded = Physics2D.CircleCast(transform.position, radius, Vector2.down, _data.GroundRayLength, _data.GroundLayers);
        return grounded;
    }
}

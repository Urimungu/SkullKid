using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{

    //Options
    private Transform _target;

    //Target Property
    public Transform Target {
        get {
            //If the target isn't present
            if(_target == null) {
                //Gets the player from the Game Manager
                if(GameManager.Manager.Player != null)
                    _target = GameManager.Manager.Player.transform;
                else
                    _target = null;
            }
            return _target;
        }
    }

    private void Update() {
        //Calls the function to move the player
        FollowPlayer(Target);
    }

    //Follows the player
    private void FollowPlayer(Transform target) {
        //If there is no target then break the function
        if(target == null) return;

        //Sets Refs
        Vector2 newPos = new Vector2();
        var settings = GameManager.Manager.Settings;

        //Movement
        newPos.x = Mathf.Lerp(target.position.x, transform.position.x, settings.CameraFollowSpeed);
        newPos.y = Mathf.Lerp(target.position.y + settings.CameraHeightOffset, transform.position.y, settings.CameraFollowSpeed);
        transform.position = new Vector3(newPos.x, newPos.y, -10);
    }
}

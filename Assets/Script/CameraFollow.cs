using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{

    //Options
    private Transform target;

    private void Update() {
        if(target != null) {
            FollowPlayer();
            return;
        }

        //Gets the Target if there is none
        target = GameManager.Manager.GetPlayer().transform;
    }

    //Follows the player
    private void FollowPlayer() {
        //Sets Refs
        Vector2 newPos = new Vector2();
        var settings = GameManager.Manager.GetSettings();

        //Movement
        newPos.x = Mathf.Lerp(target.position.x, transform.position.x, settings.CameraFollowSpeed);
        newPos.y = Mathf.Lerp(target.position.y + settings.CameraHeightOffset, transform.position.y, settings.CameraFollowSpeed);
        transform.position = new Vector3(newPos.x, newPos.y, -10);
    }
}

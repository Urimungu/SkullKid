using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed;

    private void Update() {
        transform.position = Vector3.Slerp(transform.position, target.position, FollowSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    //References
    public Transform Target;
    public Sprite Gun, GunShoot;
    public float ReloadSpeed = 0.4f;

    //Variables
    private const float HeightOffset = 0.4f;
    private const float Radius = 0.5f;

    private bool _canMove;
    private bool _isRunning;
    private float _timer;
    private Vector2 _center;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update() {
        FollowTarget(Target);

        if (!_isRunning && Time.time > _timer && Input.GetKeyDown(KeyCode.Mouse0))
        {
            _timer = Time.time + ReloadSpeed;
            _isRunning = true;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        _spriteRenderer.sprite = GunShoot;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.sprite = Gun;
        _isRunning = false;
    }

    private void FollowTarget(Transform target) {
        //Follows Player
        _center = (Vector2)target.position + (Vector2.up * HeightOffset);
        var camDistance = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _center).magnitude;
        var tempRadius = camDistance > Radius ? Radius : camDistance;
        transform.position = _center + ((Vector2)transform.right * tempRadius);

        //Flips the Sprite
        _spriteRenderer.flipY = Input.mousePosition.x < Camera.main.WorldToScreenPoint(_center).x;

        //Rotates to look at mouse
        Vector2 newPos = Input.mousePosition - Camera.main.WorldToScreenPoint(_center);
        float angle= Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

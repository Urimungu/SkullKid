using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    //References
    [Header("References")]
    public Transform Target;
    public Sprite Gun, GunShoot;
    public float ReloadSpeed = 0.4f;

    public LayerMask Layers;
    private GameObject _shotHolder;

    //Variables
    private float HeightOffset = 0.4f;
    private float Radius = 0.5f;

    private bool _canMove = true;
    private bool _isRunning;
    private float _timer;
    private Vector2 _center;
    private SpriteRenderer _spriteRenderer;
    private GameObject _bullet;

    private void Awake() {
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _bullet = Resources.Load<GameObject>("Projectiles/Bullet");
    }

    private void Update() {
        if(_canMove)
            FollowTarget(Target);

        HeightOffset = Target.GetComponent<CircleCollider2D>() != null && Target.GetComponent<CircleCollider2D>().enabled ? 0.2f : 0.4f;

        if (!_isRunning && Time.time > _timer && Input.GetKey(KeyCode.Mouse0)){
            _timer = Time.time + ReloadSpeed;
            _isRunning = true;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        SpawnBullet();
        _spriteRenderer.sprite = GunShoot;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.sprite = Gun;
        _isRunning = false;
    }

    //Spawns the Bullet in
    private void SpawnBullet() {
        //Spawns in a shot holder if one doesn't exist
        if(_shotHolder == null) {
            _shotHolder = new GameObject();
            _shotHolder.name = "ShotHolder";
        }

        //Spawns in the bullet
        GameObject shot = Instantiate(_bullet, transform.GetChild(_spriteRenderer.flipY ? 2 : 1).position, transform.rotation, _shotHolder.transform);
        shot.GetComponent<Bullet>().SpawnBullet(10, 10, 2, 0);
    }

    private void FollowTarget(Transform target) {
        //Follows Player
        _center = (Vector2)target.position + (Vector2.up * HeightOffset);
        var camDistance = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _center).magnitude;
        var tempRadius = camDistance > Radius ? Radius : camDistance;

        //Moves the gun back if there is something in the way
        Vector2 newDirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _center).normalized;
        var hit = Physics2D.BoxCast(_center, new Vector2(0.12f, 0.12f), 0, newDirection, Radius + 0.3f, Layers);
        transform.position = _center + ((Vector2)transform.right * (hit ? Mathf.Clamp(hit.distance - 0.2f, 0, 10) : tempRadius));

        //Flips the Sprite
        _spriteRenderer.flipY = Input.mousePosition.x < Camera.main.WorldToScreenPoint(_center).x;

        //Rotates to look at mouse
        Vector2 newPos = Input.mousePosition - Camera.main.WorldToScreenPoint(_center);
        float angle= Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

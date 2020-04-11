using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    //References
    private Transform _target;
    private CharacterData _data;
    private Animator _anim;

    //Variables
    private float _timer;

    //Sets the necessary information
    public void SetTarget(Transform target, CharacterData data) {
        _target = target;
        _data = data;
        _anim = transform.GetChild(0).GetComponent<Animator>();
    }

    //Controls the gun
    private void Update() {

        //Moves the gun
        FollowTarget(_target);

        //Shoots the gun and has a refresh rate
        if (Time.time > _timer && Input.GetKey(KeyCode.Mouse0)){
            _timer = Time.time + _data.ReloadSpeed;
            _anim.SetTrigger("Shoot");
            SpawnBullet();
        }
    }

    //Spawns the Bullet
    private void SpawnBullet() {
        //Creates a Projectile Holder if there is none
        if(GameManager.Manager.GetProjectileHolder() == null)
            GameManager.Manager.SetProjectileHolder();

        //Spawns in the bullet
        var _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        GameObject shot = Instantiate(_data.Projectile, transform.GetChild(_spriteRenderer.flipY ? 2 : 1).position, transform.rotation);
        shot.transform.parent = GameManager.Manager.GetProjectileHolder().transform;
        shot.GetComponent<Bullet>().SpawnBullet(_target.gameObject, _data);
    }

    //Follows the target
    private void FollowTarget(Transform target) {
        //Follows Player
        var HeightOffset = target.GetComponent<CircleCollider2D>() != null && target.GetComponent<CircleCollider2D>().enabled ? 0.2f : 0.4f;
        var center = (Vector2)target.position + (Vector2.up * HeightOffset);

        var camDistance = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - center).magnitude;
        var tempRadius = camDistance > _data.GunRadius ? _data.GunRadius : camDistance;

        //Moves the gun back if there is something in the way
        var newDirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - center).normalized;
        var hit = Physics2D.BoxCast(center, new Vector2(0.12f, 0.12f), 0, newDirection, _data.GunRadius + 0.3f, _data.GunLayers);
        transform.position = center + ((Vector2)transform.right * (hit ? Mathf.Clamp(hit.distance - 0.2f, 0, 10) : tempRadius));

        //Flips the Sprite
        transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = Input.mousePosition.x < Camera.main.WorldToScreenPoint(center).x;

        //Rotates to look at mouse
        Vector2 newPos = Input.mousePosition - Camera.main.WorldToScreenPoint(center);
        float angle= Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    //References
    private Animator _anim;

    //Variables
    private float _timer;

    //Sets the animator reference
    public void Start() {
        _anim = transform.GetChild(0).GetComponent<Animator>();
    }

    //Spawns the Bullet
    public void Shoot(GameObject target, CharacterData data) {
        //Refresh Rate
        if(Time.time < _timer) return;

        //Sets the variables
        _timer = Time.time + data.ReloadSpeed;
        _anim.SetTrigger("Shoot");

        //Spawns in the bullet
        var _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        GameObject shot = Instantiate(data.Projectile, transform.GetChild(_spriteRenderer.flipY ? 2 : 1).position, transform.rotation);
        shot.transform.parent = GameManager.Manager.ProjectileHolder.transform;
        shot.GetComponent<Bullet>().SpawnBullet(target.gameObject, data);
    }

    //Follows the target
    public void FollowTarget(Transform target, CharacterData data) {
        //Sets up the variables to start following the player
        var HeightOffset = target.GetComponent<CircleCollider2D>() != null && target.GetComponent<CircleCollider2D>().enabled ? 0.2f : 0.4f;
        var center = (Vector2)target.position + (Vector2.up * HeightOffset);

        var camDistance = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - center).magnitude;
        var tempRadius = camDistance > data.GunRadius ? data.GunRadius : camDistance;

        //Moves the gun back if there is something in the way
        var newDirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - center).normalized;
        var hit = Physics2D.BoxCast(center, new Vector2(0.12f, 0.12f), 0, newDirection, data.GunRadius + 0.3f, data.GunLayers);
        transform.position = center + ((Vector2)transform.right * (hit ? Mathf.Clamp(hit.distance - 0.2f, 0, 10) : tempRadius));

        //Flips the Sprite
        transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = Input.mousePosition.x < Camera.main.WorldToScreenPoint(center).x;

        //Rotates to look at mouse
        Vector2 newPos = Input.mousePosition - Camera.main.WorldToScreenPoint(center);
        float angle= Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Stats
    private float _speed;
    private float _damage;
    private float _lifeTime;
    private float _gravity;

    //Variables
    private bool _isRunning;
    private bool _hurtPlayer;

    //References
    private Rigidbody2D _rb2d;

    //Sets all of the values
    public void SpawnBullet(float speed, float damage, float lifeTime, float gravity, bool hurtPlayer = false) {
        //Sets the Values
        _speed = speed;
        _damage = damage;
        _lifeTime = lifeTime;
        _gravity = gravity;
        _hurtPlayer = hurtPlayer;

        _rb2d = GetComponent<Rigidbody2D>();

        if(!_isRunning) {
            _isRunning = true;
            StartCoroutine(LifeTime());
        }

        Movement();
    }

    //Destroys after specific amount of time
    IEnumerator LifeTime() {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    //Moves the bullet forward
    private void Movement() {
        _rb2d.velocity = (transform.right * _speed) + (Vector3.down * _gravity);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Hits the Player
        if(other.CompareTag("Player") && _hurtPlayer) {
            //Deals damage to the Player
            Destroy(gameObject);
            return;
        }

        if(!other.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

}

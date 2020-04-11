using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{
    //References
    private Rigidbody2D _rb2d;
    private CharacterData _data;
    private GameObject _shooter;    //The entity that shot the bullet

    //Variables
    private bool _isRunning;

    //Sets all of the values
    public void SpawnBullet(GameObject shooter, CharacterData data) {

        //Sets the references
        _data = data;
        _shooter = shooter;
        _rb2d = GetComponent<Rigidbody2D>();

        if(!_isRunning) {
            _isRunning = true;
            StartCoroutine(LifeTime());
        }

        //Moves the bullet forward
        _rb2d.velocity = (transform.right * _data.ProjectileSpeed) + (Vector3.down * _data.ProjectileGravity);
    }

    //Destroys after specific amount of time
    IEnumerator LifeTime() {
        yield return new WaitForSeconds(_data.ProjectileLifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Hits the Player
        if(other.CompareTag("Player") && other != _shooter) {
            //Deals damage to the Player
            Destroy(gameObject);
            return;
        }

        if(!other.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Settings))]
public class GameManager : MonoBehaviour {
    //Singleton Game Manager Reference
    public static GameManager Manager;

    [Header("Player")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Animator _camAnim;

    [Header("Shooting")]
    [SerializeField] private GameObject _projectileHolder;


    private void Awake() {
        //Singleton
        if(Manager == null) {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        ChangeMusic("LittleTalks");
    }
    //Functions
    #region Functions
        //Changes the song that is playing in the background
    public void ChangeMusic(string songName) {
        //If it couldn't find the specified song then return
        if(Resources.Load<AudioClip>("Music/" + songName) == null) return;

        //Gets the Audio from Resources and plays it
        GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Music/" + songName);
        GetComponent<AudioSource>().Play();
    }
    #endregion
    //Additional Properties
    #region Properites
    public GameObject Player { get => _player; set { _player = value; } }
    public GameObject Camera { get => _camera; set { _camera = value; } }
    public Animator CamAnim { get => _camAnim; set { _camAnim = value; } }
    public Settings Settings { get => GetComponent<Settings>(); }
    #endregion
    #region Full Properites
    public GameObject ProjectileHolder {
        get {
            //Creates a new Projectile Holder if there is none
            if(_projectileHolder == null)
                _projectileHolder = new GameObject("Projectile Holder");
            return _projectileHolder;
        }
    }
    #endregion
}

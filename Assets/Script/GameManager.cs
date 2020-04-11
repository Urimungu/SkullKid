using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Settings))]
public class GameManager : MonoBehaviour {
    public static GameManager Manager;

    [Header("Player")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject ProjectileHolder;
    [SerializeField] private Animator CamAnim;


    private void Awake() {
        //Singleton
        if(Manager == null) {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    //Get Functions
    public GameObject GetPlayer() { return Player;}
    public GameObject GetCamera() { return Camera; }
    public GameObject GetProjectileHolder() { return ProjectileHolder; }
    public Animator GetCamAnim() { return CamAnim; }
    public Settings GetSettings() { return GetComponent<Settings>(); }

    //Set functions
    public void SetPlayer(GameObject value) { Player = value; }
    public void SetCamera(GameObject value) { Camera = value; }
    public void SetCamAnim(Animator value) { CamAnim = value; }
    public void SetProjectileHolder() { ProjectileHolder = new GameObject("ProjectileHolder"); }
}

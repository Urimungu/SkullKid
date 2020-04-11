using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //References
    [SerializeField] private List<GameObject> Menus = new List<GameObject>();

    //Variables
    private int _state;

    private void Awake() {
        //Initializes
        SwitchState(0);
    }

    //Changes state
    public void SwitchState(int state) {
        _state = state;

        //Sets active the menu if it's in the right state
        for(int i = 0; i < Menus.Count; i++)
            Menus[i].SetActive(i == _state);
    }

    //Toggles the Fullscreen
    public void FullScreen(Toggle fullscren) {
        Screen.fullScreen = fullscren.isOn;
    }

    //Change the Resolution
    public void ChangeResolution(Dropdown dropdown) {
        //Initializes the Resolution
        int Width = Screen.width;
        int Height = Screen.height;

        //Chooses the Resolution
        switch(dropdown.value) {
            case 0: Width = 640;    Height = 480;   break;
            case 1: Width = 800;    Height = 600;   break;
            case 2: Width = 960;    Height = 720;   break;
            case 3: Width = 1024;   Height = 768;   break;
            case 4: Width = 1280;   Height = 960;   break;
            case 5: Width = 1400;   Height = 1050;  break;  
            case 6: Width = 1440;   Height = 1080;  break;
            case 7: Width = 1600;   Height = 1200;  break;
            case 8: Width = 1856;   Height = 1392;  break;
            case 9: Width = 1920;   Height = 1440;  break;
            case 10:    Width = 1920;   Height = 1080;  break;
            case 11:    Width = 2048;   Height = 1536;  break;
        }

        //Sets the Resolution
        Screen.SetResolution(Width, Height, Screen.fullScreen);
    }
}

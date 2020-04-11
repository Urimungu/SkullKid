using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    //References
    [SerializeField] private List<GameObject> Screens = new List<GameObject>();
    [SerializeField] private GameObject WarningMenu;

    //Variables
    private int _state;

    private void Awake() {
        //Starts the game at the press to begin
        SwitchState(0);
        CloseWarning();
    }

    private void FixedUpdate() {
        //Checks for starting the game
        if(_state == 0 && Input.anyKeyDown)
            SwitchState(1);
        
    }

    //Switches the State
    public void SwitchState(int newState) {
        //Sets the new state
        _state = newState;

        //Activates the Sceens that need to be activated
        for(int i = 0; i < Screens.Count; i++)
            Screens[i].SetActive(i == _state);
    }

    //Closes the game
    public void QuitGame() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }


    //Brings up a warning if something wrong happens
    public void OpenWarning(string header, string body, string optionOne = "Agree", string optionTwo = "") {
        //Turns everything on and Sets everything
        WarningMenu.SetActive(true);
        WarningMenu.transform.Find("Header").GetComponent<Text>().text = header;
        WarningMenu.transform.Find("Body").GetComponent<Text>().text = body;
        WarningMenu.transform.Find("OptionOne").GetChild(0).GetComponent<Text>().text = optionOne;

        //Turns off Option Two if it's not needed
        if(optionTwo != "") {
            WarningMenu.transform.Find("OptionTwo").gameObject.SetActive(true);
            WarningMenu.transform.Find("OptionTwo").GetChild(0).GetComponent<Text>().text = optionTwo;
        }
    }

    //Closes the Warning Pop-up
    public void CloseWarning() {
        WarningMenu.transform.Find("OptionTwo").gameObject.SetActive(false);
        WarningMenu.SetActive(false);

        //Removes listeners
        WarningMenu.transform.Find("OptionOne").GetComponent<Button>().onClick.RemoveAllListeners();
        WarningMenu.transform.Find("OptionTwo").GetComponent<Button>().onClick.RemoveAllListeners();
    }

    //Warnings
    public void NotAvailable(string message) {
        OpenWarning("Warning:", message, "Close");
        WarningMenu.transform.Find("OptionOne").GetComponent<Button>().onClick.AddListener(CloseWarning);
    }
}
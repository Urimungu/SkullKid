using UnityEngine.UI;
using UnityEngine;

public class KeyController : MonoBehaviour{
    //variables
    [SerializeField] private Text _text;
    [SerializeField] private string _message;

    private bool _inRange;

    private void FixedUpdate() {
        //If the player is in range then display the message to interact
        _text.text = _inRange ? _message : "";
    }

    //If the player has entered the trigger
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) _inRange = true;
    }

    //If the player has left the trigger
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) _inRange = false;
    }
}

using UnityEngine;

public class KeypadTrigger : MonoBehaviour
{
    public PasscodeSystem keypad;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            keypad.OpenPanel();
        }
    }
}
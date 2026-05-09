using UnityEngine;
using TMPro;

public class PasscodeSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    public Animator doorAnimator;
    public string correctCode = "1234";

    public void OpenPanel() {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f; // FREEZE THE GAME
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CheckCode() {
        if (inputField.text == correctCode) {
            Time.timeScale = 1f; // UNFREEZE
            doorAnimator.SetBool("IsOpen", true);
            ClosePanel();
        } else {
            inputField.text = ""; // Clear if wrong
        }
    }

    public void ClosePanel() {
        Time.timeScale = 1f;
        this.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
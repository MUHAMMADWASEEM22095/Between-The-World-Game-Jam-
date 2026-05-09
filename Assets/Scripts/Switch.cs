using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 offRotation = new Vector3(-90, 0, 0); // Initial
    public Vector3 onRotation = new Vector3(90, 0, 0);  // Toggled
    public float animationSpeed = 8f;

    private bool isSwitchedOn = false;
    private Quaternion targetRotation;

    void Start()
    {
        // Shuru mein switch -90 par hona chahiye
        transform.localRotation = Quaternion.Euler(offRotation);
        targetRotation = transform.localRotation;
    }

    void Update()
    {   
        // Smoothly rotate towards the target
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * animationSpeed);
    }

    public void ToggleSwitch()
    {
        // 1. Animation hamesha honi chahiye
        isSwitchedOn = !isSwitchedOn;
        targetRotation = isSwitchedOn ? Quaternion.Euler(onRotation) : Quaternion.Euler(offRotation);

        // 2. Result check tabhi hoga jab switch ON ho (90 degree par)
        if (isSwitchedOn)
        {
            // Check karein ke kya player ne saari wires (4) jor li hain?
            if (PuzzleManager.instance != null && PuzzleManager.instance.connections.Count == PuzzleManager.instance.totalWires)
            {
                Debug.Log("Sari wires juri hui hain. Result check kar rahe hain...");
                CheckFinalPuzzleResult();
            }
            else
            {
                // Agar wires poori nahi hain toh sirf animation hogi, kuch aur nahi
                Debug.Log("Abhi puzzle poora solve nahi hua, sirf switch move ho raha hai.");
            }
        }
    }

    void CheckFinalPuzzleResult()
    {
        if (PuzzleManager.instance != null)
        {
            // Pehle check karein ke kya player ne wires poori ki hain
            if (PuzzleManager.instance.connections.Count == PuzzleManager.instance.totalWires)
            {
                if (PuzzleManager.instance.IsPuzzleCorrect())
                {
                    Debug.Log("Success! System Online.");
                    // Lights ON ya Door Open logic yahan
                }
                else
                {
                    // Agar pattern ghalat hai toh PlayerInteraction ka reset chalayein
                    PlayerInteraction player = FindObjectOfType<PlayerInteraction>();
                    if (player != null)
                    {
                        player.ResetGame();
                    }
                }
            }
        }
    }
}

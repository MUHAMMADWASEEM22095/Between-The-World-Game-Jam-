using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    public Transform wireContainer; // Isay Transform rakhein taake loop asani se chale

    // Key = Left Color, Value = Right Color
    public Dictionary<string, string> connections = new Dictionary<string, string>();
    public int totalWires = 4;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void RegisterConnection(string leftColor, string rightColor)
    {
        if (connections.ContainsKey(leftColor))
            connections[leftColor] = rightColor;
        else
            connections.Add(leftColor, rightColor);

        Debug.Log("Current Connections Count: " + connections.Count);
    }

    public bool IsPuzzleCorrect()
    {
        if (connections.Count < totalWires) return false;

        // Pattern Check: Red->Blue, Green->Red, Blue->Yellow, Yellow->Green
        bool isRedCorrect = GetTarget("Red") == "Blue";
        bool isGreenCorrect = GetTarget("Green") == "Red";
        bool isBlueCorrect = GetTarget("Blue") == "Yellow";
        bool isYellowCorrect = GetTarget("Yellow") == "Green";

        return isRedCorrect && isGreenCorrect && isBlueCorrect && isYellowCorrect;
    }

    private string GetTarget(string leftColor)
    {
        if (connections.TryGetValue(leftColor, out string target))
            return target;
        return "";
    }

    public void ResetConnections()
    {
        // 1. Dictionary saaf karein
        connections.Clear();
        Debug.Log("Connections Cleared in Dictionary.");

        // 2. Visual Wires delete karein
        if (wireContainer != null)
        {
            foreach (Transform child in wireContainer)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("All Wire Objects Destroyed.");
        }
        else
        {
            Debug.LogError("WireContainer reference missing in Inspector!");
        }

        // 3. Sab buttons ko dobara clickable banayein
        ResetButtonsState();
    }

    void ResetButtonsState()
    {
        WireTask[] allTasks = FindObjectsOfType<WireTask>();
        foreach (WireTask task in allTasks)
        {
            task.isMatched = false;
        }
    }
}
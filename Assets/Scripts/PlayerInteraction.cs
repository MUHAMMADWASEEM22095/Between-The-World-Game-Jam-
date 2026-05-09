using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public PlayerInventory inventory;

    public Transform playerCamera;
    private StarterAssetsInputs _inputs;

    [Header("Gridbox Settings")]
    public GameObject BoxLid;
    public float openAngle = -90f;
    public float rotationSpeed = 2f;

    private bool isOpen = false;
    private bool isAnimating = false;
    private Quaternion closedRotation;

    [Header("UI Settings")]
    public Image reticle;
    private Color interactColor = Color.red;
    private Color normalColor = Color.white;
    public GameObject interactPanel;

    [Header("Circuit Puzzle Settings")]
    public GameObject inspectButton;
    public GameObject circuitPuzzlePanel;

    private void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
        if (BoxLid != null)
        {
            closedRotation = BoxLid.transform.localRotation;
        }
        interactPanel.SetActive(false);
        circuitPuzzlePanel.SetActive(false);
        inspectButton.SetActive(false);
    }
    void Update()
    {
        UpdateReticle();

        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformInteraction();
        }

        if(Input.GetKeyDown(KeyCode.E) && inspectButton.activeSelf)
        {
            OnInspectButtonClicked();
        }
    }

    void UpdateReticle()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Key") || hit.collider.CompareTag("Lock") || hit.collider.CompareTag("Gridbox") || hit.collider.CompareTag("Switch"))
            {
                interactPanel.SetActive(true);
                reticle.color = interactColor;
            }
            else
            {
                interactPanel.SetActive(false);
                reticle.color = normalColor;
            }
        }
        else
        {
            interactPanel.SetActive(false);
            reticle.color = normalColor;
        }
    }

    void PerformInteraction()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            Debug.Log("Interacted with: " + hit.collider.name);
            if (hit.collider.CompareTag("Key"))
            {
                inventory.hasCircuitKey = true;
                Destroy(hit.collider.gameObject);
                Debug.Log("Key Picked Up!");
            }

            if (hit.collider.gameObject.name.StartsWith("lock"))
            {
                if (inventory.hasCircuitKey)
                {
                    inventory.isLockOpen = true;
                    Debug.Log("Opening Lock...");
                }
            }

            if (hit.collider.CompareTag("Gridbox"))
            {
                if (inventory.isLockOpen && !isAnimating)
                {
                    ToggleGridbox();
                }
            }
            if (hit.collider.CompareTag("Switch"))
            {
                Switch handle = hit.collider.GetComponentInChildren<Switch>();

                if (handle != null)
                {
                    handle.ToggleSwitch();
                    Debug.Log("Switch Toggled via PlayerInteraction!");
                }
                else
                {
                    Debug.LogError("SwitchHandle script switch object par nahi mili!");
                }
            }
        }
        
    }
    IEnumerator AnimateLid(Quaternion targetRotation)
    {
        isAnimating = true;
        Quaternion startRotation = BoxLid.transform.localRotation;

        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime * rotationSpeed;
            BoxLid.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, time);
            yield return null;
        }

        BoxLid.transform.localRotation = targetRotation;
        isAnimating = false;

        Debug.Log(isOpen ? "Gridbox Opened!" : "Gridbox Closed!");
    }

    void ToggleGridbox()
    {
        isOpen = !isOpen;
        Quaternion targetRot = isOpen ? Quaternion.Euler(openAngle, 0, 0) * closedRotation : closedRotation;
        inspectButton.SetActive(isOpen);
        if (!isOpen) circuitPuzzlePanel.SetActive(false);

        StartCoroutine(AnimateLid(targetRot));
    }
    public void OnInspectButtonClicked()
    {
        circuitPuzzlePanel.SetActive(true);
        inspectButton.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (GetComponent<StarterAssets.FirstPersonController>() != null)
        {
            GetComponent<StarterAssets.FirstPersonController>().enabled = false;
        }
        _inputs.cursorLocked = false;
        _inputs.cursorInputForLook = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<FirstPersonController>().enabled = false;
    }
    public void ClosePuzzlePanel()
    {
        circuitPuzzlePanel.SetActive(false);
        inspectButton.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (GetComponent<StarterAssets.FirstPersonController>() != null)
        {
            GetComponent<StarterAssets.FirstPersonController>().enabled = true;
        }
        if (GetComponent<UnityEngine.InputSystem.PlayerInput>() != null)
        {
            GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
        }
        _inputs.cursorLocked = true;
        _inputs.cursorInputForLook = true;
    }

    public void ResetGame()
    {
        //inventory.hasCircuitKey = false;
        Debug.Log("Resetting Game due to wrong wiring...");
        PuzzleManager.instance.ResetConnections();

    }

    private void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawRay(playerCamera.position, playerCamera.forward * interactRange);

            Gizmos.DrawWireSphere(playerCamera.position + (playerCamera.forward * interactRange), 0.1f);
        }
    }
}


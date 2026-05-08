using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public PlayerInventory inventory;

    public Transform playerCamera;

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

    private void Start()
    {
        if (BoxLid != null)
        {
            closedRotation = BoxLid.transform.localRotation;
        }
        interactPanel.SetActive(false);
    }
    void Update()
    {
        UpdateReticle();

        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformInteraction();
        }
    }

    void UpdateReticle()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Key") || hit.collider.CompareTag("Lock") || hit.collider.CompareTag("Gridbox"))
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

        StartCoroutine(AnimateLid(targetRot));
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


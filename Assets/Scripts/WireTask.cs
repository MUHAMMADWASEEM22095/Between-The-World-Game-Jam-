using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireTask : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Settings")]
    public string wireColor; // Is node ka color (e.g. "Red")
    public Image wireImagePrefab; // UI Image jo line banayegi
    public RectTransform canvasRect; // Puzzle Panel ka RectTransform

    private Image currentWire;
    private Vector2 startPoint;
    public bool isMatched = false;

    [Header("Fine Tuning")]
    public float rightEdgeOffset = 90f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isMatched) return;

        currentWire = Instantiate(wireImagePrefab, canvasRect);
        currentWire.color = GetComponent<Image>().color;

        RectTransform buttonRect = GetComponent<RectTransform>();

        // Manual Offset ke sath Extreme Right position
        // Width/2 ke sath hum rightEdgeOffset jama (add) kar rahe hain
        Vector3 rightEdgePos = transform.position + (transform.right * ((buttonRect.rect.width / 2f) + rightEdgeOffset));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            rightEdgePos,
            eventData.pressEventCamera,
            out startPoint
        );

        currentWire.rectTransform.anchoredPosition = startPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isMatched || currentWire == null) return;

        Vector2 localMousePos;
        // Mouse position ko local space mein convert karein
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out localMousePos
        );

        UpdateWire(localMousePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Mouse Released!"); // 1. Check karein kya ye aata hai?

        if (currentWire == null) return;

        // Check karein ke mouse ke neeche kya hai
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

        if (hitObject != null)
        {
            Debug.Log("Mouse is over: " + hitObject.name + " with Tag: " + hitObject.tag); // 2. Iska naam kya hai?

            if (hitObject.CompareTag("RightNode"))
            {
                WireNode targetNode = hitObject.GetComponent<WireNode>();
                if (targetNode != null)
                {
                    // Snap aur logic yahan...
                    isMatched = true;
                    PuzzleManager.instance.RegisterConnection(wireColor, targetNode.colorName);
                    return;
                }
                else
                {
                    Debug.LogError("Object has RightNode tag but NO WireNode script!");
                }
            }
        }
        else
        {
            Debug.Log("Mouse is over NOTHING (Null)");
        }

        Destroy(currentWire.gameObject);
    }

    void UpdateWire(Vector2 targetPos)
    {
        if (currentWire == null) return;

        Vector2 direction = targetPos - startPoint;
        float distance = direction.magnitude;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        currentWire.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
        currentWire.rectTransform.sizeDelta = new Vector2(distance, 20f);

        // Yeh line ensure karti hai ke wire hamesha button ke kinaray se juri rahe
        currentWire.rectTransform.anchoredPosition = startPoint;
    }
}

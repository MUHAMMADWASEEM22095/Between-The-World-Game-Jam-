using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public PlayerInventory inventory;
    private Rigidbody rb;
    private bool hasFallen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;
    }
    void Update()
    {
        if (inventory.isLockOpen && !hasFallen)
        {
            OpenLock();
        }
    }
    void OpenLock()
    {
        if (rb != null)
        {
            gameObject.tag = "Untagged";
            rb.useGravity = true;
            rb.isKinematic = false;
            hasFallen = true;
            Debug.Log("Lock is now open and falling!");
        }
    }
}

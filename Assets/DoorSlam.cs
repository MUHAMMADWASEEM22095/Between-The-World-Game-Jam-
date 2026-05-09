using UnityEngine;

public class DoorSlam : MonoBehaviour
{
    public Animator doorAnimator; 

    // When you enter the box
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("IsNear", true);
            Debug.Log("Player is near! Door closing.");
        }
    }

    // When you walk out of the box
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimator.SetBool("IsNear", false);
            Debug.Log("Player left! Door opening.");
        }
    }
}
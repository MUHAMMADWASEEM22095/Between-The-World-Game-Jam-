using UnityEngine;

public class Infinite_Loop : MonoBehaviour
{
    public Transform resetTransform; 
    private CharacterController controller;
    private StarterAssets.FirstPersonController fpsController; // Reference to the movement script

    void Start()
    {
        controller = GetComponent<CharacterController>();
        fpsController = GetComponent<StarterAssets.FirstPersonController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blocker"))
        {
            if (resetTransform != null && controller != null)
            {
                // 1. Disable controller to allow position change
                controller.enabled = false;

                // 2. Zero out the velocity in CharacterController
                // This clears any momentum/inertia
                controller.Move(Vector3.zero);

                // 3. Reset FPS controller internal velocity if available
                if(fpsController != null) {
                    // Reset gravity/fall speed to prevent momentum carryover
                    // This uses reflection to clear the _verticalVelocity field
                    var verticalVelocityField = typeof(StarterAssets.FirstPersonController)
                        .GetField("_verticalVelocity", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if(verticalVelocityField != null) {
                        verticalVelocityField.SetValue(fpsController, 0f);
                    }
                }

                // 4. Teleport player
                transform.position = resetTransform.position;
                // Optional: Also reset rotation for consistency
                // transform.rotation = resetTransform.rotation;

                // 5. Re-enable controller
                controller.enabled = true;
                
                Debug.Log("Seamless Teleport Complete - Position: " + resetTransform.position);
            }
            else
            {
                Debug.LogWarning("Missing resetTransform or CharacterController!");
            }
        }
    }
}
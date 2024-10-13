using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float baseSpeed = 2f; // Base upward speed
    public float speedIncreaseRate = 2f; // Rate at which the camera speeds up when the player is near the top
    public float maxSpeed = 10f; // Maximum speed for the camera
    public float topOffset = 2f; // Offset to determine when to speed up the camera
    public float smoothTime = 0.3f; // Time to smooth the camera's movement

    private float currentSpeed; // Current speed of the camera
    private bool isCameraMoving = false; // Flag to check if the camera should move
    private bool isSpeedModified = false; // Flag to check if speed modification is active
    private Vector3 velocity = Vector3.zero; // Velocity used by SmoothDamp
    private float modifiedSpeedFactor = 1f; // Store the speed factor when a power-up is collected

    void Start()
    {
        // Initialize the current speed
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (isCameraMoving)
        {
            // Move the camera upwards at the current speed
            transform.position += new Vector3(0, currentSpeed * Time.deltaTime, 0);

            // Ensure the player stays within the camera's view
            KeepPlayerInView();
        }
    }

    // Public method to start moving the camera
    public void StartMovingCamera()
    {
        // Start the camera movement
        isCameraMoving = true;
    }

    // Check if the camera is moving
    public bool IsCameraMoving()
    {
        // Return the value of isCameraMoving
        return isCameraMoving;
    }

    // Public method to modify the camera's speed by a factor (e.g., from power-up)
    public void ModifySpeed(float speedFactor)
    {
        // Modify the current speed by the given factor
        currentSpeed *= speedFactor;
        // Store the speed factor for later use
        modifiedSpeedFactor = speedFactor;
        // Track if speed is modified
        isSpeedModified = speedFactor != 1f;
    }

    // Public method to reset the speed modification flag when power-up ends
    public void ResetSpeedModification()
    {
        // Clear the speed modification flag
        isSpeedModified = false;
        // Reset the modified speed factor
        modifiedSpeedFactor = 1f;
    }

    // Ensure the player stays in view
    private void KeepPlayerInView()
    {
        float playerYPosition = player.position.y;
        float topScreenY = Camera.main.transform.position.y + Camera.main.orthographicSize;

        // If the player is close to the top or above the camera, adjust the camera speed
        if (playerYPosition > topScreenY - topOffset)
        {
            // Use SmoothDamp to smoothly move the camera towards the player's position without choppiness
            Vector3 targetPosition = new Vector3(transform.position.x, player.position.y + topOffset, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // Increase the camera's current speed to catch up to the player
            currentSpeed = Mathf.Clamp(currentSpeed + (speedIncreaseRate * Time.deltaTime), baseSpeed, maxSpeed);
        }
        else
        {
            // Once the camera catches up, return to the modified speed (half speed during power-up)
            if (isSpeedModified)
            {
                // Return to the modified speed
                currentSpeed = baseSpeed * modifiedSpeedFactor;
            }
            else
            {
                // Reset to base speed if no power-up is active
                currentSpeed = baseSpeed;
            }
        }
    }
}

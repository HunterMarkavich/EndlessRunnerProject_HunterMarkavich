using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera

    private void Start()
    {
        if (mainCamera == null)
        {
            // If the camera isn't assigned in the Inspector, find the main camera
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Update the background position to match the camera's position
        Vector3 newPosition = new Vector3(transform.position.x, mainCamera.transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
}

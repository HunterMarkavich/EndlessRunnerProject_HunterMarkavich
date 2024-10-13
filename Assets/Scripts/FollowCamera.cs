using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Reference to the camera's transform
    public Transform cameraTransform;

    void Start()
    {
        if (cameraTransform == null)
        {
            // Find the main camera if not assigned
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Update the Y position of the wall to match the camera's Y position, keeping the X and Z positions fixed
        transform.position = new Vector3(transform.position.x, cameraTransform.position.y, transform.position.z);
    }
}

using UnityEngine;

public class ColumnFollowCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private float yOffset;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        // Calculate initial offset
        yOffset = transform.position.y - cameraTransform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, cameraTransform.position.y + yOffset, transform.position.z);
    }
}

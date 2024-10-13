using UnityEngine;

public class OrbitalDrone : MonoBehaviour
{
    public float orbitRadius = 2f; // Radius of the circular movement
    public float orbitSpeed = 50f; // Speed of the circular movement (degrees per second)

    private Vector3 centerPoint; // The center point of the drone's circular path
    private float angle; // Current angle of the drone around the center
    private Camera mainCamera; // Reference to the main camera
    private bool hasLogged = false; // Track if the spawn log has been printed

    void Start()
    {
        // Set the initial center point as the starting position of the drone
        centerPoint = transform.position;

        // Get the reference to the main camera
        mainCamera = Camera.main;

        // Log when the drone is spawned
        Debug.Log("Drone spawned at: " + transform.position);
        // Mark that the log has been printed
        hasLogged = true;
    }

    void Update()
    {
        // Increment the angle to make the drone move in a circular path
        angle += orbitSpeed * Time.deltaTime;

        // Calculate the new position in a circular trajectory
        float radianAngle = angle * Mathf.Deg2Rad;
        float offsetX = Mathf.Cos(radianAngle) * orbitRadius;
        float offsetY = Mathf.Sin(radianAngle) * orbitRadius;

        // Update the drone's position to form the circular motion around the center point
        transform.position = new Vector3(centerPoint.x + offsetX, centerPoint.y + offsetY, transform.position.z);

        // Check if the drone is below the camera view
        float despawnY = mainCamera.transform.position.y - mainCamera.orthographicSize;
        if (transform.position.y < despawnY)
        {
            // Log when the drone is despawned
            Debug.Log("Drone despawned at: " + transform.position);

            // Destroy the drone game object
            Destroy(gameObject);
        }
    }

    public void ModifySpeed(float speedFactor)
    {
        // Modify the orbit speed by the given factor
        orbitSpeed *= speedFactor;
    }

    // Handle collision with the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            // Check if the player is not invincible
            if (playerMovement != null && !playerMovement.IsInvincible()) 
            {
                // Trigger the game over event
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    // End the game if the player touches the drone
                    gameManager.GameOver();
                }

                Debug.Log("Player collided with OrbitalDrone and died.");
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed at which the player moves
    public float jumpForce = 15f; // Force applied when the player jumps
    public float fallMultiplier = 2.5f; // Multiplier for fall speed
    public float lowJumpMultiplier = 2f; // Multiplier for low jumps
    public float coyoteTime = 0.2f; // Time allowed for jumping after leaving a platform
    private float coyoteTimer; // Timer to track coyote time

    private Rigidbody2D rb; // Reference to the player's Rigidbody2D
    private SpriteRenderer spriteRenderer; // Reference to the player's sprite renderer
    private bool isGrounded; // Track if the player is on the ground
    private GameManager gameOverManager; // Reference to the GameManager
    private CameraController cameraController; // Reference to the camera controller
    private bool isInvincible = false; // Track if the player is invincible
    private bool isRocketBoostActive = false; // Track if rocket boost is active
    private float targetRocketBoostHeight; // Target height for the rocket boost
    private float rocketBoostSpeed; // Speed of the rocket boost
    public float invincibilityTransparency = 0.5f; // Transparency level for invincibility
    private Collider2D playerCollider; // Reference to the player's collider

    private string[] obstacleTags = { "Meteor", "OrbitalDrone" }; // Tags for obstacles
    private string[] platformTags = { "Solid", "PassThrough", "Bouncy", "TutorialPlatform", "Spikes" }; // Tags for platforms

    public Sprite regularSprite; // Reference to the regular player sprite
    public Sprite jumpingSprite; // Reference to the jumping sprite

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        playerCollider = GetComponent<Collider2D>(); // Get the Collider2D component
        gameOverManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        cameraController = FindObjectOfType<CameraController>(); // Find the CameraController in the scene
    }

    private void Update()
    {
        // Handle horizontal movement based on player input
        float moveInput = Input.GetAxis("Horizontal");

        // Set the new velocity
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Flip the sprite based on the direction of movement
        if (moveInput > 0)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput < 0)
        {
            // Face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Check if the player falls off the screen and trigger Game Over
        if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
        {
            // End the game if the player falls
            gameOverManager.GameOver();
            // Deactivate the player object
            gameObject.SetActive(false);
        }

        // Jump logic
        if (isGrounded)
        {
            // Reset coyote timer if grounded
            coyoteTimer = coyoteTime;
            // Set to regular sprite when on the ground
            spriteRenderer.sprite = regularSprite;
        }
        else
        {
            // Count down coyote timer
            coyoteTimer -= Time.deltaTime;
            // Set to jumping sprite when in the air
            spriteRenderer.sprite = jumpingSprite;
        }

        // Jump input detection
        if (Input.GetButtonDown("Jump") && (isGrounded || coyoteTimer > 0))
        {
            // Apply jump force
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Modify fall speed for a better jump experience
        if (rb.velocity.y < 0)
        {
            // Increase fall speed
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Faster fall if jump button is released
            rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Rocket boost handling
        if (isRocketBoostActive)
        {
            if (transform.position.y < targetRocketBoostHeight)
            {
                // Move the player upward
                rb.velocity = new Vector2(rb.velocity.x, rocketBoostSpeed);
            }
            else
            {
                // End the boost when the target height is reached
                EndRocketBoost();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isRocketBoostActive) // Only check collisions if not boosted
        {
            // Allow jumping from various platform types
            if (collision.gameObject.CompareTag("Ground") ||
                collision.gameObject.CompareTag("Solid") ||
                collision.gameObject.CompareTag("Bouncy") ||
                collision.gameObject.CompareTag("PassThrough") ||
                collision.gameObject.CompareTag("TutorialPlatform") ||
                collision.gameObject.CompareTag("StartingGround"))
            {
                isGrounded = true; // Set grounded state to true

                // Start camera movement if NOT on the StartingGround
                if (!collision.gameObject.CompareTag("StartingGround"))
                {
                    StartCameraMovement(collision.gameObject); // Pass the collided platform object
                }
            }

            // Handle collision with spikes
            if (collision.gameObject.CompareTag("Spikes"))
            {
                if (isInvincible)
                {
                    // Treat spikes like platforms if invincible
                    isGrounded = true;
                    // Start camera movement if on spikes and invincible
                    StartCameraMovement(collision.gameObject);
                }
                else
                {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    if (gameManager != null)
                    {
                        // End the game if hitting spikes without invincibility
                        gameManager.GameOver();
                        // Deactivate player object
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset grounded state if exiting a platform
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Solid") ||
            collision.gameObject.CompareTag("Bouncy") || collision.gameObject.CompareTag("PassThrough") ||
            collision.gameObject.CompareTag("TutorialPlatform") || collision.gameObject.CompareTag("StartingGround") ||
            collision.gameObject.CompareTag("Spikes"))
        {
            // Set grounded to false only on exit
            isGrounded = false;
        }
    }

    private void StartCameraMovement(GameObject collidedObject)
    {
        if (cameraController != null && !cameraController.IsCameraMoving() &&
            // Prevent camera movement on StartingGround
            !collidedObject.CompareTag("StartingGround"))
        {
            // Start camera movement
            cameraController.StartMovingCamera();
        }
    }

    public void SetInvincible(bool value)
    {
        // Set invincibility status
        isInvincible = value;
        Debug.Log("Player invincibility set to: " + value);

        if (spriteRenderer != null)
        {
            // Get the current sprite color
            Color newColor = spriteRenderer.color;

            if (isInvincible)
            {
                // Make the player semi-transparent
                newColor.a = invincibilityTransparency;
            }
            else
            {
                // Set to fully visible
                newColor.a = 1f;
            }

            // Apply the new color
            spriteRenderer.color = newColor;
        }
    }

    public void StartRocketBoost(float targetHeight, float speed)
    {
        // Activate rocket boost
        isRocketBoostActive = true;
        // Set target height
        targetRocketBoostHeight = transform.position.y + targetHeight;
        // Set speed of the boost
        rocketBoostSpeed = speed;

        // Reset Y velocity
        rb.velocity = new Vector2(rb.velocity.x, 0);

        // Ignore platform collisions during the boost
        IgnorePlatformCollisions(true);
    }

    private void EndRocketBoost()
    {
        // Deactivate rocket boost
        isRocketBoostActive = false;
        // Reset Y velocity
        rb.velocity = new Vector2(rb.velocity.x, 0);
        // Reset invincibility state
        SetInvincible(false);
        // Delay before re-enabling collisions
        StartCoroutine(ReenablePlatformCollisionsWithDelay());
    }

    private IEnumerator ReenablePlatformCollisionsWithDelay()
    {
        // Wait a bit before re-enabling collisions
        yield return new WaitForSeconds(0.1f);
        // Re-enable platform collisions
        IgnorePlatformCollisions(false);
    }

    public void IgnorePlatformCollisions(bool ignore)
    {
        // Loop through each platform tag and ignore collisions
        foreach (string tag in platformTags)
        {
            // Find platforms by tag
            GameObject[] platforms = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject platform in platforms)
            {
                // Get the platform's collider
                Collider2D platformCollider = platform.GetComponent<Collider2D>();
                if (platformCollider != null)
                {
                    // Ignore or enable collisions
                    Physics2D.IgnoreCollision(playerCollider, platformCollider, ignore);
                }
            }
        }
    }

    public bool IsInvincible()
    {
        // Check if the player is currently invincible
        return isInvincible;
    }
}


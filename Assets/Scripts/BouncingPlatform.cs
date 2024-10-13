using UnityEngine;

public class BouncingPlatformScript : MonoBehaviour
{
    // Adjust this value for the bounce height
    public float bounceForce = 10f;
    // Reference to the Animator component
    private Animator animator;

    private void Start()
    {
        // Get the Animator attached to the platform
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Apply the bounce force
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);

                // Trigger the squish animation
                animator.SetTrigger("Squish");
            }
        }
    }
}

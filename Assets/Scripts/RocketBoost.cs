using UnityEngine;

public class RocketBoost : MonoBehaviour
{
    public float rocketBoostHeight = 300f; // Target height to reach
    public float rocketBoostSpeed = 10f; // Speed at which the player moves upward

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Apply the rocket boost effect to the player
                playerMovement.StartRocketBoost(rocketBoostHeight, rocketBoostSpeed);
                // Set player invincible during boost
                playerMovement.SetInvincible(true);

                // Ignore collisions with solid platforms while boosted
                playerMovement.IgnorePlatformCollisions(true);

                // Destroy the power-up after collection
                Destroy(gameObject);
            }
        }
    }
}

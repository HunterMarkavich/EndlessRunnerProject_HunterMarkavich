using UnityEngine;

public class BedOfSpikes : MonoBehaviour
{
    private Collider2D spikesCollider;

    private void Start()
    {
        // Get the spikes' Collider2D component
        spikesCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();

            // Only trigger game over if the player is not invincible
            if (!playerMovement.IsInvincible() && collision.transform.position.y > transform.position.y)
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.GameOver();
                }
            }
        }
    }

    // Expose the spikes' collider for ignoring collisions
    public Collider2D GetSpikesCollider()
    {
        return spikesCollider;
    }
}


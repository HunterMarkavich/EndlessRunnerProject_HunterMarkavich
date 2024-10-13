using UnityEngine;

public class StarFragment : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Find the ScoreManager and update the score
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                // Add 5 points when a fragment is collected
                scoreManager.AddScore(5);
            }

            // Destroy the StarFragment after it is collected
            Destroy(gameObject);
        }
    }
}

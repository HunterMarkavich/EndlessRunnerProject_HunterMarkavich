using UnityEngine;

public class Meteor : MonoBehaviour
{
    private float fallSpeed;

    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }

    public void ModifySpeed(float speedFactor)
    {
        // Modify the fall speed by the given factor
        fallSpeed *= speedFactor;
    }

    void Update()
    {
        // Make the meteor fall at the specified speed
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Destroy the meteor once it goes below the camera to prevent memory buildup
        if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize - 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the meteor collides with the player, end the game
        if (collision.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
    }
}

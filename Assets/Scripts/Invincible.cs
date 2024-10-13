using UnityEngine;
using System.Collections;

public class Invincible : PowerUpBase
{
    public float invincibleDuration = 5f; // Set duration for the invincibility power-up

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ActivateEffect(GameObject player)
    {
        Debug.Log("Invincibility power-up collected.");

        // Make the power-up invisible after collection
        if (spriteRenderer != null)
        {
            // Disable the sprite renderer to make it invisible
            spriteRenderer.enabled = false;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on the power-up.");
        }

        // Start the invincibility coroutine
        StartCoroutine(InvincibleEffect(player));
    }

    IEnumerator InvincibleEffect(GameObject player)
    {
        Debug.Log("Invincible activated, starting coroutine.");

        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.SetInvincible(true);
            Debug.Log("Player is now invincible.");
        }
        else
        {
            Debug.LogError("PlayerMovement script not found on player.");
        }

        // Wait for the duration of the invincibility
        Debug.Log("Waiting for " + invincibleDuration + " seconds.");
        // Wait for the set duration
        yield return new WaitForSeconds(invincibleDuration);

        // Deactivate invincibility after the duration ends
        if (playerMovement != null)
        {
            Debug.Log("Invincibility duration complete. Ending invincibility.");
            playerMovement.SetInvincible(false);
        }
        else
        {
            Debug.LogError("PlayerMovement script not found when deactivating invincibility.");
        }

        Debug.Log("Invincibility power-up effect ended.");

        // Destroy the power-up object after the effect ends
        Destroy(gameObject);
        Debug.Log("Invincibility power-up object destroyed.");
    }
}

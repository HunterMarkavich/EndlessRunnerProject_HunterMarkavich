using UnityEngine;

public class SlowDownTime : PowerUpBase
{
    public float slowFactor = 0.5f; // Factor by which you slow down the speed
    public float slowDownDuration = 10f; // How long the slow effect lasts

    protected override void ActivateEffect(GameObject player)
    {
        Debug.Log("SlowDownTime power-up collected!");

        // Delegate the slow-down effect to the GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ActivateSlowDown(slowFactor, slowDownDuration);
        }

        // Destroy the power-up after collection
        Destroy(gameObject);
    }
}

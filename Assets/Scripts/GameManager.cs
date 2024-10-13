using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Reference to the Game Over Panel
    public TextMeshProUGUI newHighScoreText; // Reference to the "NEW HIGH SCORE" text
    public TextMeshProUGUI finalScoreText; // Reference to the Final Score text in the Game Over panel
    public HighScoreManager highScoreManager; // Reference to HighScoreManager
    public GameObject greyOverlay; // Reference to the grey overlay UI image

    private ScoreManager scoreManager; // Reference to ScoreManager
    private bool isSlowingDown = false; // Track if the slow-down is active

    void Start()
    {
        // Hide the grey overlay at the start
        if (greyOverlay != null)
        {
            greyOverlay.SetActive(false);
        }
        else
        {
            Debug.LogError("Grey Overlay is not assigned in the Inspector!");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Additional reference checks
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null) Debug.LogError("ScoreManager not found in the scene!");
        if (highScoreManager == null) Debug.LogError("HighScoreManager is not assigned in the Inspector!");
        if (finalScoreText == null) Debug.LogError("Final Score Text is not assigned in the Inspector!");
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
        {
            // Freeze the game
            Time.timeScale = 0;
            // Show the Game Over panel
            gameOverPanel.SetActive(true);

            if (scoreManager != null)
            {
                int finalScore = Mathf.FloorToInt(scoreManager.GetScore());
                // Update the final score text
                finalScoreText.text = "FINAL SCORE: " + finalScore;

                if (highScoreManager != null)
                {
                    bool isNewHighScore = highScoreManager.UpdateHighScores(finalScore);

                    if (isNewHighScore)
                    {
                        // Show the "NEW HIGH SCORE" text
                        newHighScoreText.gameObject.SetActive(true);
                    }
                    else
                    {
                        // Hide the "NEW HIGH SCORE" text
                        newHighScoreText.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void GoToHomeScreen()
    {
        SceneManager.LoadScene("HomeScreen");
    }


    // Method to handle the slow-down effect
    public void ActivateSlowDown(float slowFactor, float duration)
    {
        // Prevent multiple slow-downs from stacking
        if (!isSlowingDown)
        {
            StartCoroutine(SlowDownTimeEffect(slowFactor, duration));
        }
    }

    private IEnumerator SlowDownTimeEffect(float slowFactor, float duration)
    {
        // Mark that the slow-down effect is active
        isSlowingDown = true;
        Debug.Log("Applying slow-down effect...");

        // Show the grey overlay
        if (greyOverlay != null)
        {
            // Activate the overlay
            greyOverlay.SetActive(true);
        }

        // Get references to relevant game objects
        CameraController camera = FindObjectOfType<CameraController>();
        Meteor[] meteors = FindObjectsOfType<Meteor>();
        OrbitalDrone[] drones = FindObjectsOfType<OrbitalDrone>();

        // Apply slow down to the camera
        if (camera != null)
        {
            camera.ModifySpeed(slowFactor);
            Debug.Log("Camera speed slowed down.");
        }

        // Apply slow down to meteors
        foreach (Meteor meteor in meteors)
        {
            meteor.ModifySpeed(slowFactor);
            Debug.Log("Meteor speed slowed down.");
        }

        // Apply slow down to drones
        foreach (OrbitalDrone drone in drones)
        {
            drone.ModifySpeed(slowFactor);
            Debug.Log("Drone speed slowed down.");
        }

        // Wait for the duration of the slow-down effect
        yield return new WaitForSecondsRealtime(duration);

        Debug.Log("Slow-down duration complete. Resetting speeds...");

        // Reset the speed of the camera
        if (camera != null)
        {
            // Return speed to normal
            camera.ModifySpeed(1 / slowFactor);
            // Clear speed modification flag
            camera.ResetSpeedModification();
            Debug.Log("Camera speed reset.");
        }

        // Reset the speed of meteors
        foreach (Meteor meteor in meteors)
        {
            meteor.ModifySpeed(1 / slowFactor);
            Debug.Log("Meteor speed reset.");
        }

        // Reset the speed of drones
        foreach (OrbitalDrone drone in drones)
        {
            drone.ModifySpeed(1 / slowFactor);
            Debug.Log("Drone speed reset.");
        }

        // Hide the grey overlay
        if (greyOverlay != null)
        {
            // Deactivate the overlay
            greyOverlay.SetActive(false);
        }

        // Mark that the slow-down effect has ended
        isSlowingDown = false;
        Debug.Log("SlowDownTime power-up effect ended.");
    }

    public void TryAgain()
    {
        // Unfreeze the game
        Time.timeScale = 1;
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

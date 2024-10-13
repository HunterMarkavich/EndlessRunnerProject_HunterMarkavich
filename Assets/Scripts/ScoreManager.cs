using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro UI component that displays the score
    public Transform playerTransform; // Reference to the player's transform to track height

    private float score; // Track the player's score
    private float highestY; // Track the highest point the player has reached

    void Start()
    {
        score = 0;
        highestY = playerTransform.position.y;
        UpdateScoreText();
    }

    void Update()
    {
        if (isScoringActive && playerTransform.position.y > highestY)
        {
            score += playerTransform.position.y - highestY;
            highestY = playerTransform.position.y;
            UpdateScoreText();
        }
    }

    public void AddScore(int amount)
    {
        // Increment the score by the specified amount
        score += amount;
        // Update the score text on UI
        UpdateScoreText();
    }


    private void UpdateScoreText()
    {
        // Update the score UI
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public float GetScore()
    {
        // Return the current score
        return score;
    }

    public bool IsNewHighScore()
    {
        // Load top 5 scores from file and check if the current score is a new high score
        string path = Application.persistentDataPath + "/highscores.txt";
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            int[] highScores = lines.Select(line => int.TryParse(line, out int result) ? result : 0).ToArray();

            if (highScores.Length < 5)
            {
                // If there are fewer than 5 high scores, any score is a new high score
                return true;
            }

            // Check if current score is higher than the lowest score in the top 5
            int lowestHighScore = highScores.Min();
            return score > lowestHighScore;
        }

        // If there are no high scores saved, this is automatically a new high score
        return true;
    }

    // Assume scoring is active at start
    private bool isScoringActive = true;

    public void StopScoring()
    {
        // Set scoring to false when the game ends
        isScoringActive = false;
    }


}

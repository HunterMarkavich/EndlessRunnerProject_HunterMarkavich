using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private string filePath; // Path to store the high score text file
    private List<int> highScores; // List to store top 5 high scores

    void Start()
    {
        // Set up file path in the persistent data directory
        filePath = Application.persistentDataPath + "/highscores.txt";
        highScores = new List<int>();

        // Load the high scores from the file when the game starts
        LoadHighScores();
        Debug.Log("Loaded High Scores: " + string.Join(", ", highScores));
    }

    // Method to update high scores with the player's final score
    public bool UpdateHighScores(int finalScore)
    {
        // Check if the final score is a new high score
        bool isNewHighScore = false;

        if (highScores.Count < 5 || finalScore > highScores[highScores.Count - 1])
        {
            isNewHighScore = true;
            highScores.Add(finalScore);
            // Sort descending
            highScores.Sort((a, b) => b.CompareTo(a));

            // Keep only the top 5 scores
            if (highScores.Count > 5)
            {
                highScores.RemoveAt(5);
            }

            // Save the updated high scores to the file
            SaveHighScores();
        }

        Debug.Log("Updated High Scores: " + string.Join(", ", highScores));
        Debug.Log("Is New High Score: " + isNewHighScore);
        return isNewHighScore;
    }

    // Method to save high scores to the file
    private void SaveHighScores()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (int score in highScores)
            {
                writer.WriteLine(score.ToString());
            }
        }
    }

    // Method to load high scores from the file
    private void LoadHighScores()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line, out int score))
                    {
                        highScores.Add(score);
                    }
                }
            }
        }
    }

    // Method to get the high scores for displaying
    public List<int> GetHighScores()
    {
        return highScores;
    }
}

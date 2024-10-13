using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI components like Button
using TMPro; // Include this if you're using TextMeshPro
using System.Collections.Generic;
using System.IO; // For reading the file

public class HighScoreScreenManager : MonoBehaviour
{
    public TextMeshProUGUI highScoresText; // Reference to the Text component
    public Button backButton; // Reference to Back button

    private List<int> highScores; // List to store high scores

    void Start()
    {
        // Load and display high scores
        LoadHighScores();
        DisplayHighScores();

        // Assign back button listener
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoToHomeScreen);
        }
        else
        {
            Debug.LogError("Back Button is not assigned in the Inspector!");
        }
    }


    void LoadHighScores()
    {
        highScores = new List<int>();

        // Path to the high scores file
        string path = Path.Combine(Application.persistentDataPath, "highscores.txt");

        // Check if the file exists
        if (File.Exists(path))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                if (int.TryParse(line, out int score))
                {
                    // Add parsed score to the list
                    highScores.Add(score);
                }
            }
        }

        // If there are less than 5 scores, fill the remaining with zeros
        while (highScores.Count < 5)
        {
            highScores.Add(0);
        }
    }


    void DisplayHighScores()
    {
        // Set initial text
        highScoresText.text = "High Scores:\n";
        foreach (var score in highScores)
        {
            // Append each score
            highScoresText.text += score + "\n";
        }
    }

    public void GoToHomeScreen()
    {
        Debug.Log("Going to Home Screen...");
        SceneManager.LoadScene("HomeScreen");

    }

}

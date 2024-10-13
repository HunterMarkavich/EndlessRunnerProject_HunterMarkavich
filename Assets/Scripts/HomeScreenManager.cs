using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreenManager : MonoBehaviour
{
    public Button playButton; // Reference to Play button
    public Button highScoresButton; // Reference to High Scores button

    void Start()
    {
        // Assign button listeners
        playButton.onClick.AddListener(StartGame);
        highScoresButton.onClick.AddListener(ShowHighScores);
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowHighScores()
    {
        SceneManager.LoadScene("HighScoresScreen");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Quit the application
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private string scoreKey = "highScore";

    private int score = 0;
    private int highScore = 0;

    // Increase the player's score and update the high score if necessary
    public void IncreaseScore(int amount)
    {
        score += amount;
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = highScore.ToString();
        }
        UpdateScoreText();
    }

    // Load the high score from PlayerPrefs when the game starts
    private void Start()
    {
        if (PlayerPrefs.HasKey(scoreKey))
        {
            highScore = PlayerPrefs.GetInt(scoreKey);
        }
        highScoreText.text = highScore.ToString();
        UpdateScoreText();
    }

    // Reset the score and save the high score to PlayerPrefs when the game ends
    public void GameOver()
    {
        PlayerPrefs.SetInt(scoreKey, highScore);
        PlayerPrefs.Save();
        score = 0;
        UpdateScoreText();
    }

    // Update the score text
    private void UpdateScoreText()
    {
        Debug.Log("plus one");
        scoreText.text = score.ToString();
    }
}

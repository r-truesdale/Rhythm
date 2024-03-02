using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreText;
    private int score = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        Debug.Log("score" + score);
    }

    private void UpdateScoreText()
{
            scoreText.text = "Score: " + score.ToString();
    }
}
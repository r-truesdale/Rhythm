using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerStats : MonoBehaviour
{
    public List<ScoreData> scores = new List<ScoreData>();
    public TMP_Text scoreText;
        [Header("UI Elements")]
        public TMP_Text tableLevelName;

    void Start()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scores = scoreManager.loadScores();
            DisplayScores();
        }
    }

    void DisplayScores()
    {
        string scoreDisplay = "Scores:\n";
        foreach (var score in scores)
        {
            // Customize how you want to display each score
            scoreDisplay += $"{score.levelName}: Early Score : {score.earlyScore}, Perfect Score : {score.perfectScore}, Late Score : {score.lateScore}\n";
        }
        scoreText.text = scoreDisplay;
    }
}

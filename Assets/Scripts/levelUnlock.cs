using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class levelUnlock : MonoBehaviour
{
    [SerializeField] private int scoreThreshold = 8000;
    [SerializeField] private Button Btn1;
    [SerializeField] private Button Btn2;
    [SerializeField] private Button Btn3;
    void Start()
    {
        List<ScoreData> scores = ScoreManager.Instance.scores;

        if (scores.Count == 0) //to stop error on first load of menu screen trying to get an index that isn't there yet
        {
            Debug.Log("No score data found.");
            return;
        }
        //finding most recent score data from the manager
        ScoreData lastPlayedData = ScoreManager.Instance.scores[ScoreManager.Instance.scores.Count - 1];
        if (lastPlayedData == null)
        {
            Debug.Log("No recent score data found.");
            return;
        }
        if (PlayerPrefs.GetString("gameState") == "game") //unlocking only works for game mode
        {
            int originalLevel = PlayerPrefs.GetInt("selectedSongIndex");
            int targetLevel = originalLevel + 1;
            bool passThreshold = lastPlayedData.totalScore >= scoreThreshold;
            Debug.Log(lastPlayedData.totalScore);
            Debug.Log(passThreshold);

            if (targetLevel == 1)
            {
                if (passThreshold == true)
                {
                    Debug.Log("btn2 interactable");
                    Btn2.interactable = true;
                }
                else
                {
                    Btn2.interactable = false;
                }
            }
            if (targetLevel == 2)
            {
                if (passThreshold == true)
                {
                    Debug.Log("btn3 interactable");
                    Btn2.interactable = true;
                    Btn3.interactable = true;
                }
                else
                {
                    Btn2.interactable = true;
                    Btn3.interactable = false;
                }
            }
            else if (targetLevel == 3)
            {
                Btn2.interactable = true;
                Btn3.interactable = true;
            }

        }
    }
}


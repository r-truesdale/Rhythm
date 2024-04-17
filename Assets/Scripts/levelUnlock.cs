using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class levelUnlock : MonoBehaviour
{
 [SerializeField] private int scoreThreshold = 100;
 [SerializeField] private Button Btn1;
 [SerializeField] private Button Btn2;
 void Start()
 {
  Debug.Log("level unlock start");
  Debug.Log(PlayerPrefs.GetString("gameState"));
  List<ScoreData> scores = ScoreManager.Instance.scores;
  ScoreData lastPlayedData = ScoreManager.Instance.scores[ScoreManager.Instance.scores.Count - 1];
  if (lastPlayedData == null)
  {
   Debug.Log("No recent score data found.");
   return;
  }
  if (PlayerPrefs.GetString("gameState") == "game")
  {
   int originalLevel = PlayerPrefs.GetInt("selectedSongIndex");
   int targetLevel = originalLevel + 1;
   bool passThreshold = lastPlayedData.totalScore >= scoreThreshold;
   Debug.Log(lastPlayedData.totalScore);
   Debug.Log(passThreshold);
   if (passThreshold = true)
   {
    if (targetLevel == 1)
    {
     Debug.Log("btn2 interactable");
     Btn2.interactable = true;
    }
    if (targetLevel == 2)
    {
     Btn2.interactable = true;
    }
   }
  }
 }
}


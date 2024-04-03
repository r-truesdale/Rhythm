using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songEnd : MonoBehaviour
{
 // bool songStatus;
 private bool scoreGraphCalled = false;
 private bool songEnded = false;

 void Start()
 {
 }
 void Update()
 {
  if (GameManager.Instance != null)
  {
   // bool songStatus = GameManager.Instance.songStatus();
   bool gameStarted = GameManager.Instance.checkGameStart();
   // bool gameEntered = GameManager.Instance.checkGameEntered();
   bool levelEnded = GameManager.Instance.checkLevelEnded();
   if (!scoreGraphCalled && levelEnded)
   {
     scoreGraphCalled = true;
     string levelName = PlayerPrefs.GetString("songName");
     string gameMode = PlayerPrefs.GetString("gameState");
     int earlyScore = 0;
     int earlyMissScore = 0;
     int perfectScore = 0;
     int lateScore = 0;
     int lateMissScore = 0;
     string timestamp = System.DateTime.Now.ToString();
     ScoreManager.Instance.getScores(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, timestamp);
     songEnded = true;
     Debug.Log("songEnd");
     SpawnManager.Instance.levelReset(); 
    }
   }
  }
 
 public bool checkGameEnded()
 {
  if (songEnded == true)
  {
   return true;
  }
  else
  {
   return false;
  }
 }
}
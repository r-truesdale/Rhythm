// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class songEnd : MonoBehaviour
// {
//  // bool songStatus;
//  private bool scoreGraphCalled = false;
//  private bool songEnded = false;
//  // Start is called before the first frame update

//  void Start()
//  {
  
//   if (songEndUI != null)
//   {
//    songEndUI.SetActive(false);
//   }
//   else
//   {
//    Debug.LogError("SongEndUI GameObject not found!");
//   }
//  }
//  void Update()
//  {
//   if (GameManager.Instance != null)
//   {
//    // If the game state is set to "game", check the song status
//    // bool songStatus = GameManager.Instance.songStatus();
//    bool gameStarted = GameManager.Instance.checkGameStart();
//    // bool gameEntered = GameManager.Instance.checkGameEntered();
//    bool levelEnded = GameManager.Instance.checkLevelEnded();
//    if (gameStarted && !scoreGraphCalled)
//    {
//     // Debug.Log("Song has ended");
//     // if (songEndUI != null)
//     // {
//     //  songEndUI.SetActive(true);
//     // }
//     // else
//     // {
//     //  // Debug.LogError("SongEndUI GameObject not found!");
//     // }
//     if (levelEnded)
//     {
//      // ScoreManager.Instance.getScoreGraph();
//      scoreGraphCalled = true;
//      string levelName = PlayerPrefs.GetString("songName");
//      string gameMode = PlayerPrefs.GetString("gameState");
//      int earlyScore = 0;
//      int earlyMissScore = 0;
//      int perfectScore = 0;
//      int lateScore = 0;
//      int lateMissScore = 0;
//      string timestamp = System.DateTime.Now.ToString();
//      ScoreManager.Instance.getScores(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, timestamp);
//      songEnded = true;
//     }
//     else
//     {
//      // Debug.Log("Song is still playing");
//      // songEndUI.SetActive(false);
//     }
//    }
//   }
//   // else if(!songStatus && !gameStarted)
//   // {//for when song hasn't started playing yet at start
//   //  //  Debug.Log("Game hasn't started");
//   //     songEndUI.SetActive(false);
//   // }
//  }
//  public bool checkGameEnded()
//  {
//   if (songEnded == true)
//   {
//    return true;
//   }
//   else
//   {
//    return false;
//   }
//  }

//  // public void reset()
//  // {
//  //  songEndUI.SetActive(false);
//  // }
// }
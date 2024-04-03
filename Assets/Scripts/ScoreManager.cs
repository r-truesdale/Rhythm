using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
 public static ScoreManager Instance { get; private set; }
 private List<ScoreData> scores = new List<ScoreData>();
 [Header("End UI Text")]

 private int score = 0;
 public float perfect = 0;
 public float early = 0;
 public float late = 0;
 public float earlyMiss = 0;
 public float lateMiss = 0;
 private string filePath;

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
#if UNITY_EDITOR || UNITY_STANDALONE
        filePath = Path.Combine(Application.persistentDataPath, "scores.json");
#elif UNITY_WEBGL
        filePath = Path.Combine(Application.streamingAssetsPath, "scores.json");
#endif
 }

 public void AddScore(int points)
 {
  score += points;
  // UpdateScoreText();
  Debug.Log("score" + score);
 }

 public void AddLate(int latePoints)
 {
  late += latePoints;
  // lateText.text = late.ToString();
 }
 public void AddEarly(int earlyPoints)
 {
  early += earlyPoints;
  // earlyText.text = early.ToString();

 }
 public void AddPerfect(int perfectPoints)
 {
  perfect += perfectPoints;
  // perfectText.text = perfect.ToString();
 }
 public void AddEarlyMiss(int earlyMissPoints)
 {
  earlyMiss += earlyMissPoints;
 }
 public void AddLateMiss(int lateMissPoints)
 {
  lateMiss += lateMissPoints;
 }

 public void getScores(string levelName, string gameMode, int earlyScore, int earlyMissScore, int perfectScore, int lateScore, int lateMissScore, string timestamp)
 { //for the json file
  timestamp = System.DateTime.Now.ToString();
  earlyScore = Mathf.RoundToInt(this.early);
  perfectScore = Mathf.RoundToInt(this.perfect);
  lateScore = Mathf.RoundToInt(this.late);
  earlyMissScore = Mathf.RoundToInt(this.earlyMiss);
  lateMissScore = Mathf.RoundToInt(this.lateMiss);
  gameMode = PlayerPrefs.GetString("gameState", "test");
  levelName = PlayerPrefs.GetString("songName", "test");
  ScoreData newScore = new ScoreData(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, timestamp);
  scores.Add(newScore);
  saveScores();
  Debug.Log(newScore);
 }

 public void clearScores()
 {
  score = 0;
  perfect = 0;
  early = 0;
  late = 0;
  earlyMiss = 0;
  lateMiss = 0;
 }
 public void saveScores()
 {
  string json = JsonConvert.SerializeObject(scores);
#if UNITY_EDITOR || UNITY_STANDALONE
  File.WriteAllText(filePath, json);
#elif   UNITY_WEBGL
StartCoroutine(SaveScoresCoroutine(json));
#endif
 }

 private IEnumerator SaveScoresCoroutine(string json)
 {
  UnityWebRequest request = UnityWebRequest.Put(filePath, json);
  yield return request.SendWebRequest();

  if (request.result != UnityWebRequest.Result.Success)
  {
   Debug.LogError("Failed to save scores: " + request.error);
  }
  else
  {
   Debug.Log("Scores saved successfully.");
  }
 }

 public List<ScoreData> loadScores()
 {
  string json;

#if UNITY_EDITOR || UNITY_STANDALONE
  json = File.ReadAllText(filePath);
#elif UNITY_WEBGL
          UnityWebRequest request = UnityWebRequest.Get(filePath);
        request.SendWebRequest();
        while (!request.isDone) { }
        json = request.downloadHandler.text;
#endif

  scores = JsonConvert.DeserializeObject<List<ScoreData>>(json);
  Debug.Log("Scores loaded successfully.");
  return scores;
 }

 public List<ScoreData> modeScores(string gameMode)
 {
  List<ScoreData> filteredScores = new List<ScoreData>();
  foreach (var score in scores)
  {
   if (score.gameMode.Equals(gameMode))
   {
    filteredScores.Add(score);
   }
  }
  // Debug.Log(filteredScores);
  return filteredScores;
 }
 public void DeleteAllScores()
 {
  string filePath = Application.persistentDataPath + "/scores.json";
  if (File.Exists(filePath))
  {
   File.Delete(filePath);
   Debug.Log("All saved score data deleted successfully.");
  }
  else
  {
   Debug.LogWarning("No saved score data found to delete.");
  }
 }
}
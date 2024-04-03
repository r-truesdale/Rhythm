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
#if UNITY_EDITOR || UNITY_STANDALONE
    private void saveScores()
    {
      Debug.Log("Saving scores for Editor/Standalone");
        string json = JsonConvert.SerializeObject(scores);
        File.WriteAllText(Application.persistentDataPath + "/scores.json", json);
        Debug.Log(json);
    }

    public List<ScoreData> loadScores()
    {
      string filePath = Application.persistentDataPath + "/scores.json";
        if (File.Exists(filePath))
        {
             string json = File.ReadAllText(filePath);
            scores = JsonConvert.DeserializeObject<List<ScoreData>>(json);
            Debug.Log("Scores loaded successfully.");
        }
        else
        {
            scores = new List<ScoreData>();
        }
        return scores;
    } 

    #elif UNITY_WEBGL
    private void saveScores()
    {
     Debug.Log("Saving scores for WebGL");
        string json = JsonConvert.SerializeObject(scores);
        StartCoroutine(SaveScoresCoroutine(json));
    }

    private IEnumerator SaveScoresCoroutine(string json)
    {
        string saveURL = Application.persistentDataPath + "/scores.json";
        UnityWebRequest request = new UnityWebRequest(saveURL, "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to save scores: " + request.error);
            yield return null;
        }
        else
        {
            Debug.Log("Scores saved successfully.");
        }
    }

 public List<ScoreData> loadScores()
{
    StartCoroutine(LoadScoresCoroutine());
    return scores;
}

private IEnumerator LoadScoresCoroutine()
{
    string loadURL = "file://" + Application.persistentDataPath + "/scores.json";
    UnityWebRequest request = UnityWebRequest.Get(loadURL);
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Failed to load scores: " + request.error);
        yield break;
    }

    string json = request.downloadHandler.text;
    scores = JsonConvert.DeserializeObject<List<ScoreData>>(json);
    Debug.Log("Scores loaded successfully.");
}
#endif
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
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
    public List<ScoreData> scores = new List<ScoreData>();//public for statsGraph
    private bool scoreRegistered;
    private int totalScore = 0;
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
            scoreRegistered = false;
        }
    }

    public void AddScore(int points)
    {
        totalScore += points;
        // Debug.Log("score" + totalScore);
    }

    public void AddLate(int latePoints)
    {
        late += latePoints;
    }
    public void AddEarly(int earlyPoints)
    {
        early += earlyPoints;
    }
    public void AddPerfect(int perfectPoints)
    {
        perfect += perfectPoints;
    }
    public void AddEarlyMiss(int earlyMissPoints)
    {
        earlyMiss += earlyMissPoints;
    }
    public void AddLateMiss(int lateMissPoints)
    {
        lateMiss += lateMissPoints;
    }

    public void getScores(string levelName, string gameMode, int earlyScore, int earlyMissScore, int perfectScore, int lateScore, int lateMissScore, int totalScore, string timestamp)
    { //for the json file
        timestamp = System.DateTime.Now.ToString();
        earlyScore = Mathf.RoundToInt(this.early);
        perfectScore = Mathf.RoundToInt(this.perfect);
        lateScore = Mathf.RoundToInt(this.late);
        earlyMissScore = Mathf.RoundToInt(this.earlyMiss);
        lateMissScore = Mathf.RoundToInt(this.lateMiss);
        totalScore = Mathf.RoundToInt(this.totalScore);
        gameMode = PlayerPrefs.GetString("gameState", "test");
        levelName = PlayerPrefs.GetString("songName", "test");
        ScoreData newScore = new ScoreData(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, totalScore, timestamp);
        scores.Add(newScore);
        saveScores();
        Debug.Log(newScore);
    }
    void Update()
    {
        if (scoreRegistered == false)
        {
            levelEndScores();
        }
    }
    public void levelEndScores()
    {
        scoreRegistered = true;
        if (GameManager.Instance.checkLevelEnded())
        {

            string levelName = PlayerPrefs.GetString("songName");
            string gameMode = PlayerPrefs.GetString("gameState");
            int earlyScore = 0;
            int earlyMissScore = 0;
            int perfectScore = 0;
            int lateScore = 0;
            int lateMissScore = 0;
            int totalScore = 0;
            string timestamp = System.DateTime.Now.ToString();
            getScores(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, totalScore, timestamp);
        }
    }

    public void clearScores()
    {
        totalScore = 0;
        perfect = 0;
        early = 0;
        late = 0;
        earlyMiss = 0;
        lateMiss = 0;
        totalScore = 0;
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
        Debug.Log(filePath);
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
        Debug.Log(json);
    }

 private IEnumerator SaveScoresCoroutine(string json)
    {
      filePath = Path.Combine(Application.streamingAssetsPath, "scores.json");
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
 Debug.Log("Attempting to load scores");
    string filePath = Path.Combine(Application.streamingAssetsPath, "scores.json");
    UnityWebRequest request = UnityWebRequest.Get(filePath);
    request.SendWebRequest();
    while (!request.isDone) { }
    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Failed to load scores: " + request.error);
        return null;
    }
    // read data from request handler
    string json = request.downloadHandler.text;
    //move data into list of ScoreData objects
    scores = JsonConvert.DeserializeObject<List<ScoreData>>(json);
    Debug.Log("Scores loaded successfully.");
    Debug.Log("Scores: "+scores);
    return scores;
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
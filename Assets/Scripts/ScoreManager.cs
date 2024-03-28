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
    public TMP_Text scoreText;
    public TMP_Text lateText;
    public TMP_Text earlyText;
    public TMP_Text perfectText;

    private int score = 0;
    private float perfect = 0;
    private float early = 0;
    private float late = 0;
    private float earlyMiss = 0;
    private float lateMiss = 0;
    [Header("Player Graph")]
    public GameObject barPrefab;
    public Transform earlyBarSpawnPoint;
    public Transform lateBarSpawnPoint;
    public Transform perfectBarSpawnPoint;

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

    public void AddLate(int latePoints)
    {
        late += latePoints;
        lateText.text = late.ToString();
    }
    public void AddEarly(int earlyPoints)
    {
        early += earlyPoints;
        earlyText.text = early.ToString();

    }
    public void AddPerfect(int perfectPoints)
    {
        perfect += perfectPoints;
        perfectText.text = perfect.ToString();
    }
    public void AddEarlyMiss(int earlyMissPoints)
    {
        earlyMiss += earlyMissPoints;
        // perfectText.text = earlyMiss.ToString();
    }
    public void AddLateMiss(int lateMissPoints)
    {
        lateMiss += lateMissPoints;
        // lateText.text = late.ToString();
    }

    public void getScoreGraph()
    {
        float totalPoints = perfect + early + late;
        float perfectWidth = (perfect / (float)totalPoints) * 100f;
        float earlyWidth = (early / (float)totalPoints) * 100f;
        float lateWidth = (late / (float)totalPoints) * 100f;
        InstantiateCube(perfectWidth, perfectBarSpawnPoint);
        InstantiateCube(earlyWidth, earlyBarSpawnPoint);
        InstantiateCube(lateWidth, lateBarSpawnPoint);
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    private void InstantiateCube(float width, Transform spawnPoint)
    {
        int widthInt = Mathf.RoundToInt(width); // Convert float to int
        Vector3 barOrigin = spawnPoint.position + new Vector3(width / 2f, 0f, 0f);
        // Instantiate the bar prefab at the calculated position
        GameObject bar = Instantiate(barPrefab, barOrigin, Quaternion.identity);
        // Set the scale of the bar based on the width
        bar.transform.localScale = new Vector3(widthInt, 5f, 1f);
    }
// public void fakeScores()
// {
//     // Dummy data for testing
//     string levelName = "DummyLevel";
//     string gameMode = "DummyMode";
//     int earlyScore = 10;
//     int earlyMissScore = 5;
//     int perfectScore = 20;
//     int lateScore = 15;
//     int lateMissScore = 8;
//     string timestamp = System.DateTime.Now.ToString();
    
//     // Call getScores with the dummy data
//     getScores(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, timestamp);
// }

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
        loadScores();
        Debug.Log(newScore);
    }

private void saveScores()
{
    string json = JsonConvert.SerializeObject(scores);
    File.WriteAllText(Application.persistentDataPath + "/scores.json", json);
}

public List<ScoreData> loadScores()
{
    if (File.Exists(Application.persistentDataPath + "/scores.json"))
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/scores.json");
        scores = JsonConvert.DeserializeObject<List<ScoreData>>(json);
    }
    else
    {
        scores = new List<ScoreData>();
    }
    return scores;
}
    public List<ScoreData> modeScores(string gameMode)
    {
        List<ScoreData> filteredScores = new List<ScoreData>();
        foreach (var score in scores)
        {
            if (score.gameMode == gameMode)
            {
                filteredScores.Add(score);
            }
        }
        return filteredScores;
    }

}
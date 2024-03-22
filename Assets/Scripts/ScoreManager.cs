using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreText;
    public TMP_Text lateText;
    public TMP_Text earlyText;
    public TMP_Text perfectText;
    private int score = 0;
    private float perfect = 0;
    private float early = 0;
    private float late = 0;
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
}
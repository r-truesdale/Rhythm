using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class playerStats : MonoBehaviour
{
    public List<ScoreData> scores = new List<ScoreData>();
    private string currentMode;
    [Header("Player Scores")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] public Transform scorePanel;
    [SerializeField] private float spacing = 10f;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text LvlName;
    [SerializeField] private TMP_Text gameMode;
    [SerializeField] private TMP_Text earlyMiss;
    [SerializeField] private TMP_Text early;
    [SerializeField] private TMP_Text perfect;
    [SerializeField] private TMP_Text late;
    [SerializeField] private TMP_Text lateMiss;
    [SerializeField] private TMP_Text modeText;
    [SerializeField] private Button switchModeButton;
    void Start()
    {
        string sceneType = PlayerPrefs.GetString("sceneType");
        if (sceneType == "stats") //only attempts to find the objects if it's the stats scene
        {
            InitializeStatsScreen();
        }
        else
        {
            return;
        }
    }
    void InitializeStatsScreen()
    {
        scoreManager = GameObject.Find("GameMaster")?.GetComponent<ScoreManager>();
        scorePanel = GameObject.Find("Canvas/Scroll/Viewport/Content")?.transform;
        scorePrefab = GameObject.Find("scorePrefab");
        switchModeButton = GameObject.Find("Canvas/ModeBtn")?.GetComponent<Button>();
        modeText = GameObject.Find("Canvas/modeText")?.GetComponent<TextMeshProUGUI>();
        if (scoreManager == null || scorePanel == null || scorePrefab == null || switchModeButton == null || modeText == null)
        {
            Debug.Log("playerStats: One or more required GameObjects or components not found.");
            return;
        }
        statsScreen();
    }
    public void statsScreen() //first scores shown are from the most recently played mode
    {
        if (PlayerPrefs.GetString("gameState") == "practice")
        {
            currentMode = "practice";
        }
        else if (PlayerPrefs.GetString("gameState") == "game")
        {
            currentMode = "game";
        }
        DisplayScores(currentMode);
        switchModeButton.onClick.AddListener(switchMode);
        UpdateModeText();
    }
    public void switchMode() // attached to a button, switches scores being displayed to other mode
    {
        currentMode = (currentMode == "game") ? "practice" : "game";
        DisplayScores(currentMode);
        UpdateModeText();
    }
    void UpdateModeText()
    {
        // update mode text based on the current mode
        modeText.text = (currentMode == "game") ? "Game Mode Scores" : "Practice Mode Scores";
    }
    void DisplayScores(string gameMode)
    {
        if (scorePanel != null)
        {
            foreach (Transform child in scorePanel)//destroys the previous scores 
            {
                Destroy(child.gameObject);
            }
            List<ScoreData> filteredScores = scoreManager.modeScores(gameMode);
            float startYPos = 0f;
            // go through each score to display
            for (int i = filteredScores.Count - 1; i >= 0; i--)
            {
                if (filteredScores[i].levelName == "Song1")//assigns actual song names to generic level names
                {
                    filteredScores[i].levelName = "Animal Crossing";
                }
                if (filteredScores[i].levelName == "Song2")
                {
                    filteredScores[i].levelName = "Hall of the Mountain King";
                }
                if (filteredScores[i].levelName == "Song3")
                {
                    filteredScores[i].levelName = "He's a Pirate";
                }
                GameObject scoreItem = Instantiate(scorePrefab, scorePanel); //scorePrefab becomes a row in the score table
                scoreItem.transform.SetParent(scorePanel, false);
                //populate the score row[i] with the corresponding saved level info
                TMP_Text[] scoreTexts = scoreItem.GetComponentsInChildren<TMP_Text>();
                scoreTexts[0].text = filteredScores[i].levelName;
                scoreTexts[1].text = filteredScores[i].earlyMissScore.ToString();
                scoreTexts[2].text = filteredScores[i].earlyScore.ToString();
                scoreTexts[3].text = filteredScores[i].perfectScore.ToString();
                scoreTexts[4].text = filteredScores[i].lateScore.ToString();
                scoreTexts[5].text = filteredScores[i].lateMissScore.ToString();
                scoreTexts[6].text = filteredScores[i].totalScore.ToString();
                //position objects so most recent is at the top
                float yPos = startYPos - (i * (scorePrefab.GetComponent<RectTransform>().rect.height + spacing));
                scoreItem.transform.localPosition = new Vector3(0f, yPos, 0f);
            }
        }
    }
}
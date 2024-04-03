using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerStats : MonoBehaviour
{
 public List<ScoreData> scores = new List<ScoreData>();
 [SerializeField] private ScoreManager scoreManager;
 [SerializeField] private GameObject scorePrefab;
 private Transform scorePanel;
 [SerializeField] private float spacing = 10f; // Adjust this value as needed

 [Header("UI Elements")]
 // public TMP_Text gameModeTxt;
 public TMP_Text LvlName;
 public TMP_Text gameMode;
 public TMP_Text earlyMiss;
 public TMP_Text early;
 public TMP_Text perfect;
 public TMP_Text late;
 public TMP_Text lateMiss;

 [SerializeField] private Button switchModeButton;
 [SerializeField] private TMP_Text modeText;
 private string currentMode;

 void Start()
 {
  scorePanel = GameObject.Find("Canvas/Scroll/Viewport/Content").transform;
  scoreManager = GameObject.Find("GameMaster").GetComponent<ScoreManager>();
  if (scoreManager != null)
  {
   scores = scoreManager.loadScores();
   Debug.Log(scores);
   currentMode = recentGameMode();
   DisplayScores(currentMode);
  }
  switchModeButton.onClick.AddListener(SwitchMode);
  UpdateModeText();
 }

 string recentGameMode()//finds most recent added score and return its gamemode 
 {
  if (scores.Count > 0)
  {
   // most recent game mode played
   return scores[scores.Count - 1].gameMode;
  }
  else
  {
   return "DefaultGameMode";
  }
 }

 void SwitchMode()
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
  foreach (Transform child in scorePanel)
  {
   Destroy(child.gameObject);
  }

  List<ScoreData> filteredScores = scoreManager.modeScores(gameMode);

  // calc score item height
  float totalHeight = filteredScores.Count * (scorePrefab.GetComponent<RectTransform>().rect.height + spacing);
  float startYPos = 0f;

  // go through each score to display
  for (int i = filteredScores.Count - 1; i>= 0; i--)
  {
   GameObject scoreItem = Instantiate(scorePrefab, scorePanel);

   scoreItem.transform.SetParent(scorePanel, false);
   TMP_Text[] scoreTexts = scoreItem.GetComponentsInChildren<TMP_Text>();
   scoreTexts[0].text = filteredScores[i].levelName;
   scoreTexts[1].text = filteredScores[i].earlyMissScore.ToString();
   scoreTexts[2].text = filteredScores[i].earlyScore.ToString();
   scoreTexts[3].text = filteredScores[i].perfectScore.ToString();
   scoreTexts[4].text = filteredScores[i].lateScore.ToString();
   scoreTexts[5].text = filteredScores[i].lateMissScore.ToString();
   scoreTexts[6].text = filteredScores[i].gameMode;

   float yPos = startYPos - (i * (scorePrefab.GetComponent<RectTransform>().rect.height + spacing));

   scoreItem.transform.localPosition = new Vector3(0f, yPos, 0f);
  }
 }
}
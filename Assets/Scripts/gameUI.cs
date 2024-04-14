using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class gameUI : MonoBehaviour
{
 public static gameUI Instance { get; private set; }
 [SerializeField] private Button startButton;
 [Header("Scripts")]
 [SerializeField] private GameManager gameManager; //instead of using inspector bc of GM instance
 [SerializeField] private SpawnManager spawnManager;
 [SerializeField] private ScoreManager scoreManager;
 [SerializeField] private GameObject songEndUI;
 [SerializeField] private songMenu songMenu;

 [SerializeField] private playerStats playerStats;
 [Header("Text Elements")]
 [SerializeField] private TMP_Text lateText;
 [SerializeField] private TMP_Text earlyText;
 [SerializeField] private TMP_Text perfectText;
 [Header("Graph Elements")]
 [SerializeField] private GameObject barPrefab;
 [SerializeField] private Transform earlyMissBarSpawnPoint;
 [SerializeField] private Transform earlyBarSpawnPoint;
 [SerializeField] private Transform perfectBarSpawnPoint;
 [SerializeField] private Transform lateBarSpawnPoint;
 [SerializeField] private Transform lateMissBarSpawnPoint;
 public Canvas pauseMenuCanvas;
 [SerializeField] private Button pauseBtn;
 [SerializeField] private Button resumeBtn;
 [SerializeField] private Button gameOverBtn;
 [SerializeField] private Button currentBtn;
 [SerializeField] private TMP_Text currentBtnText;
 private bool waitingForKeyInput = false; 
 void Start()
 {
  // Find the GameManager instance
  gameManager = FindObjectOfType<GameManager>();
  spawnManager = GameObject.Find("GameMaster").GetComponent<SpawnManager>();
  playerStats = GameObject.Find("GameMaster").GetComponent<playerStats>();
  scoreManager = GameObject.Find("GameMaster").GetComponent<ScoreManager>();
  songMenu = GameObject.Find("GameMaster").GetComponent<songMenu>();
  songEndUI.SetActive(false);
 }

 void Update()
 {
  if (GameManager.Instance.songDuration())
  {
   songEnded();
  }
  pauseMenu();
  inputKeyChoice();
 }

 public void songEnded()
 {
  gameOverBtn.onClick.Invoke();
 }
 // Update is called once per frame
 public void pauseMenu()
 {
  if (Input.GetKeyDown(KeyCode.P))
  {
   pauseMenuCanvas.enabled = !pauseMenuCanvas.enabled;
   if (pauseMenuCanvas.enabled)
   {
    GameManager.Instance.PauseGame();
    SpawnManager.Instance.stopSpawn();
    pauseBtn.onClick.Invoke();
   }
   else
   {
    resumeBtn.onClick.Invoke();
    SpawnManager.Instance.playSpawn();
    GameManager.Instance.ResumeGame();
   }
  }
 }
 public void resumeLevel()
 {
  SpawnManager.Instance.playSpawn();
  GameManager.Instance.ResumeGame();
 }
 public void startBtn()
 {
  GameManager.Instance.StartGame();
  SpawnManager.Instance.findObjects();
  SpawnManager.Instance.initialize();
  SpawnManager.Instance.playSpawn();
 }
 public void endBtn()
 {
  songEndUI.SetActive(true);
  scoreGraph();
  SpawnManager.Instance.stopSpawn();
  SpawnManager.Instance.destroyArrows();
 }

 public void lvlSelectPracBtn()
 {
  // GameManager.Instance.resetLevel();
  GameManager.Instance.gameEndBtn();
  SpawnManager.Instance.levelReset();
  ScoreManager.Instance.clearScores();
  songMenu.practiceMenu();
 }
 public void lvlSelectGameBtn()
 {
  // GameManager.Instance.resetLevel();
  GameManager.Instance.gameEndBtn();
  SpawnManager.Instance.levelReset();
  ScoreManager.Instance.clearScores();
  songMenu.gameMenu();
 }
 public void mainMenuBtn()
 {
  GameManager.Instance.gameEndBtn();
  SpawnManager.Instance.levelReset();
  ScoreManager.Instance.clearScores();
 }
 public void statsBtn()
 {
  GameManager.Instance.gameEndBtn();
  SpawnManager.Instance.levelReset();
  ScoreManager.Instance.clearScores();
  songMenu.playerStatsMenu();
 }
 public void scoreGraph() // calcs and instatiates score graph
 {
  float totalPoints = ScoreManager.Instance.perfect + ScoreManager.Instance.early + ScoreManager.Instance.late + ScoreManager.Instance.earlyMiss + ScoreManager.Instance.lateMiss;
  float perfectWidth = (ScoreManager.Instance.perfect / totalPoints) * 100f;
  float earlyWidth = (ScoreManager.Instance.early / totalPoints) * 100f;
  float lateWidth = (ScoreManager.Instance.late / totalPoints) * 100f;
  float earlyMissWidth = (ScoreManager.Instance.earlyMiss / totalPoints) * 100f;
  float lateMissWidth = (ScoreManager.Instance.lateMiss / totalPoints) * 100f;

  InstantiateCube(perfectWidth, perfectBarSpawnPoint);
  InstantiateCube(earlyWidth, earlyBarSpawnPoint);
  InstantiateCube(lateWidth, lateBarSpawnPoint);
  InstantiateCube(earlyMissWidth, earlyMissBarSpawnPoint);
  InstantiateCube(lateMissWidth, lateMissBarSpawnPoint);
  earlyText.text = scoreManager.early.ToString();
  perfectText.text = scoreManager.perfect.ToString();
  lateText.text = scoreManager.late.ToString();

  string levelName = PlayerPrefs.GetString("songName");
  string gameMode = PlayerPrefs.GetString("gameState");
  int earlyScore = 0;
  int earlyMissScore = 0;
  int perfectScore = 0;
  int lateScore = 0;
  int lateMissScore = 0;
  string timestamp = System.DateTime.Now.ToString();
  ScoreManager.Instance.getScores(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, timestamp);
 }

 private void InstantiateCube(float width, Transform spawnPoint)
 {
  int widthInt = Mathf.RoundToInt(width); // Convert float to int
  Vector3 barOrigin = spawnPoint.position + new Vector3(width / 2f, 0f, 0f);
  GameObject bar = Instantiate(barPrefab, barOrigin, Quaternion.identity);
  bar.transform.localScale = new Vector3(widthInt, 5f, 1f);
 }
 public void setInputKey(KeyCode key)
 {
  GameManager.Instance.setInputKey(key);
 }

 public KeyCode getInputKey()
 {
  return GameManager.Instance.getInputKey();
 }
 public void SetNextInputKey(Button button)
 {
   currentBtnText.GetComponent<TMP_Text>();
  if (!waitingForKeyInput){
  currentBtn = button;
  currentBtnText.text = "Press Key...";
  waitingForKeyInput = true;
 }
 }
 public void inputKeyChoice()
 {
   currentBtnText.GetComponent<TMP_Text>();
  if (currentBtn != null && Input.anyKeyDown && waitingForKeyInput)
  {
   foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
   {
    if (Input.GetKeyDown(keyCode))
    {
     gameManager.setInputKey(keyCode);
     currentBtnText.text = keyCode.ToString(); 
     currentBtn = null;
     waitingForKeyInput = false; 
     break;
    }
   }
  }
 }
 public void resetKey()
 {
  setInputKey(KeyCode.Space);
  currentBtnText.text = "SPACE";
 }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameUI : MonoBehaviour
{
 [SerializeField] private Button startButton;
 [SerializeField] private GameManager gameManager; //instead of using inspector bc of GM instance
 [SerializeField] private SpawnManager spawnManager;
 [SerializeField] private ScoreManager scoreManager;
 [SerializeField] private songMenu songMenu;
 [SerializeField] private GameObject barPrefab;
 [SerializeField] private Transform earlyMissBarSpawnPoint;
 [SerializeField] private Transform earlyBarSpawnPoint;
 [SerializeField] private Transform perfectBarSpawnPoint;
 [SerializeField] private Transform lateBarSpawnPoint;
 [SerializeField] private Transform lateMissBarSpawnPoint;
 [SerializeField] private GameObject songEndUI;
 public Canvas EndCanvas;
 bool levelEnded = GameManager.Instance.checkLevelEnded();


 // }
 void Start()
 {

  // Find the GameManager instance
  GameManager gameManager = FindObjectOfType<GameManager>();
  spawnManager = GameObject.Find("GameMaster").GetComponent<SpawnManager>();
  scoreManager = GameObject.Find("GameMaster").GetComponent<ScoreManager>();
  songMenu = GameObject.Find("GameMaster").GetComponent<songMenu>();
  songEndUI.SetActive(false);


  if (gameManager != null && spawnManager != null)
  {
   // Get the Button component attached to this GameObject
   startButton = GameObject.Find("StartBtn").GetComponent<Button>();
  }
  else
  {
   Debug.LogError("GameManager not found in the scene.");
  }
 }

 void Update()
 {
  if (levelEnded)
  {
   
   songEndUI.SetActive(true); // Activate the canvas
   scoreGraph(); // Call the method to display score graph
  }
 }
 // Update is called once per frame
 public void startBtn()
 {
  gameManager.StartGame();
  spawnManager.findObjects();
  spawnManager.initialize();
 }
  public void startBtn2()
 {
  gameManager.startgame2();

  spawnManager.findObjects();
  spawnManager.initialize();
 }
 public void endBtn()
 {
  spawnManager.stopSong();
  songEndUI.SetActive(true);
  scoreGraph();
 }
  public void statsBtn()
 {
  gameManager.resetLevel();
  spawnManager.levelReset();
  songMenu.playerStatsMenu();
 }
 public void scoreGraph()
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
 }

 private void InstantiateCube(float width, Transform spawnPoint)
 {
  int widthInt = Mathf.RoundToInt(width); // Convert float to int
  Vector3 barOrigin = spawnPoint.position + new Vector3(width / 2f, 0f, 0f);
  GameObject bar = Instantiate(barPrefab, barOrigin, Quaternion.identity);
  bar.transform.localScale = new Vector3(widthInt, 5f, 1f);
 }
}

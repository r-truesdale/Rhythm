using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
 public static GameManager Instance { get; private set; }

 [Header("Midi Data")]
 public List<float> midiScoreBeats;
 public List<float> midiScoreDownBeats;
 [Header("Arrows")]
 public GameObject arrowPrefab;
 private float timingThreshold = 0.1f;
 public bool gameStarted;
 public bool levelPlaying;
 public float gameStartTime;
 
 private bool spawningPaused = false;
 private List<string> previousScores = new List<string>();
 // private bool isArrowSpawnCoroutineRunning = false;
 [SerializeField] private MidiFilePlayer midiFilePlayer; // Reference to the MidiFilePlayer
  // Flag to track if the level is playing
 private float levelStartTime = 0f; // Time when the level started playing


 void Start()
 {
  InitializeGameManager();
  Debug.Log("GMStart");
  if (midiFilePlayer == null)
  {
   Debug.LogError("MidiFilePlayer is not assigned!");
  }
  else
  {
   Debug.Log("MidiFilePlayer is assigned correctly.");
  }
 }
 public void InitializeGameManager()
 {
  if (!gameStarted) // Check if the game hasn't started yet
  {
   if (Instance == null)
   {
    Instance = this;
    DontDestroyOnLoad(gameObject);
   }
   else
   {
    Destroy(gameObject);
   }
  }
  else
  {
   Destroy(gameObject); // If the game has started, destroy this instance
  }
  midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
  StartCoroutine(LoadSongData());
 }
 void Awake()
 {
  levelPlaying = false;
  gameStarted = false;
  midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
 }
 public IEnumerator LoadSongData()
 {
  yield return songData.Instance.LoadSongs();
 }
 private void FindMidiFilePlayer()
 {
  midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
  if (midiFilePlayer == null)
  {
   Debug.LogError("MidiFilePlayer component not found in the scene.");
  }
  else
  {
   Debug.LogError("MidiFilePlayer component found and assigned successfully.");
  }
 }
 public void StartGame()
 {
  gameStarted = true;
  levelPlaying = true;
  UpdateBeatOptions();
  SpawnManager.Instance.InitializeArrowsSpawned(midiScoreBeats.Count);
  SpawnManager.Instance.InitializeMidiScoreBeats(midiScoreBeats);
  levelStartTime = Time.time; // record the start time of the level
  Debug.Log("Game Started at time: " + levelStartTime);
  midiFilePlayer.MPTK_Play(); // start playing the MIDI file
 }

 public void UpdateBeatOptions()
 {
  int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
  int beatType = PlayerPrefs.GetInt("beatType", 0); // Default to 0 for midi_score_beats
  Debug.Log("selectedSongIndex: " + selectedSongIndex);
  switch (beatType)
  {
   case 0: // midi_score_beats
    midiScoreBeats = songData.Instance.GetMidiScoreBeats(selectedSongIndex);
    break;
   case 1: // midi_score_downbeats
    midiScoreBeats = songData.Instance.GetMidiScoreDownBeats(selectedSongIndex);
    break;
   // case 2:
   //     // Get the beats for the third option and assign them to midiScoreBeats
   //     break;
   default: // default to midi_score_beats if PlayerPrefs value is invalid
    midiScoreBeats = songData.Instance.GetMidiScoreBeats(selectedSongIndex);
    break;
  }
 }

 void Update()
 {
  if (midiFilePlayer == null)
  {
   midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
  }
  checkGameStart();
  songStatus();
  checkLevelEnded();
  if (levelPlaying)
  {
   float elapsedTime = Time.time - levelStartTime;
  }
  Debug.Log("Level playing" + levelPlaying);
  Debug.Log("Game started" + gameStarted);
  Debug.Log("Song Status" + songStatus());
 }
 public float CheckElapsedTime()
 {
  return Time.time - levelStartTime;
 }

 public bool songStatus()
 {
  if (midiFilePlayer.MPTK_IsPlaying == true)
  {
   // Debug.Log("still playing");
   return true;
  }
  else
  {
   // Debug.Log("game ended");
   return false;
  }
 }

 public bool checkGameStart()
 {
  if (gameStarted == true)
  {
   return true;
  }
  else
  {
   return false;
  }
 }
 // public void EndLevel()
 // {
 //  levelPlaying = false;
 // }
 // public void StartLevel()
 // {
 //  levelPlaying = true;
 // }
 // public void EnterMenu()
 // {
 //  levelPlaying = false;
 // }
 public bool checkLevelEnded()
 {
  if (midiFilePlayer.MPTK_IsPlaying == false && gameStarted && !levelPlaying)
  {
   return true;
  }
  else
  {
   return false;
  }
 }

 public void resetLevel()
 {
  midiFilePlayer.MPTK_Stop();
  // spawningPaused = false;
 }
 public void gameStartBtn()
 {
  gameStarted = true;
  levelPlaying = true;
 }
 public void gameEndBtn()
 {
  gameStarted = false;
  levelPlaying = false;
 }
}
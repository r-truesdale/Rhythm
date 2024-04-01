using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
 public static GameManager Instance { get; private set; }

 [Header("Testing")]
 public TMP_Text beatsNum;

 [Header("Midi Data")]
 // public MidiFilePlayer midiFilePlayer;
 public List<float> midiScoreBeats;
 public List<float> midiScoreDownBeats;
 [Header("Arrows")]
 public GameObject arrowPrefab;

 [Header("Hitbox")]
 // public HitBox[] HitBoxes; //HitBox objects for arrow spawning
 // private HitBox hitBoxInstance;

 private float timingThreshold = 0.1f;
 public bool gameStarted = false;
 // private bool songStatus = false;
 public float gameStartTime;
 private bool spawningPaused = false;
 private List<string> previousScores = new List<string>();
 // private bool isArrowSpawnCoroutineRunning = false;
 [SerializeField] private MidiFilePlayer midiFilePlayer; // Reference to the MidiFilePlayer

 void Start()
 {
  gameStarted = false;
  InitializeGameManager();
  Debug.Log("GMStart");
  // InitializeGameManager();
  if (midiFilePlayer == null)
  {
   Debug.LogError("MidiFilePlayer is not assigned!");
  }
  else
  {
   Debug.Log("MidiFilePlayer is assigned correctly.");
  }
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
   UpdateBeatOptions();
  // HitBoxes = FindObjectsOfType<HitBox>();

  gameStartTime = Time.time;
  Debug.Log(gameStartTime);
  Debug.Log("Game Started!");
  // if(SpawnManager.Instance.initialized = true){
  SpawnManager.Instance.InitializeArrowsSpawned(midiScoreBeats.Count);
  SpawnManager.Instance.InitializeMidiScoreBeats(midiScoreBeats);
  // }
  // Start spawning arrows based on the JSON file timings
  // StartCoroutine(CheckArrowSpawn());
  // CheckArrowSpawn();
  // Start playing the MIDI file after a delay
  // float delay = GetTimeToHitbox(); // Adjust this delay as needed
  // float delay = 0;
  // StartCoroutine(DelayedStartMidi(delay));
  FindMidiFilePlayer();
  midiFilePlayer.MPTK_Play(); // Start playing the MIDI file
 }
public void startgame2(){
   gameStarted = true;
  gameStartTime = Time.time;
    SpawnManager.Instance.InitializeArrowsSpawned(midiScoreBeats.Count);
  SpawnManager.Instance.InitializeMidiScoreBeats(midiScoreBeats);
  Debug.Log(gameStartTime);
  Debug.Log("Game2 Started!");
    FindMidiFilePlayer();
  midiFilePlayer.MPTK_Play();
}
 // IEnumerator DelayedStartMidi(float delay)
 // {
 //  yield return new WaitForSeconds(delay);
 //  midiFilePlayer.MPTK_Play(); // Start playing the MIDI file
 //  gameStarted = true;
 // }
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
  StartCoroutine(LoadSongData());
 }
 public IEnumerator LoadSongData()
 {
  yield return songData.Instance.LoadSongs();
  // Check if midiScoreBeats is not null before initializing arrowsSpawned
  // if (midiScoreBeats != null)
  // {
  //     arrowsSpawned = new bool[midiScoreBeats.Count];
  //     for (int i = 0; i < arrowsSpawned.Length; i++)
  //     {
  //         arrowsSpawned[i] = false;
  //     }
  // }
  // gameEntered = true;
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
   // case 2: // Your third option
   //     // Get the beats for the third option and assign them to midiScoreBeats
   //     break;
   default: // Default to midi_score_beats if PlayerPrefs value is invalid
    midiScoreBeats = songData.Instance.GetMidiScoreBeats(selectedSongIndex);
    break;
  }
 }

 void Update()
 {
  if (midiFilePlayer == null || midiScoreBeats == null)
   return;
  // CheckArrowSpawn();
  // HandlePlayerInput();
  checkGameStart();
  songStatus();
  checkLevelEnded();
  
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
 // public bool checkGameEntered()
 // {
 //  if (gameEntered == true)
 //  {
 //   return true;
 //  }
 //  else
 //  {
 //   return false;
 //  }
 // }

 public bool checkLevelEnded()
 {
  if (midiFilePlayer.MPTK_IsPlaying == false && gameStarted)
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
  // songStatus = false;
  // gameStarted = false;
  // gameEntered = false;
  spawningPaused = false;
 }
}
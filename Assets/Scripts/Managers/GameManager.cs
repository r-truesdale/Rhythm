using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Midi Data")]
    public List<float> midiScoreBeats;
    public List<float> midiScoreDownBeats;
    [Header("Timings")]
    private float timingThreshold = 0.1f;
    public bool gameStarted;
    public bool levelPlaying;
    public float gameStartTime;
    private bool spawningPaused = false;
    private float pausedTime;
    public bool gamePaused = false;
    private float levelStartTime = 0f;
    //private List<string> previousScores = new List<string>();
    [Header("Objects")]
    public GameObject BeatObjectPrefab;
    [SerializeField] private MidiFilePlayer midiFilePlayer;
    public double lastNote;
    public double currentDuration;
    //private KeyCode defaultInputKey = KeyCode.Space;
    private KeyCode currentInputKey = KeyCode.Space;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Home")
        {
            PlayerPrefs.SetString("sceneType", "menu");
        }
        Debug.Log(PlayerPrefs.GetString("sceneType"));
        InitializeGameManager();
        if (midiFilePlayer == null)
        {
            Debug.Log("MidiFilePlayer is not assigned!");
        }
        else
        {
            Debug.LogWarning("MidiFilePlayer is assigned correctly.");
        }
    }
    public void InitializeGameManager()
    {
        if (!gameStarted) // check if the game has started
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
            Destroy(gameObject); // if the game has started, destroy this instance
        }
        midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
        StartCoroutine(LoadSongData());
    }
    void Awake()
    {
        levelPlaying = false;
        gameStarted = false;
    }
    public IEnumerator LoadSongData()
    {
        if (songData.Instance == null)
        {
            Debug.Log("song data null");
            yield break;
        }
        Debug.Log("load song data");
        yield return songData.Instance.LoadSongs();
    }
    private void FindMidiFilePlayer()
    {
        midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
        if (midiFilePlayer == null)
        {
            Debug.Log("MidiFilePlayer component not found in the scene.");
        }
        else
        {
            Debug.Log("MidiFilePlayer component found and assigned successfully.");
        }
    }
    public void StartGame()
    {
        UpdateBeatOptions();
        // Debug.Log("MidiScoreBeats"+ midiScoreBeats);
        SpawnManager.Instance.InitializeBeatObjectsSpawned(midiScoreBeats.Count);
        SpawnManager.Instance.InitializeMidiScoreBeats(midiScoreBeats);

        Debug.Log("Game Started at time: " + levelStartTime);
        float delay = 0.5f;
        StartCoroutine(DelayedStartMidi(delay));
        // midiFilePlayer.MPTK_Play(); // start playing the MIDI file
    }
    IEnumerator DelayedStartMidi(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindMidiFilePlayer();
        levelStartTime = Time.time; // record the start time of the level
        gameStarted = true;
        levelPlaying = true;
        midiFilePlayer.MPTK_Play(); // Start playing the MIDI file
    }

    public void UpdateBeatOptions()
    {
        if (songData.Instance != null)
        {
            int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
            int beatType = PlayerPrefs.GetInt("beatType", 0); // Default to 0 for midi_score_beats
            Debug.Log("selectedSongIndex: " + selectedSongIndex);
            if (PlayerPrefs.GetString("gameState") == "practice")
            {
                switch (beatType)
                {
                    case 0: // midi_score_beats
                        midiScoreBeats = songData.Instance.GetMidiScoreBeats(selectedSongIndex);
                        Debug.Log("beats");
                        break;
                    case 1: // midi_score_downbeats
                        midiScoreBeats = songData.Instance.GetMidiScoreDownBeats(selectedSongIndex);
                        Debug.Log("down beats");
                        break;
                    case 2:
                        midiScoreBeats = songData.Instance.GetMidiOffBeats(selectedSongIndex);
                        Debug.Log("off beats");
                        break;
                    default: // default to midi_score_beats if PlayerPrefs value is invalid
                        midiScoreBeats = songData.Instance.GetMidiOffBeats(selectedSongIndex);
                        break;
                }
            }
            else if (PlayerPrefs.GetString("gameState") == "game")
                switch (beatType)
                {
                    case 0: //
                        midiScoreBeats = songData.Instance.GetMidiEasy(selectedSongIndex);
                        Debug.Log("easy");
                        break;
                    case 1: // 
                        midiScoreBeats = songData.Instance.GetMidiMedium(selectedSongIndex);
                        Debug.Log("medium");
                        break;
                    case 2:
                        midiScoreBeats = songData.Instance.GetMidiHard(selectedSongIndex);
                        Debug.Log("hard");
                        break;
                    default: // default to easy if PlayerPrefs value is invalid
                        midiScoreBeats = songData.Instance.GetMidiEasy(selectedSongIndex);
                        Debug.Log("Defualt - Easy");
                        break;
                }
        }
    }

    void Update()
    {
        if (midiFilePlayer == null)
        {
            midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
        }
        else if (midiFilePlayer != null)
        {
            lastNote = midiFilePlayer.MPTK_DurationMS;
            currentDuration = midiFilePlayer.MPTK_Position;
            checkGameStart();
            songStatus();
            checkLevelEnded();
            songDuration();
            // Debug.Log(songDuration());
            // Debug.Log(lastNote);
            // Debug.Log(currentDuration);
            //   Debug.Log("Level playing" + levelPlaying);
            // Debug.Log("Game started" + gameStarted);
            // Debug.Log("Song Status" + songStatus());
            if (levelPlaying)
            {
                float elapsedTime = Time.time - levelStartTime;
            }
        }
    }
    public bool songDuration()
    {
        if (levelPlaying && (lastNote) <= (currentDuration))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float CheckElapsedTime()
    {
        return Time.time - levelStartTime;
    }

    public bool songStatus()
    {
        if (midiFilePlayer != null)
        {
            if (midiFilePlayer.MPTK_IsPlaying == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
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
    public void EndLevel()
    {
        levelPlaying = false;
    }

    public bool checkLevelEnded()
    {
        if (midiFilePlayer != null)
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
        return false;
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
        spawningPaused = false;
    }
    public void PauseGame()
    {
        gamePaused = true;
        pausedTime = Time.time;
    }

    public void ResumeGame()
    {
        if (gamePaused)
        {
            gamePaused = false;
            float pauseDuration = Time.time - pausedTime;
            levelStartTime += pauseDuration;
        }
    }
    public KeyCode getInputKey()
    {
        return currentInputKey;
    }
    public void setInputKey(KeyCode key)
    {
        currentInputKey = key;
    }
}
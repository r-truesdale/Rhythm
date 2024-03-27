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
    public MidiFilePlayer midiFilePlayer;
    public List<float> midiScoreBeats;
    public List<float> midiScoreDownBeats;
    [Header("Arrows")]
    public GameObject arrowPrefab;
    public bool[] arrowsSpawned;

    [Header("Hitbox")]
    public HitBox[] HitBoxes; //HitBox objects for arrow spawning
    public float perfectTimingWindow = 0.2f;
    private HitBox hitBoxInstance;
    public List<GameObject> spawnedArrows = new List<GameObject>(); // Track spawned arrows
    private float timingThreshold = 0.1f;
    private bool gameStarted = false;
    private bool gameEntered = false;
    private float gameStartTime;
    private List<string> previousScores = new List<string>();
    // private bool isArrowSpawnCoroutineRunning = false;

    void Start()
    {
        InitializeGameManager();
        hitboxTimings();
        gameEntered = true;
    }
    public void StartGame()
    {
        gameStartTime = Time.time;
        Debug.Log("gameStartTime");
        Debug.Log("Game Started!");

        // Start spawning arrows based on the JSON file timings
        // StartCoroutine(CheckArrowSpawn());
        CheckArrowSpawn();
        // Start playing the MIDI file after a delay
        float delay = GetTimeToHitbox(); // Adjust this delay as needed
        // float delay = 0;
        StartCoroutine(DelayedStartMidi(delay));
    }
    public void hitboxTimings()
    {
        float arrowSpeed = arrowPrefab.GetComponent<arrows>().speed;
        float perfectHitboxSize = arrowSpeed * perfectTimingWindow;
        SetHitboxSize(perfectHitboxSize);

    }
    IEnumerator DelayedStartMidi(float delay)
    {
        yield return new WaitForSeconds(delay);
        midiFilePlayer.MPTK_Play(); // Start playing the MIDI file
        gameStarted = true;
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
        StartCoroutine(LoadSongData());
    }
    public IEnumerator LoadSongData()
    {
        yield return songData.Instance.LoadSongs();
        UpdateBeatOptions();
        // Check if midiScoreBeats is not null before initializing arrowsSpawned
        if (midiScoreBeats != null)
        {
            arrowsSpawned = new bool[midiScoreBeats.Count];
            for (int i = 0; i < arrowsSpawned.Length; i++)
            {
                arrowsSpawned[i] = false;
            }
        }
        else
        {
            Debug.LogError("Failed to load song data. midiScoreBeats is null.");
        }
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
        CheckArrowSpawn();
        HandlePlayerInput();
        songStatus();
    }


    Vector3 GetArrowPosition()
    {
        return transform.position;
    }
    Vector3 GetArrowSpawnPosition()
    {
        // Return the spawn position of the arrow (you may need to adjust this based on your scene setup)
        return SpawnManager.Instance.GetArrowSpawnPosition(); // Modify this to match your arrow spawn position
    }

    //     void getArrowSpawnTime
    //     {
    //     float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
    //     arrowSpawnTime = midiScoreBeats[0] - currentPosition - GetTimeToHitbox();
    // }
    void CheckArrowSpawn()
    {
        if (!gameStarted) // Check if the game has started
            return;

        if (gameStarted)
        {
            float gamePosition = Time.time - gameStartTime;
            float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
            bool arrowSpawnedThisFrame = false; // Flag to track if an arrow has been spawned in this frame
            for (int i = 0; i < midiScoreBeats.Count; i++)
            {
                float beatTime = midiScoreBeats[i];
                float arrowSpawnTime = beatTime + GetTimeToHitbox() - 0.5f;
                // Debug.Log((arrowSpawnTime));
                float arrowSpeed = arrowPrefab.GetComponent<arrows>().speed;
                if (i < arrowsSpawned.Length && !arrowSpawnedThisFrame && !arrowsSpawned[i] && gamePosition >= arrowSpawnTime)
                {
                    GameObject newArrow = SpawnManager.Instance.SpawnArrow(arrowPrefab, arrowSpawnTime, beatTime, arrowSpeed); // Spawn the arrow earlier
                    spawnedArrows.Add(newArrow);
                    arrowsSpawned[i] = true;
                    arrowSpawnedThisFrame = true; // Set the flag to true since an arrow has been spawned in this frame
                    beatsNum.text = beatTime.ToString();
                    break;
                }
            }
        }
    }

void HandlePlayerInput()
{
    if (gameStarted && Input.GetKeyDown(KeyCode.Space)) // Check if the game has started
    {
        // Iterate over all spawned arrows
        for (int i = 0; i < spawnedArrows.Count; i++)
        {
            GameObject arrowObject = spawnedArrows[i];
            
            // Check if the arrow object is valid and active
            if (arrowObject != null && arrowObject.activeSelf)
            {
                arrows arrowScript = arrowObject.GetComponent<arrows>();
                
                // Check if the arrow script is valid
                if (arrowScript != null)
                {
                    int hitBoxIndex = arrowScript.hitBoxIndex;

                    // Process the hit with the appropriate timing parameters
                    HitBox hitBox = HitBoxes[hitBoxIndex];
                    if (hitBox != null)
                    {
                        hitBox.ProcessHit(arrowScript.beatTime, GetPlaybackTime(), hitBoxIndex);
                    }
                    else
                    {
                        Debug.LogError("HitBox not found for arrow.");
                    }

                    // Remove the arrow from spawnedArrows list and destroy it
                    spawnedArrows.RemoveAt(i);
                    Destroy(arrowObject);

                    // Exit the loop to prevent interacting with subsequent arrows
                    break;
                }
            }
        }
    }
}
        
    float GetTimeToHitbox()
    {
        // Calculate the time it takes for an arrow to reach the hitbox based on its speed and hitbox position
        float hitBoxDistance = Vector3.Distance(GetArrowSpawnPosition(), HitBoxes[0].transform.position); // Assuming there's only one hitbox
        arrows arrow = arrowPrefab.GetComponent<arrows>(); // Get the arrows component from the arrowPrefab
        if (arrow != null)
        {
            // Debug.Log(hitBoxDistance / arrow.speed);
            return hitBoxDistance / arrow.speed;
        }
        else
        {
            Debug.LogError("Arrows component not found on arrowPrefab.");
            return 0f; // Or handle the error in a way appropriate for your application
        }
    }

    void SetHitboxSize(float size)
    {
        float otherSize = size * 1.5f;
        float otherHitboxOffset = (otherSize - HitBoxes[1].transform.localScale.y);
        HitBoxes[0].transform.localScale = new Vector3(HitBoxes[0].transform.localScale.x, size, HitBoxes[0].transform.localScale.z);

        // Adjust the position of the early hitbox
        Vector3 earlyHitboxPosition = HitBoxes[1].transform.localPosition;
        earlyHitboxPosition.y += otherHitboxOffset;
        HitBoxes[1].transform.localPosition = earlyHitboxPosition;
        HitBoxes[1].transform.localScale = new Vector3(HitBoxes[1].transform.localScale.x, otherSize, HitBoxes[1].transform.localScale.z);

        Vector3 earlyMissHitboxPosition = HitBoxes[3].transform.localPosition;
        earlyMissHitboxPosition.y += otherHitboxOffset * 1.5f;
        HitBoxes[3].transform.localPosition = earlyMissHitboxPosition;
        // HitBoxes[3].transform.localScale = new Vector3(HitBoxes[3].transform.localScale.x, otherSize, HitBoxes[3].transform.localScale.z);
        // Adjust the position of the late hitbox
        Vector3 lateHitboxPosition = HitBoxes[2].transform.localPosition;
        lateHitboxPosition.y -= otherHitboxOffset; // negative bc position is below perfect hitbox
        HitBoxes[2].transform.localPosition = lateHitboxPosition;
        HitBoxes[2].transform.localScale = new Vector3(HitBoxes[2].transform.localScale.x, otherSize, HitBoxes[2].transform.localScale.z);
    }
    public float GetPlaybackTime()
    {
        if (midiFilePlayer != null)
        {
            return (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
        }
        else
        {
            Debug.LogError("MidiFilePlayer is not assigned in GameManager.");
            return 0f;
        }
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
    public bool checkGameEntered()
    {
        if (gameEntered == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
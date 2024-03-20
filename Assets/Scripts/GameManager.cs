using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MidiFilePlayer midiFilePlayer;
    public TMP_Text beatsNum;
    public TMP_Text debugTest;
    public TMP_Text scoreText;
    public List<float> midiScoreBeats;
    public List<float> midiScoreDownBeats;
    public bool[] arrowsSpawned;
    public GameObject arrowPrefab; // Reference to the arrow prefab
    public HitBox[] HitBoxes; // Reference to HitBox objects for arrow spawning
    private HitBox hitBoxInstance;
    public List<GameObject> spawnedArrows = new List<GameObject>(); // Track spawned arrows
    private float timingThreshold = 0.1f; // Adjust as needed
    private bool gameStarted = false;
    private List<string> previousScores = new List<string>();

    private void Awake()
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
    }
    void Start()
    {
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
            // Handle the case where midiScoreBeats is null (e.g., show an error message)
            Debug.LogError("Failed to load song data. midiScoreBeats is null.");
        }
    }

    public void UpdateBeatOptions()
    {
        int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
        int beatType = PlayerPrefs.GetInt("beatType", 0); // Default to 0 for midi_score_beats
        Debug.Log(selectedSongIndex);
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
    }
    public void StartGame()
    {
        gameStarted = true;
        Debug.Log("Game Started!");
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
    void CheckArrowSpawn()
    {
        if (!gameStarted) // Check if the game has started
            return;

        float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
        for (int i = 0; i < midiScoreBeats.Count; i++)
        {
            float beatTime = midiScoreBeats[i];
            float arrowSpawnTime = beatTime - currentPosition - GetTimeToHitbox(); // Adjusted spawn time

            if (i < arrowsSpawned.Length && !arrowsSpawned[i] && arrowSpawnTime <= timingThreshold)
            {
                GameObject newArrow = SpawnManager.Instance.SpawnArrow(arrowPrefab, arrowSpawnTime, beatTime); // Spawn the arrow earlier
                spawnedArrows.Add(newArrow);
                arrowsSpawned[i] = true;
                beatsNum.text = beatTime.ToString();
            }
        }
    }
    void HandlePlayerInput()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.Space)) // Check if the game has started
        {
            if (spawnedArrows.Count > 0)
            {
                GameObject arrowToRemove = spawnedArrows[0];
                arrows arrowScript = arrowToRemove.GetComponent<arrows>();
                int hitBoxIndex = arrowScript.hitBoxIndex;
                for (int i = 0; i < spawnedArrows.Count; i++)
                {
                    // GameObject arrow = spawnedArrows[i];
                    // arrows arrowScript = arrow.GetComponent<arrows>();
                    if (arrowScript != null)
                    {
                        // Check if the arrow has already collided with a hitbox
                        if (!arrowScript.gameObject.activeSelf) // Checking if the arrow gameObject is active
                        {
                            // Arrow already collided, skip to the next one
                            continue;
                        }
                        // Process the hit with the appropriate timing parameters
                        HitBox hitBox = HitBoxes[hitBoxIndex];
                        
                        if (hitBox != null)
                        {
                            hitBox.ProcessHit(arrowScript.beatTime, GetPlaybackTime(), hitBoxIndex);
                            // string accuracy = AccuracyManager.Instance.GetAccuracyResult();
                            // debugTest.text = $"Beat Time: {arrowScript.beatTime}\nHit Timing: {accuracy}";
                        }
                        else
                        {
                            Debug.LogError("HitBox not found for arrow.");
                        }
                        spawnedArrows.RemoveAt(0);
                        Destroy(arrowToRemove);
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

}
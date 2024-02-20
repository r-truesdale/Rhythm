using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MidiPlayerTK;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MidiFilePlayer midiFilePlayer;
    public GameObject arrowPrefab;
    public TMP_Text beatsNum;
    private List<float> time;
    private bool[] arrowsSpawned;

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

    void Start()
    {
        StartCoroutine(LoadSongData());
    }

    IEnumerator LoadSongData()
    {
        yield return songData.Instance.LoadSongs();
        Initialize();
    }

    void Initialize()
    {
        int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
        // Debug.Log(selectedSongIndex);
        time = songData.Instance.GetMidiScoreBeats(selectedSongIndex);
        arrowsSpawned = new bool[time.Count];
    }

    void Update()
    {

        if (midiFilePlayer == null || time == null)
            return;

        float currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;

        for (int i = 0; i < time.Count; i++)
        {
            // Debug.Log(time[i]);
            float spawnTime = time[i];
            float timeRange = 0.01f;

            if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
            {
                SpawnArrow();
                arrowsSpawned[i] = true;
                beatsNum.text = spawnTime.ToString();
                Debug.Log(spawnTime);
                break;
            }
        }
    }

    private void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, new Vector3(UnityEngine.Random.Range(-120, 100), 50, -10), Quaternion.identity);
    }
}


// Having a GameManager instance as a singleton is a common design pattern in Unity game development. There are several reasons why this pattern is used:

// Centralized Management: The GameManager serves as a centralized hub for managing various aspects of the game, such as audio, player progress, scorekeeping, level loading, and more. By having a single instance accessible from anywhere in the game, it becomes easier to coordinate different game systems and functionalities.

//     Global Access: Since the GameManager instance is typically accessible from any script in the game, it provides a convenient way for other scripts to interact with shared game state and functionality without needing direct references to each other. This promotes loose coupling between components and enhances code organization and readability.

//     Persistence: By marking the GameManager instance as DontDestroyOnLoad, it persists across scene changes. This ensures that important game state and functionality remain consistent throughout the entire gameplay experience, even as players move between different levels or sections of the game.

//     Singleton Pattern: Implementing the GameManager as a singleton ensures that only one instance of it exists at any given time. This prevents multiple instances from being accidentally created and helps maintain data integrity and consistency across the game.

// Overall, using a GameManager instance as a singleton provides a clean and efficient way to manage and coordinate various aspects of the game, leading to better organization, maintainability, and scalability of the game codebase.
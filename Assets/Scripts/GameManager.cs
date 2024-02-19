using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MidiPlayerTK;
using TMPro;
// public class GameManager : MonoBehaviour
// {
//     public MidiFilePlayer midiFilePlayer;
//     public GameObject arrowPrefab;
//     public JArray time;
//     public int selectedSongIndex;
//     public TMP_Text beatNum;
//     private bool[] arrowsSpawned;
//     public songData songDataInstance;

//     void Start()
//     {
//         if (songDataInstance != null && songDataInstance.songsData != null)
//         {
//             // Access midi score beats using songDataInstance
//             int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
//             time = songDataInstance.GetMidiScoreBeats(selectedSongIndex);
//             arrowsSpawned = new bool[time.Count];
//             Debug.Log(time);
//             // Proceed with the rest of your logic
//         }
//         else
//         {
//             Debug.LogError("songData instance is not assigned or JSON data is not loaded!");
//         }
//     }
//     void Update()
//     {
//         var currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
//         // Spawn arrows based on beat timings
//         for (int i = 0; i < time.Count; i++)
//         {
//             float spawnTime = time[i].Value<float>();
//             float timeRange = 0.01f;
//             // Debug.Log(spawnTime);
//             if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
//             {
//                 SpawnArrow();
//                 beatNum.text = time[i].ToString();
//                 Debug.Log(time[i]);
//                 // Mark the arrow as spawned for this beat
//                 arrowsSpawned[i] = true;
//                 break;
//             }
//         }
//     }
//     private void SpawnArrow()
//     {
//         GameObject arrow = Instantiate(arrowPrefab, new Vector3(Random.Range(-120, 100), 50, -10), Quaternion.identity);
//     }
// }
public class GameManager : MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer;
    public GameObject arrowPrefab;
    public TMP_Text beatNum;
    private bool[] arrowsSpawned;
    private List<float> time;
    

    // Start is called before the first frame update
    void Start()
    {
        // Wait for the songData to load asynchronously
        StartCoroutine(LoadSongData());
    }

    IEnumerator LoadSongData()
    {
        // Wait for songData to be loaded
        while (songData.Instance == null || songData.Instance.AllSongs == null)
        {
            yield return null;
        }

        // Song data is loaded, proceed with initialization
        Initialize();
    }

    void Initialize()
    {
        // Access midi score beats using songDataInstance
        int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
        time = songData.Instance.GetMidiScoreBeats(selectedSongIndex);
        arrowsSpawned = new bool[time.Count];
        Debug.Log(time);
    }

    // Update is called once per frame
    void Update()
    {
        var currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
        // Spawn arrows based on beat timings
        for (int i = 0; i < time.Count; i++)
        {
            float spawnTime = time[i];
            float timeRange = 0.01f;
            // Debug.Log(spawnTime);
            if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
            {
                SpawnArrow();
                // beatNum.text = time[i].ToString();
                Debug.Log(time[i]);
                // Mark the arrow as spawned for this beat
                arrowsSpawned[i] = true;
                break;
            }
        }
    }

    private void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, new Vector3(UnityEngine.Random.Range(-120, 100), 50, -10), Quaternion.identity);
    }
}
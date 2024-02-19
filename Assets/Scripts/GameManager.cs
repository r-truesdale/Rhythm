// using System.IO;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Newtonsoft.Json.Linq;
// using MidiPlayerTK;
// using TMPro;
// public class GameManager : MonoBehaviour
// {
//     public MidiFilePlayer midiFilePlayer;
//     public GameObject arrowPrefab;
//     private JArray time;
//     public int selectedSongIndex;
//     // public TMP_Text beatNum;
//     private bool[] arrowsSpawned;
//     public songData songDataInstance;
//     void Start()
//     {
//         // selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
//         // JArray time = songData.GetMidiScoreBeats(selectedSongIndex);
//         // Debug.Log(time);
//         // arrowsSpawned = new bool[time.Count];
//         if (songDataInstance != null)
//         {
//             int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
//             time = songDataInstance.GetMidiScoreBeats(selectedSongIndex);
//             Debug.Log(time);
//             // arrowsSpawned = new bool[time.Count];
//             arrowsSpawned = new bool[time != null ? time.Count : 0];
//         }
//         else
//         {
//             Debug.LogError("songData instance is not assigned!");
//         }
//     }

//     // void Update()
//     // {
//     //     var currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
//     //     // Spawn arrows based on beat timings
//     //     for (int i = 0; i < time.Count; i++)
//     //     {
//     //         float spawnTime = time[i].Value<float>();
//     //         float timeRange = 0.01f;

//     //         if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
//     //         {
//     //             // SpawnArrow();
//     //             // beatNum.text = time[i].ToString();
//     //             Debug.Log("arrow");
//     //             // Mark the arrow as spawned for this beat
//     //             arrowsSpawned[i] = true;
//     //         }
//     //     }
//     // }

//     // void Update()
//     // {
//     //     if (songDataInstance != null)
//     //     {
//     //         var currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
//     //         // Spawn arrows based on beat timings
//     //         for (int i = 0; i < time.Count; i++)
//     //         {
//     //             float spawnTime = time[i].Value<float>();
//     //             float timeRange = 0.01f;

//     //             if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
//     //             {
//     //                 SpawnArrow();
//     //                 arrowsSpawned[i] = true;
//     //             }
//     //         }
//     //     }
//     // }
//         void Update()
//     {
//         if (songDataInstance != null && time != null)
//         {
//             var currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
//             // Spawn arrows based on beat timings
//             for (int i = 0; i < time.Count; i++)
//             {
//                 float spawnTime = time[i].Value<float>();
//                 float timeRange = 0.01f;

//                 if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
//                 {
//                     SpawnArrow();
//                     arrowsSpawned[i] = true;
//                 }
//             }
//         }
//     }
//     private void SpawnArrow()
//     {
//         GameObject arrow = Instantiate(arrowPrefab, new Vector3(Random.Range(-120, 100), 50, -10), Quaternion.identity);
//     }
// }


using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using MidiPlayerTK;
using TMPro;
public class GameManager : MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer;
    public GameObject arrowPrefab;
    public JArray time;
    public int selectedSongIndex;
    public TMP_Text beatNum;
    private bool[] arrowsSpawned;
    public songData songDataInstance;
    // void Start()
    // {
    //     if (songDataInstance != null)
    //     {
    //         int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
    //         var time = songDataInstance.GetMidiScoreBeats(selectedSongIndex);

    //         Debug.Log(time);
    //     }
    //     else
    //     {
    //         Debug.LogError("songData instance is not assigned!");
    //     }
    // arrowsSpawned = new bool[time.Count];
    void Start()
    {
        if (songDataInstance != null && songDataInstance.songsData != null)
        {
            // Access midi score beats using songDataInstance
            int selectedSongIndex = PlayerPrefs.GetInt("selectedSongIndex", 0);
            time = songDataInstance.GetMidiScoreBeats(selectedSongIndex);
            arrowsSpawned = new bool[time.Count];
            Debug.Log(time);
            // Proceed with the rest of your logic
        }
        else
        {
            Debug.LogError("songData instance is not assigned or JSON data is not loaded!");
        }
    }
    void Update()
    {
        var currentTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
        // Spawn arrows based on beat timings
        for (int i = 0; i < time.Count; i++)
        {
            float spawnTime = time[i].Value<float>();
            float timeRange = 0.01f;
            // Debug.Log(spawnTime);
            if (!arrowsSpawned[i] && Mathf.Abs(currentTime - spawnTime) < timeRange)
            {
                SpawnArrow();
                beatNum.text = time[i].ToString();
                Debug.Log(time[i]);
                // Mark the arrow as spawned for this beat
                arrowsSpawned[i] = true;
                break;
            }
        }
    }
    private void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, new Vector3(Random.Range(-120, 100), 50, -10), Quaternion.identity);
    }
}
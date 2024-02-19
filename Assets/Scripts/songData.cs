// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using Newtonsoft.Json.Linq;

// public class songData : MonoBehaviour
// {
//     public JArray songsData;
//     // public static JArray songsData { get; private set; }

//     IEnumerator Start()
//     {
//         string jsonFilePath = Application.streamingAssetsPath + "/jsontest.json";
//         UnityWebRequest www = UnityWebRequest.Get(jsonFilePath);
//         yield return www.SendWebRequest();
//         if (www.result == UnityWebRequest.Result.Success)
//         {
//             string jsonContent = www.downloadHandler.text;

//             try
//             {
//                 JObject jsonData = JObject.Parse(jsonContent);
//                 songsData = (JArray)jsonData["songs"];
//                 Debug.Log("JSON data loaded successfully.");
//             }
//             catch (System.Exception e)
//             {
//                 Debug.LogError("Error parsing JSON data: " + e.Message);
//             }
//         }
//         else
//         {
//             Debug.LogError("Failed to fetch JSON data: " + www.error);
//         }
//     }

//     // public static JArray GetMidiScoreBeats(int songIndex)
//     // {
//     //     if (songsData != null && songIndex >= 0 && songIndex < songsData.Count)
//     //     {
//     //         JObject selectedSong = (JObject)songsData[songIndex];
//     //         var midiScoreBeats = (JArray)selectedSong["midi_score_beats"];
//     //         // Debug.Log("Number of beats for song " + songIndex + ": " + (midiScoreBeats != null ? midiScoreBeats.Count : 0));
//     //         return midiScoreBeats;
//     //     }
//     //     else
//     //     {
//     //         Debug.LogError("Invalid song index or missing data.");
//     //         return null; // Handle invalid songIndex or missing data
//     //     }
//     // }
//     // public static JArray songsBeats { get; private set; }
//     // void Start()
//     // {
//     //     // locate and load json file data
//     //     string jsonFilePath = Application.streamingAssetsPath + "/scripts/jsontest.json";
//     //     string jsonContent = File.ReadAllText(jsonFilePath);
//     //     JObject jsonData = JObject.Parse(jsonContent);
//     //     songsData = (JArray)jsonData["songs"];
//     //     // Debug.Log(songsData);
//     //     // Debug.Log("Number of songs: " + (songsData != null ? songsData.Count : 0));
//     // // }
//     // public JArray GetMidiScoreBeats(int songIndex)
//     // {
//     //     if (songsData != null && songIndex >= 0 && songIndex < songsData.Count)
//     //     {
//     //         JObject selectedSong = (JObject)songsData[songIndex];
//     //         var midiScoreBeats = (JArray)selectedSong["midi_score_beats"];
//     //         Debug.Log("Number of beats for song " + songIndex + ": " + (midiScoreBeats != null ? midiScoreBeats.Count : 0));
//     //         return midiScoreBeats;
//     //     }
//     //     else
//     //     {
//     //         Debug.Log(songsData);
//     //         Debug.LogError("Invalid song index or missing data.");
//     //         return null; // Handle invalid songIndex or missing data
//     //     }
//     // }
//         private void ParseMidiScoreBeats()
//     {
//         if (songsData != null)
//         {
//             foreach (JObject song in songsData)
//             {
//                 JArray midiScoreBeats = (JArray)song["midi_score_beats"];
//                 // Add midi_score_beats array to the song's data
//                 song["midi_score_beats_array"] = midiScoreBeats;
//             }
//         }
//     }

//     // Method to get midi_score_beats for a specific song index
//     public JArray GetMidiScoreBeats(int songIndex)
//     {
//         if (songsData != null && songIndex >= 0 && songIndex < songsData.Count)
//         {
//             JObject selectedSong = (JObject)songsData[songIndex];
//             // Retrieve midi_score_beats_array for the selected song
//             var midiScoreBeats = (JArray)selectedSong["midi_score_beats_array"];
//             Debug.Log("Number of beats for song " + songIndex + ": " + (midiScoreBeats != null ? midiScoreBeats.Count : 0));
//             return midiScoreBeats;
//         }
//         else
//         {
//             Debug.LogError("Invalid song index or missing data.");
//             return null;
//         }
//     }
// }

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

public class songData : MonoBehaviour
{
    public JArray songsData;

    // public static JArray songsBeats { get; private set; }
    // void Start()
    // {
    //     // locate and load json file data
    //     string jsonFilePath = Path.Combine(Application.dataPath + "/StreamingAssets", "jsontest.json");
    //     string jsonContent = File.ReadAllText(jsonFilePath);
    //     JObject jsonData = JObject.Parse(jsonContent);
    //     songsData = (JArray)jsonData["songs"];
    //     // Debug.Log(songsData);
    //     // Debug.Log("Number of songs: " + (songsData != null ? songsData.Count : 0));
    // }
    //         IEnumerator Start()
    //     {
    //         string jsonFilePath = Application.streamingAssetsPath + "/jsontest.json";
    //         UnityWebRequest www = UnityWebRequest.Get(jsonFilePath);
    //         yield return www.SendWebRequest();
    //         if (www.result == UnityWebRequest.Result.Success)
    //         {
    //             string jsonContent = www.downloadHandler.text;

    //             try
    //             {
    //                 JObject jsonData = JObject.Parse(jsonContent);
    //                 songsData = (JArray)jsonData["songs"];
    //                 Debug.Log("JSON data loaded successfully.");
    //             }
    //             catch (System.Exception e)
    //             {
    //                 Debug.LogError("Error parsing JSON data: " + e.Message);
    //             }
    //         }
    //         else
    //         {
    //             Debug.LogError("Failed to fetch JSON data: " + www.error);
    //         }
    //     }
    //     public static JArray GetMidiScoreBeats(int songIndex)
    //     {
    //         if (songsData != null && songIndex >= 0 && songIndex < songsData.Count)
    //         {
    //             JObject selectedSong = (JObject)songsData[songIndex];
    //             var midiScoreBeats = (JArray)selectedSong["midi_score_beats"];
    //             Debug.Log("Number of beats for song " + songIndex + ": " + (midiScoreBeats != null ? midiScoreBeats.Count : 0));
    //             return midiScoreBeats;
    //         }
    //         else
    //         {
    //             Debug.LogError("Invalid song index or missing data.");
    //             return null; // Handle invalid songIndex or missing data
    //         }
    //     }
    // }

void Start()
    {
        LoadJsonData();
    }

 void LoadJsonData()
    {
        string jsonFilePath = Application.streamingAssetsPath + "/jsontest.json";
        string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

        try
        {
            JObject jsonData = JObject.Parse(jsonContent);
            songsData = (JArray)jsonData["songs"];
            Debug.Log("JSON data loaded successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing JSON data: " + e.Message);
        }
    }
    public JArray GetMidiScoreBeats(int songIndex)
    {
        if (songsData != null && songIndex >= 0 && songIndex < songsData.Count)
        {
            JObject selectedSong = (JObject)songsData[songIndex];
            var midiScoreBeats = (JArray)selectedSong["midi_score_beats"];
        return midiScoreBeats;
    }
    else
    {
        Debug.LogError("Invalid song index or missing data.");
        return null; // Handle invalid songIndex or missing data
    }
    }
}
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;

// public class songData : MonoBehaviour
// {
//     public JArray songsData;
//     void Start()
//     {
//         LoadJsonData();
//     }

//     void LoadJsonData()
//     {
//         string jsonFilePath = Application.streamingAssetsPath + "/jsontest.json";
//         string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

//         try
//         {
//             JObject jsonData = JObject.Parse(jsonContent);
//             songsData = (JArray)jsonData["songs"];
//             Debug.Log("JSON data loaded successfully.");
//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError("Error parsing JSON data: " + e.Message);
//         }
//     }
//     public JArray GetMidiScoreBeats(int songIndex)
//     {
//         if (songsData != null && songIndex >= 0 && songIndex < songsData.Count)
//         {
//             JObject selectedSong = (JObject)songsData[songIndex];
//             var midiScoreBeats = (JArray)selectedSong["midi_score_beats"];
//             return midiScoreBeats;
//         }
//         else
//         {
//             Debug.LogError("Invalid song index or missing data.");
//             return null; // Handle invalid songIndex or missing data
//         }
//     }
// }

public class songData : MonoBehaviour
{
    public static songData Instance { get; private set; }
    public List<SongBlueprint> AllSongs;
    private List<SongBlueprint> songsData;
    

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        LoadSongs();
    }

    void LoadSongs()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "SongBlueprint.json");
        Debug.Log("JSON file path: " + jsonPath);
        if (File.Exists(jsonPath))
        {
            string jsonData = File.ReadAllText(jsonPath);
            AllSongs data = JsonConvert.DeserializeObject<AllSongs>(jsonData);
            ConvertToSongBlueprints(data);
        }
        else
        {
            Debug.LogError("SongBlueprint.json not found at path: " + jsonPath);
        }
    }

    void ConvertToSongBlueprints(AllSongs data)
    {
        AllSongs = data.songs;
    }

    // Method to retrieve midi score beats for a specific song
    public List<float> GetMidiScoreBeats(int songIndex)
    {
        if (AllSongs != null && songIndex >= 0 && songIndex < AllSongs.Count)
        {
            return AllSongs[songIndex].midi_score_beats;
        }
        else
        {
            Debug.LogError("Invalid song index or songs data is not initialized.");
            return null;
        }
    }
}
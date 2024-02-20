using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class songData : MonoBehaviour
{
    public static songData Instance { get; private set; }
    public List<SongBlueprint> AllSongs;


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

    // void LoadSongs()
    // {
    //     string jsonPath = Path.Combine(Application.streamingAssetsPath, "SongBlueprint.json");
    //     Debug.Log("JSON file path: " + jsonPath);
    //     if (File.Exists(jsonPath))
    //     {
    //         string jsonData = File.ReadAllText(jsonPath);
    //         AllSongs data = JsonConvert.DeserializeObject<AllSongs>(jsonData);
    //         ConvertToSongBlueprints(data);
    //     }
    //     else
    //     {
    //         Debug.LogError("SongBlueprint.json not found at path: " + jsonPath);
    //     }
    // }

    //public so it can be accessed by other scripts
    public IEnumerator LoadSongs()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "SongBlueprint.json");
        Debug.Log("JSON file path: " + jsonPath);
        // Load JSON data from file
        string jsonData = null;
#if UNITY_EDITOR || UNITY_STANDALONE
    if (File.Exists(jsonPath))
    {
        jsonData = File.ReadAllText(jsonPath);
        // Debug.Log(jsonData);
    }
#elif UNITY_WEBGL
    using (UnityWebRequest www = UnityWebRequest.Get(jsonPath))
    {
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load JSON file: " + www.error);
            yield break; // Exit coroutine on error
        }
        jsonData = www.downloadHandler.text;
        // Debug.Log(jsonData);
    }
#endif
        // Deserialize JSON data into List<SongBlueprint>
        if (jsonData != null)
        {
            AllSongs = JsonConvert.DeserializeObject<AllSongs>(jsonData).songs;
        }
        else
        {
            Debug.LogError("Failed to load JSON data.");
        }

        yield return null; // Ensure a return value for the coroutine
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
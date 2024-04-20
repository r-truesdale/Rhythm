using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class songData : MonoBehaviour
{
    public static songData Instance { get; private set; }
    public List<SongBlueprint> AllSongs;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
        StartCoroutine(LoadSongs());
        Debug.Log("songdata start");
    }
    public IEnumerator LoadSongs()
    {
        Debug.Log("loading songs");
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "SongBlueprint.json");
        Debug.Log(jsonPath);
        string jsonData = null;
#if UNITY_EDITOR || UNITY_STANDALONE
        if (File.Exists(jsonPath))
        {
            jsonData = File.ReadAllText(jsonPath);
            Debug.Log(jsonData);
        }
#elif UNITY_WEBGL
    using (UnityWebRequest www = UnityWebRequest.Get(jsonPath))
    {
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load JSON file: " + www.error);
            yield break; // exit coroutine on error
        }
        jsonData = www.downloadHandler.text;
    }
#endif
        // deserialize JSON data into List<SongBlueprint>
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

    //retrieve midi score beats for a specific song
    public List<float> GetMidiScoreBeats(int songIndex)
    {
        if (AllSongs != null && songIndex >= 0 && songIndex < AllSongs.Count)
        {
            return AllSongs[songIndex].midi_score_beats;
        }
        else
        {
            // Debug.LogError("Invalid song index or songs data is not initialized.");
            return null;
        }
    }
    public List<float> GetMidiScoreDownBeats(int songIndex)
    {
        if (AllSongs != null && songIndex >= 0 && songIndex < AllSongs.Count)
        {
            return AllSongs[songIndex].midi_score_downbeats;
        }
        else
        {
            // Debug.LogError("Invalid song index or songs data is not initialized.");
            return null;
        }
    }
    public List<float> GetMidiOffBeats(int songIndex)
    {
        if (AllSongs != null && songIndex >= 0 && songIndex < AllSongs.Count)
        {
            return AllSongs[songIndex].midi_offbeats;
        }
        else
        {
            return null;
        }
    }
    public List<float> GetMidiEasy(int songIndex)
    {
        if (AllSongs != null && songIndex >= 0 && songIndex < AllSongs.Count)
        {
            return AllSongs[songIndex].easy;
        }
        else
        {
            return null;
        }
    }
    public List<float> GetMidiMedium(int songIndex)
    {
        if (AllSongs != null && songIndex >= 0 && songIndex < AllSongs.Count)
        {
            return AllSongs[songIndex].medium;
        }
        else
        {
            return null;
        }
    }
    public List<float> GetMidiHard(int songIndex)
    {
        if (AllSongs != null && songIndex >= 0 && songIndex < AllSongs.Count)
        {
            return AllSongs[songIndex].hard;
        }
        else
        {
            return null;
        }
    }
}
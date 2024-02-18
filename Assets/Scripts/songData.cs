using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class songData : MonoBehaviour
{
    public static JArray songsData { get; private set; }
    // public static JArray songsBeats { get; private set; }
    void Start()
    {
        // locate and load json file data
        string jsonFilePath = Path.Combine(Application.dataPath + "/scripts", "jsontest.json");
        string jsonContent = File.ReadAllText(jsonFilePath);
        JObject jsonData = JObject.Parse(jsonContent);
        songsData = (JArray)jsonData["songs"];
        // Debug.Log(songsData);
        // Debug.Log("Number of songs: " + (songsData != null ? songsData.Count : 0));
    }
    public static JArray GetMidiScoreBeats(int songIndex)
    {
        if (songsData != null && songIndex >= 0 && songIndex < songsData.Count)
        {
            JObject selectedSong = (JObject)songsData[songIndex];
            var midiScoreBeats = (JArray)selectedSong["midi_score_beats"];
            Debug.Log("Number of beats for song " + songIndex + ": " + (midiScoreBeats != null ? midiScoreBeats.Count : 0));
            return midiScoreBeats;
        }
        else
        {
            Debug.LogError("Invalid song index or missing data.");
            return null; // Handle invalid songIndex or missing data
        }
    }
}

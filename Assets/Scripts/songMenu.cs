using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class songMenu : MonoBehaviour
{
    public TMP_Text songName;
    public string[] sceneNames;
    public string selectedSongName;
    void Start()
    {
        JArray songsData = songData.songsData;
        // Display UI prompt with song names
        string prompt = "Choose a song:\n";
        songName.text = prompt;
        Debug.Log(songsData.Count);
    }

    // Update is called once per frame
    public void DisplaySelectedSongName(int songIndex)
    {
        JArray songsData = songData.songsData;
        // Check if selected index is valid
        if (songIndex >= 1 && songIndex <= songsData.Count)
        {
            // Get the name of the selected song
            string selectedSongName = (string)songsData[songIndex - 1]["name"];
            songName.text = selectedSongName;   
            Debug.Log(selectedSongName);
        }
        else
        {
            songName.text = "Invalid selection.";
        }
    }

    public void chooseLevel(int songIndex)
    {

        if (songIndex >= 1 && songIndex <= sceneNames.Length)
        {
            PlayerPrefs.SetInt("selectedSongIndex", songIndex);
            SceneManager.LoadScene(sceneNames[songIndex - 1]);
            Debug.Log(songName.text + " chosen");
        }
        else
        {
            Debug.LogError("Invalid selection");
        }
    }
}


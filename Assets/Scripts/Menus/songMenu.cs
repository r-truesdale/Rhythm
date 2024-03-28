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
    private List<SongBlueprint> songsData;
    void Start()
    {
        songsData = songData.Instance.AllSongs;

        // // Display UI prompt with song names
        // string prompt = "Choose a song:\n";
        // for (int i = 0; i < songsData.Count; i++)
        // {
        //     prompt += (i + 1) + ". " + songsData[i].name + "\n";
        // }
        // songName.text = prompt;
    }

    // Update is called once per frame
    // public void DisplaySelectedSongName(int songIndex)
    // {
    //     // Check if selected index is valid
    //     if (songIndex >= 1 && songIndex <= songsData.Count)
    //     {
    //         // Get the name of the selected song
    //         
    //         songName.text = selectedSongName;
    //         Debug.Log(selectedSongName);
    //     }
    //     else
    //     {
    //         songName.text = "Invalid selection.";
    //     }
    // }

    // public void chooseLevel(int songIndex)
    // {

    //     if (songIndex >= 1 && songIndex <= sceneNames.Length)
    //     {
    //         PlayerPrefs.SetInt("selectedSongIndex", songIndex-1);
    //         SceneManager.LoadScene(sceneNames[songIndex - 1]);
    //         Debug.Log(sceneNames[songIndex - 1] + " chosen");
    //     }
    //     else
    //     {
    //         Debug.LogError("Invalid selection");
    //     }
    // }

    public void gameLevel(int songIndex)
    {
        // if (songIndex >= 1 && songIndex <= sceneNames.Length)
        // {
        //     PlayerPrefs.SetInt("selectedSongIndex", songIndex - 1);
        //     SceneManager.LoadScene(sceneNames[songIndex - 1]);
        //     Debug.Log(sceneNames[songIndex - 1] + " chosen");
        // }
        // string selectedSongName = songsData[songIndex].name;
        PlayerPrefs.SetInt("selectedSongIndex", songIndex);
        Debug.Log(songIndex);
        PlayerPrefs.SetString("gameState", "game");
        PlayerPrefs.SetString("songName", sceneNames[songIndex]);
        SceneManager.LoadScene(sceneNames[songIndex]);
    }
    public void practiceLevel1(int songIndex)
    {
        PlayerPrefs.SetInt("selectedSongIndex", songIndex);
        PlayerPrefs.SetString("gameState", "practice");
        SceneManager.LoadScene("Song1P");
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void gameMenu()
    {
        SceneManager.LoadScene("gameModeMenu");
    }
    public void practiceMenu()
    {
        SceneManager.LoadScene("practiceModeMenu");
    }
    public void playerStatsMenu()
    {
        SceneManager.LoadScene("playerStats");
    }

    public void statsToMenu(){
        string mode = PlayerPrefs.GetString("gameState");
        if(mode == "game"){
            gameMenu();
        }
        else{
            practiceMenu();
        }
    }
}
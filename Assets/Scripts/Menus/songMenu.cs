using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class songMenu : MonoBehaviour
{
    public string[] sceneNames;
    private List<SongBlueprint> songsData;
    public void getSongData() // retrieves songData list to load correct scenes
    {
        songsData = songData.Instance.AllSongs;
    }
    public void gameLevel(int songIndex)
    {
        getSongData();
        PlayerPrefs.SetInt("selectedSongIndex", songIndex);
        Debug.Log("songIndex = " + songIndex);
        PlayerPrefs.SetString("sceneType", "gameplay");
        PlayerPrefs.SetString("gameState", "game");
        PlayerPrefs.SetString("songName", sceneNames[songIndex]);
        SceneManager.LoadScene(sceneNames[songIndex]);
    }
    public void practiceLevel(int songIndex)
    {
        getSongData();
        PlayerPrefs.SetInt("selectedSongIndex", songIndex);
        Debug.Log("songIndex = " + songIndex);
        PlayerPrefs.SetString("sceneType", "gameplay");
        PlayerPrefs.SetString("gameState", "practice");
        PlayerPrefs.SetString("songName", sceneNames[songIndex]);
        SceneManager.LoadScene(sceneNames[songIndex] + "P");
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PlayerPrefs.SetString("sceneType", "menu");
    }
    public void gameMenu()
    {
        SceneManager.LoadScene("gameModeMenu");
        PlayerPrefs.SetString("sceneType", "menu");
        PlayerPrefs.SetString("gameState", "game");
    }
    public void practiceMenu()
    {
        SceneManager.LoadScene("practiceModeMenu");
        PlayerPrefs.SetString("sceneType", "menu");
        PlayerPrefs.SetString("gameState", "practice");
    }
    public void playerStatsMenu()
    {
        PlayerPrefs.SetString("sceneType", "stats");
        SceneManager.LoadScene("playerStats");
    }
    public void statsToMenu()
    {
        string mode = PlayerPrefs.GetString("gameState");
        if (mode == "game")
        {
            gameMenu();
        }
        else
        {
            practiceMenu();
        }
    }
}
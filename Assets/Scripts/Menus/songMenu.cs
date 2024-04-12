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
 public void getSongData()
 {
  songsData = songData.Instance.AllSongs;
 }
 public void gameLevel(int songIndex)
 {
  getSongData();
  PlayerPrefs.SetInt("selectedSongIndex", songIndex);
  Debug.Log(songIndex);
  PlayerPrefs.SetString("sceneType", "gameplay");
  PlayerPrefs.SetString("gameState", "game");
  PlayerPrefs.SetString("songName", sceneNames[songIndex]);
  SceneManager.LoadScene(sceneNames[songIndex]);
 }
 public void practiceLevel1(int songIndex)
 {
  PlayerPrefs.SetString("sceneType", "gameplay");
  PlayerPrefs.SetInt("selectedSongIndex", songIndex);
  PlayerPrefs.SetString("gameState", "practice");
  SceneManager.LoadScene("Song1P 1");
 }
 public void practiceLevel(int songIndex)
 {
  getSongData();
  PlayerPrefs.SetString("sceneType", "gameplay");
  PlayerPrefs.SetInt("selectedSongIndex", songIndex);
  PlayerPrefs.SetString("gameState", "practice");
  PlayerPrefs.SetString("songName", sceneNames[songIndex]);
  SceneManager.LoadScene(sceneNames[songIndex] + "P");
 }
 public void mainMenu()
 {
    // getSongData();
  SceneManager.LoadScene("MainMenu");
  PlayerPrefs.SetString("sceneType", "menu");
 }
 public void gameMenu()
 {
  SceneManager.LoadScene("gameModeMenu");
  PlayerPrefs.SetString("sceneType", "menu");
 }
 
 public void practiceMenu()
 {
  SceneManager.LoadScene("practiceModeMenu");
  PlayerPrefs.SetString("sceneType", "menu");
 }
 public void playerStatsMenu()
 {
  PlayerPrefs.SetString("sceneType", "stats");
  SceneManager.LoadScene("playerStats");
 }

 public void statsToMenu()
 {
  PlayerPrefs.SetString("sceneType", "menu");
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
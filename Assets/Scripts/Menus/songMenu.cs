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
 }
 public void gameLevel(int songIndex)
 {
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
  public void practiceLevel(int songIndex)
 {
  PlayerPrefs.SetInt("selectedSongIndex", songIndex);
  PlayerPrefs.SetString("gameState", "practice");
  SceneManager.LoadScene(sceneNames[songIndex]+"P");
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songEnd : MonoBehaviour
{
    // bool songStatus;
    private GameObject _SongEndUI;
    private bool scoreGraphCalled = false;
    // Start is called before the first frame update
    public GameObject songEndUI
    {
        get { return _SongEndUI; }
        private set { _SongEndUI = value; }
    }

    void Start()
    {
        songEndUI = GameObject.Find("SongEndUI");
        if (songEndUI != null)
        {
            songEndUI.SetActive(false);
        }
        else
        {
            Debug.LogError("SongEndUI GameObject not found!");
        }
    }
    void Update()
    {
        // if (PlayerPrefs.GetString("gameState") == "game")
        // {
        // If the game state is set to "game", check the song status
        bool songStatus = GameManager.Instance.songStatus();
        bool gameStarted = GameManager.Instance.checkGameStart();
        bool gameEntered = GameManager.Instance.checkGameEntered();
        if (!songStatus && gameStarted && !scoreGraphCalled && gameEntered)
        {
            // Debug.Log("Song has ended");
            if (songEndUI != null)
            {
                songEndUI.SetActive(true);
            }
            else
            {
                // Debug.LogError("SongEndUI GameObject not found!");
            }
            ScoreManager.Instance.getScoreGraph();
            scoreGraphCalled = true;
        }
        else if (songStatus && gameStarted)
        {
            // Debug.Log("Song is still playing");
            songEndUI.SetActive(false);
        }
        else if (!songStatus && !gameStarted)
        {//for when song hasn't started playing yet at start
        //  Debug.Log("Game hasn't started");
        songEndUI.SetActive(false);
        }
        // }
    }
}
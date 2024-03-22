using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songEnd : MonoBehaviour
{
    // bool songStatus;
    public GameObject SongEndUI;
    private bool scoreGraphCalled = false;
    // Start is called before the first frame update
    void Start()
    {
        SongEndUI.SetActive(false); 
    }

    void Update()
    {
        // if (PlayerPrefs.GetString("gameState") == "game")
        // {
        // If the game state is set to "game", check the song status
        bool songStatus = GameManager.Instance.songStatus();
        bool gameStarted = GameManager.Instance.checkGameStart();
        if (!songStatus && gameStarted && !scoreGraphCalled)
        {
            // Debug.Log("Song has ended");
            SongEndUI.SetActive(true);
            ScoreManager.Instance.getScoreGraph();
            scoreGraphCalled = true;
        }
        else if (songStatus && gameStarted)
        {
            // Debug.Log("Song is still playing");
        }
        else if (!songStatus && !gameStarted){//for when song hasn't started playing yet at start
        // Debug.Log("Game hasn't started");
        }
        // }
    }
}
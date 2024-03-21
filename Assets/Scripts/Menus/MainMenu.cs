using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void practiceModePlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("practiceModeMenu");
    }
        public void gameModePlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("gameModeMenu");
    }
}

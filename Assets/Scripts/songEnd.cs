using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songEnd : MonoBehaviour
{
    bool songStatus;
    // Start is called before the first frame update
    void Start()
    {

    }

void Update(){
            songStatus = GameManager.Instance.songEnded();
        if (songStatus==true)
        {
            Debug.Log("Song has ended");
            // Add your song end actions here, such as game over logic
        }
        if (songStatus==false)
        {
            Debug.Log("Song is still playing");
        }
}
}
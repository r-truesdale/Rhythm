using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
public class arrows : MonoBehaviour
{
    public float speed = 1.0f;
    public float arrowSpawnTime;
    public float beatTime;
    public int hitBoxIndex;
    void Update()
    {
        // Move the arrow continuously
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    public void SetHitBoxIndex(int index)
    {
        hitBoxIndex = index;
        if (hitBoxIndex == 4)
        { //if it gets to the latemiss hitbox, destroy
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        // If the arrow becomes invisible, destroy it
        Destroy(gameObject);
    }

    // void CheckArrowSpawn()
    // {
    //     if (!gameStarted) // Check if the game has started
    //         return;

    //     if (spawningPaused)
    //         return;

    //     if (gameStarted)
    //     {
    //         float gamePosition = Time.time - gameStartTime;
    //         float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
    //         bool arrowSpawnedThisFrame = false; // Flag to track if an arrow has been spawned in this frame
    //         for (int i = 0; i < midiScoreBeats.Count; i++)
    //         {
    //             float beatTime = midiScoreBeats[i];
    //             float arrowSpawnTime = beatTime + GetTimeToHitbox() - 0.5f;
    //             // Debug.Log((arrowSpawnTime));
    //             float arrowSpeed = arrowPrefab.GetComponent<arrows>().speed;
    //             if (i < arrowsSpawned.Length && !arrowSpawnedThisFrame && !arrowsSpawned[i] && gamePosition >= arrowSpawnTime)
    //             {
    //                 GameObject newArrow = SpawnManager.Instance.SpawnArrow(arrowPrefab, arrowSpawnTime, beatTime, arrowSpeed); // Spawn the arrow earlier
    //                 spawnedArrows.Add(newArrow);
    //                 arrowsSpawned[i] = true;
    //                 arrowSpawnedThisFrame = true; // Set the flag to true since an arrow has been spawned in this frame
    //                 beatsNum.text = beatTime.ToString();
    //                 break;
    //             }
    //         }
    //     }
    // }
}
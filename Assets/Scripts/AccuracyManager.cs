using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccuracyManager : MonoBehaviour
{
    public static AccuracyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CalculateAccuracyAndScore(float arrowBeat, float currentPlaybackTime, int hitBoxIndex)
    {
        Debug.Log("calcscore");
        Debug.Log("Arrow Beat: " + arrowBeat);
        Debug.Log("Current Playback Time: " + currentPlaybackTime);
        Debug.Log("Hitbox Index: " + hitBoxIndex);
        // Calculate the timing accuracy (absolute difference between current MIDI score beat and current playback time)
        float timingAccuracy = Mathf.Abs(arrowBeat - currentPlaybackTime);

        // Depending on the hitbox index, adjust scoring logic
        switch (hitBoxIndex)
        {
            case 0: // Too early
                Debug.Log("Timing accuracy: " + timingAccuracy);
                Debug.Log("Too early! Beat: " + arrowBeat);
                ScoreManager.Instance.AddScore(-50); // Deduct points for too early hits
                break;
            case 1: // Perfect
                // Assuming a threshold of 0.1 seconds for perfect timing accuracy (adjust as needed)
                if (timingAccuracy < 0.1f)
                {
                    Debug.Log("Timing accuracy: " + timingAccuracy);
                    Debug.Log("Perfect timing accuracy! Beat: " + arrowBeat);
                    ScoreManager.Instance.AddScore(100); // Award maximum points for perfect hits
                }
                break;
            case 2: // Too late
                Debug.Log("Timing accuracy: " + timingAccuracy);
                Debug.Log("Too late! Beat: " + arrowBeat);
                ScoreManager.Instance.AddScore(-50); // Deduct points for too late hits
                break;
            default:
                Debug.LogWarning("Invalid hitbox index.");
                break;
        }
    }
}
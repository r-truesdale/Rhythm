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

    public void CalculateAccuracyAndScore(float beatTime, float currentPlaybackTime, int hitBoxIndex)
    {
        // Debug.Log("Arrow Beat: " + beatTime);
        // Debug.Log("Current Playback Time: " + currentPlaybackTime);
        // Debug.Log("Hitbox Index: " + hitBoxIndex);
        // Calculate the timing accuracy (absolute difference between current MIDI score beat and current playback time)
        float timingAccuracy = Mathf.Abs(beatTime - currentPlaybackTime);
        // Debug.Log("Timing Accuracy: " + timingAccuracy);
        switch (hitBoxIndex)
        {
            case 0: // perfect
                // if (timingAccuracy < 0.5f)
                {
                    Debug.Log("Timing accuracy: " + timingAccuracy);
                    Debug.Log("Perfect timing accuracy! Beat: " + beatTime);
                    ScoreManager.Instance.AddScore(100); // Award maximum points for perfect hits
                }
                break;
            case 1: // too early
                // if (timingAccuracy < 0)
                {
                    Debug.Log("Timing accuracy: " + timingAccuracy);
                    Debug.Log("Too early! Beat: " + beatTime);
                    ScoreManager.Instance.AddScore(-50); // Deduct points for too early hits
                }
                break;
            case 2: // Too late
                // if (timingAccuracy >= 0.5f)
                {
                    Debug.Log("Timing accuracy: " + timingAccuracy);
                    Debug.Log("Too late! Beat: " + beatTime);
                    ScoreManager.Instance.AddScore(-50); // Deduct points for too late hits
                }
                break;
            default:
                Debug.LogWarning("Invalid hitbox index.");
                break;
        }
    }
}
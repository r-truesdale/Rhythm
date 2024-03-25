using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccuracyManager : MonoBehaviour
{
    public static AccuracyManager Instance { get; private set; }

    private string accuracyResult;

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
        // Debug.Log("Current Playback Time: " + currentPlaybackTime);
        // Debug.Log("Hitbox Index: " + hitBoxIndex);
        accuracyResult = "miss";
        // Calculate the timing accuracy (absolute difference between current MIDI score beat and current playback time)
        float timingAccuracy = Mathf.Abs(beatTime - currentPlaybackTime);
        // Debug.Log("Timing Accuracy: " + timingAccuracy);
        switch (hitBoxIndex)
        {
            case 0: // perfect
                    // if (timingAccuracy < 0.1f)
                    // {
                Debug.Log("Perfect timing accuracy! Beat: " + beatTime + "playback:" + currentPlaybackTime + "accuracy:" + timingAccuracy);
                accuracyResult = "perfect";
                ScoreManager.Instance.AddScore(100); // Award maximum points for perfect hits
                ScoreManager.Instance.AddPerfect(1);
                // }
                break;
            case 1: // too early
                Debug.Log("Too early! Beat: " + beatTime + "playback:" + currentPlaybackTime + "accuracy:" + timingAccuracy);
                accuracyResult = "early";
                ScoreManager.Instance.AddScore(-50); // Deduct points for too early hits
                ScoreManager.Instance.AddEarly(1);
                break;
            case 2: // Too late
                Debug.Log("Too Late! Beat: " + beatTime + "playback:" + currentPlaybackTime + "accuracy:" + timingAccuracy);
                accuracyResult = "late";
                ScoreManager.Instance.AddScore(-50); // Deduct points for too late hits
                ScoreManager.Instance.AddLate(1);
                break;
            case 3: // Too late
                Debug.Log("Early Miss! Beat: " + beatTime + "playback:" + currentPlaybackTime + "accuracy:" + timingAccuracy);
                accuracyResult = "late";
                ScoreManager.Instance.AddScore(-100); // Deduct points for too late hits
                ScoreManager.Instance.AddEarly(1);
                break;
            case 4: // Too late
                Debug.Log("Late Miss! Beat: " + beatTime + "playback:" + currentPlaybackTime + "accuracy:" + timingAccuracy);
                accuracyResult = "late";
                ScoreManager.Instance.AddScore(-100); // Deduct points for too late hits
                ScoreManager.Instance.AddLate(1);
                break;
            default:
                Debug.LogWarning("Invalid hitbox index.");
                break;
        }
    }
    public string GetAccuracyResult()
    {
        return accuracyResult;
    }
}
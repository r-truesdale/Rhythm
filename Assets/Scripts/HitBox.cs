using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HitBox : MonoBehaviour
{
    public string hitBoxName;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "Arrow" tag
        if (other.CompareTag("Arrow"))
        {
            // Retrieve the arrows script component
            arrows arrow = other.GetComponent<arrows>();
            // Ensure the arrows script component is not null
            if (arrow != null)
            {
                float currentPlaybackTime = GameManager.Instance.GetPlaybackTime();
                // Get the hitbox index
                int hitBoxIndex = GetHitBoxIndex();
                // Calculate accuracy and score
                AccuracyManager.Instance.CalculateAccuracyAndScore(arrow.arrowBeat, currentPlaybackTime, hitBoxIndex);
                // Destroy the arrow
                Destroy(other.gameObject);
            }
        }
    }

public int GetHitBoxIndex()
{
    // Determine the hitbox index based on its name or position in the array
    switch (hitBoxName)
    {
        case "TooEarly":
            Debug.Log("Hitbox Name: TooEarly");
            return 0;
        case "Perfect":
            Debug.Log("Hitbox Name: Perfect");
            return 1;
        case "TooLate":
            Debug.Log("Hitbox Name: TooLate");
            return 2;
        default:
            Debug.LogWarning("Invalid hitbox name: " + hitBoxName);
            return -1; // Invalid hitbox
    }
}
    public void ProcessHit(float arrowBeat, float currentPlaybackTime, int hitBoxIndex)
    {
        // Call the AccuracyManager to calculate accuracy and score
        AccuracyManager.Instance.CalculateAccuracyAndScore(arrowBeat, currentPlaybackTime, hitBoxIndex);
    }
}

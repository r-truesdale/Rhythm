using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
public class HitBox : MonoBehaviour
{
    public string hitBoxName;
    private float perfectHitboxSize = 1.0f;
    private float otherHitboxMultiplier = 1f;
    private BeatObjects BeatObject;
    private float perfectTimingWindow = 0.2f;
    private Transform perfectSpawnPoint;
    private Transform earlySpawnPoint;
    private Transform lateSpawnPoint;
    private Transform earlyMissSpawnPoint;
    private Transform lateMissSpawnPoint;
    private float oldPerfectHitboxSize;
    public float[] perfectHitboxYPositions;
    [SerializeField] private RectTransform hitboxContainer;
    public void Start()
    {
        perfectHitboxYPositions = new float[] { -50.0f, -25.0f, 0f, 25f };
        GameObject BeatObjectPrefab = SpawnManager.Instance.BeatObjectPrefab;
        perfectSpawnPoint = GameObject.Find("Perfect").transform;
        earlySpawnPoint = GameObject.Find("TooEarly").transform;
        lateSpawnPoint = GameObject.Find("TooLate").transform;
        earlyMissSpawnPoint = GameObject.Find("EarlyMiss").transform;
        lateMissSpawnPoint = GameObject.Find("LateMiss").transform;
        BeatObjects BeatObject = BeatObjectPrefab.GetComponent<BeatObjects>();
        if (BeatObject != null)
        {
            float BeatObjectspeed = BeatObject.speed;
            oldPerfectHitboxSize = perfectHitboxSize;
            perfectHitboxSize = BeatObjectspeed * perfectTimingWindow;
        }
        else
        {
            Debug.LogError("BeatObject prefab not found or BeatObjects script not attached to prefab.");
        }
        SetHitBoxSize();
        setPosition();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beat"))
        {
            //get BeatObjects script component
            BeatObjects BeatObject = other.GetComponent<BeatObjects>();
            if (BeatObject != null)
            {
                int hitBoxIndex = GetHitBoxIndex();
                BeatObject.SetHitBoxIndex(hitBoxIndex);
            }
            else
            {
                Debug.LogError("BeatObjects component not found on the object triggering the collider.");
            }
        }
    }
    public int GetHitBoxIndex()
    {
        switch (hitBoxName)
        {
            case "Perfect":
                return 0;
            case "TooEarly":
                return 1;
            case "TooLate":
                return 2;
            case "EarlyMiss":
                return 3;
            case "LateMiss":
                return 4;
            default:
                Debug.LogWarning("Invalid hitbox name: " + hitBoxName);
                return -1;
        }
    }
    private void SetHitBoxSize()
    {
        transform.localScale = new Vector3(perfectHitboxSize, transform.localScale.y, transform.localScale.z);
    }

    private void setPosition()
    {
        float sizeDifference = perfectHitboxSize - oldPerfectHitboxSize;
        if (hitBoxName == "Perfect" && perfectSpawnPoint != null)
            setScale(perfectSpawnPoint.position, perfectHitboxSize);
        else if (hitBoxName == "TooEarly" && earlySpawnPoint != null)
            newScale(earlySpawnPoint.position, oldPerfectHitboxSize);
        else if (hitBoxName == "TooLate" && lateSpawnPoint != null)
            newScale(lateSpawnPoint.position, -oldPerfectHitboxSize);
        else if (hitBoxName == "EarlyMiss" && earlyMissSpawnPoint != null)
            newScale(earlyMissSpawnPoint.position, oldPerfectHitboxSize * otherHitboxMultiplier);
        else if (hitBoxName == "LateMiss" && lateMissSpawnPoint != null)
            newScale(lateMissSpawnPoint.position, -oldPerfectHitboxSize * otherHitboxMultiplier);
        else
            Debug.LogError("Spawn Point not assigned for hitbox: " + hitBoxName);
    }
    private void setScale(Vector3 position, float size)
    {
        transform.position = position;
        transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
    }
    private void newScale(Vector3 position, float sizeDifference)
    {
        float earlyMissSize = (hitboxContainer.rect.width - (perfectHitboxSize * 3) - (sizeDifference * 3));
        float lateMissSize = (hitboxContainer.rect.width - (perfectHitboxSize * 3) - earlyMissSize);
        if (hitBoxName == "EarlyMiss")
        {
            transform.position = position + Vector3.right * (sizeDifference);
            transform.localScale = new Vector3(earlyMissSize - (perfectHitboxSize / 2), transform.localScale.y, transform.localScale.z);
        }
        if (hitBoxName == "LateMiss")
        {
            transform.position = position + Vector3.right * (sizeDifference * 2);
            transform.localScale = new Vector3(lateMissSize, transform.localScale.y, transform.localScale.z);
        }
        else if (hitBoxName == "Perfect" || hitBoxName == "TooEarly" || hitBoxName == "TooLate")
        {
            transform.position = position + Vector3.right * sizeDifference;
            transform.localScale = new Vector3(perfectHitboxSize, transform.localScale.y, transform.localScale.z);
        }
    }
    public bool ProcessHit(float beatTime, float currentPlaybackTime, int hitBoxIndex) //processes when player presses input key
    {
        // call accuracyManager to calculate accuracy and score
        AccuracyManager.Instance.CalculateAccuracyAndScore(beatTime, currentPlaybackTime, hitBoxIndex);
        return true; //makes sure the same beat can't be processed twice
    }
    public void moveHitbox(float direction) // only called in game mode, moves the perfect hitbox to predetermined positions 
    {
        int currentPositionIndex = currentPerfectIndex();
        int newPositionIndex = currentPositionIndex + Mathf.RoundToInt(direction);

        if (newPositionIndex >= 0 && newPositionIndex < perfectHitboxYPositions.Length)
        {
            transform.position = new Vector3(transform.position.x, perfectHitboxYPositions[newPositionIndex], transform.position.z);
            Debug.Log("New Position: " + newPositionIndex);
        }
        else
        {
            Debug.LogWarning("Invalid position index for perfect hitbox.");
        }
    }

    private int currentPerfectIndex() // gets the current place in the array the perfect hitbox is positioned in 
    {
        for (int i = 0; i < perfectHitboxYPositions.Length; i++)
        {
            if (Mathf.Approximately(transform.position.y, perfectHitboxYPositions[i]))
            {
                return i;
            }
        }
        return -1;
    }
}
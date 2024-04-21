using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
public class HitBox : MonoBehaviour
{
    public string hitBoxName;
    private float perfectHitboxSize = 1.0f;
    private float otherHitboxMultiplier = 1f;
    private arrows arrow;
    private float perfectTimingWindow = 0.2f;
    private Transform perfectSpawnPoint;
    private Transform earlySpawnPoint;
    private Transform lateSpawnPoint;
    private Transform earlyMissSpawnPoint;
    private Transform lateMissSpawnPoint;
    private float oldPerfectHitboxSize;
    public float[] perfectHitboxYPositions;
    [SerializeField] private RectTransform hitboxContainer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            //get arrows script component
            arrows arrow = other.GetComponent<arrows>();
            if (arrow != null)
            {
                int hitBoxIndex = GetHitBoxIndex();
                arrow.SetHitBoxIndex(hitBoxIndex);
            }
            else
            {
                Debug.LogError("Arrows component not found on the object triggering the collider.");
            }
        }
    }

    public int GetHitBoxIndex()
    {
        switch (hitBoxName)
        {
            case "Perfect":
                // Debug.Log("Hitbox Name: Perfect");
                return 0;
            case "TooEarly":
                // Debug.Log("Hitbox Name: TooEarly");
                return 1;
            case "TooLate":
                // Debug.Log("Hitbox Name: TooLate");
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
    public void Start()
    {
        perfectHitboxYPositions = new float[] { -50.0f, -25.0f, 0f, 25f };
        GameObject arrowPrefab = SpawnManager.Instance.arrowPrefab;
        perfectSpawnPoint = GameObject.Find("Perfect").transform;
        earlySpawnPoint = GameObject.Find("TooEarly").transform;
        lateSpawnPoint = GameObject.Find("TooLate").transform;
        earlyMissSpawnPoint = GameObject.Find("EarlyMiss").transform;
        lateMissSpawnPoint = GameObject.Find("LateMiss").transform;
        arrows arrow = arrowPrefab.GetComponent<arrows>();
        if (arrow != null)
        {
            float arrowSpeed = arrow.speed;
            oldPerfectHitboxSize = perfectHitboxSize;
            perfectHitboxSize = arrowSpeed * perfectTimingWindow;
        }
        else
        {
            Debug.LogError("Arrow prefab not found or arrows script not attached to prefab.");
        }
        SetHitBoxSize();
        setPosition();
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
        {//(perfect hitboxsize *3)/2  = perfecthitbox size
            transform.position = position + Vector3.right * (sizeDifference);
            transform.localScale = new Vector3(earlyMissSize - (perfectHitboxSize / 2), transform.localScale.y, transform.localScale.z);
        }
        if (hitBoxName == "LateMiss")
        {//extra size difference so beat has fully left late hitbox before being registered as latemiss
            transform.position = position + Vector3.right * (sizeDifference * 2);
            transform.localScale = new Vector3(lateMissSize, transform.localScale.y, transform.localScale.z);
        }
        else if (hitBoxName == "Perfect" || hitBoxName == "TooEarly" || hitBoxName == "TooLate")
        {
            transform.position = position + Vector3.right * sizeDifference;
            transform.localScale = new Vector3(perfectHitboxSize, transform.localScale.y, transform.localScale.z);
        }
    }
    private HitBox FindHitBox(string hitBoxName)
    {
        foreach (Transform child in transform.parent)
        {
            if (child.CompareTag("HitBox") && child.GetComponent<HitBox>().hitBoxName == hitBoxName)
            {
                return child.GetComponent<HitBox>();
            }
        }
        Debug.Log("null");
        return null;
    }
    public bool ProcessHit(float beatTime, float currentPlaybackTime, int hitBoxIndex)
    {
        // call accuracyManager to calculate accuracy and score
        AccuracyManager.Instance.CalculateAccuracyAndScore(beatTime, currentPlaybackTime, hitBoxIndex);
        return true;
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
        Debug.LogWarning("Current position not found in perfect hitbox positions array");
        return -1;
    }
}
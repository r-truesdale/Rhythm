using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

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

    public GameObject SpawnArrow(GameObject arrowPrefab, float arrowSpawnTime, float beatTime)
    {
        // Instantiate the arrow prefab at a random position within a range
        GameObject newArrow = Instantiate(arrowPrefab, GetArrowSpawnPosition(), Quaternion.identity);

        // Assign arrowSpawnTime and beatTime to the arrow
        arrows arrowScript = newArrow.GetComponent<arrows>();
        if (arrowScript != null)
        {
            arrowScript.arrowSpawnTime = arrowSpawnTime;
            arrowScript.beatTime = beatTime;
        }
        else
        {
            Debug.LogError("Arrows component not found on arrowPrefab.");
        }
        return newArrow;
    }
    public Vector3 GetArrowSpawnPosition()
    {
        return new Vector3(Random.Range(-120f, 100f), 116f, -2.5f);
    }
}
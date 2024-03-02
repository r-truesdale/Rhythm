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

    public GameObject SpawnArrow(GameObject arrowPrefab, float arrowBeat)
    {
        // Instantiate the arrow prefab at a random position within a range
        //using getarrowspawnposition bc i need only that in gamemanager
        GameObject newArrow = Instantiate(arrowPrefab, GetArrowSpawnPosition(), Quaternion.identity);
        newArrow.GetComponent<arrows>().arrowBeat = arrowBeat;
        return newArrow;
    }
        public Vector3 GetArrowSpawnPosition()
    {
        return new Vector3(Random.Range(-120f, 100f), 116f, -2.5f);
    }
}
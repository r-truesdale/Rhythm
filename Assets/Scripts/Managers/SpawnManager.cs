using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using MidiPlayerTK;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    [Header("Objects")]
    [SerializeField] private MidiFilePlayer midiFilePlayer;
    [SerializeField] private List<HitBox> hitBoxes = new List<HitBox>();
    [SerializeField] private List<float> midiScoreBeats = new List<float>(); // Reference to the MIDI score beat
    public GameObject BeatObjectPrefab; // public so hitbox class can access for speed

    [Header("Beat Spawning")]
    [SerializeField] private bool[] BeatObjectsSpawned;
    public List<GameObject> spawnedBeatObjects = new List<GameObject>(); // Track spawned BeatObjects
    public bool initialized = false;
    private bool spawningPaused = false;
    private bool BeatObjectVisibilityOff = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            spawningPaused = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (PlayerPrefs.GetString("sceneType") == "gameplay") //only starts if correct scene type
        {
            findObjects();
            CheckBeatObjectspawn();
            initialize();
            if (midiScoreBeats != null)
            {
                BeatObjectsSpawned = new bool[midiScoreBeats.Count];
                for (int i = 0; i < BeatObjectsSpawned.Length; i++)
                {
                    BeatObjectsSpawned[i] = false;
                }
            }
            else
            {
                Debug.LogError("Failed to load song data. midiScoreBeats is null.");
            }
        }
    }
    void Update()
    {
        GameManager.Instance.checkGameStart();
        CheckBeatObjectspawn();
        HandlePlayerInput();
        if (PlayerPrefs.GetString("gameState") == "game") //only for game mode, handles box movement 
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("move1");
                hitBoxes[0].moveHitbox(1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Debug.Log("move-1");
                hitBoxes[0].moveHitbox(-1);
            }
        }
    }
    public void findObjects()
    {
        if (midiFilePlayer == null)
        {
            Debug.Log("SpawnManager: Find MIDIPlayer and Hitboxes");
            midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                FindHitboxesInHierarchy(rootObject.transform);
            }
        }
    }
    private void FindHitboxesInHierarchy(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // check if current child has "Hitbox" tag
            if (child.CompareTag("HitBox"))
            {
                HitBox hitBoxComponent = child.GetComponent<HitBox>();
                if (hitBoxComponent != null)
                {
                    hitBoxes.Add(hitBoxComponent);
                }
                else
                {
                    Debug.LogError("HitBox component not found on 'Hitbox' tagged GameObject");
                }
            }
            //search child objects
            FindHitboxesInHierarchy(child);
        }
    }
    public void initialize()//sets up correct number of beats to spawn
    {
        InitializeBeatObjectsSpawned(midiScoreBeats.Count);
        InitializeMidiScoreBeats(midiScoreBeats);
    }

    public void InitializeBeatObjectsSpawned(int count)
    {
        BeatObjectsSpawned = new bool[count];
        for (int i = 0; i < BeatObjectsSpawned.Length; i++)
        {
            BeatObjectsSpawned[i] = false;
        }
        initialized = true;
        Debug.Log("SpawnManager: BeatObjects spawned initialized");
    }

    public void InitializeMidiScoreBeats(List<float> scoreBeats)
    {
        midiScoreBeats = new List<float>(scoreBeats);
        Debug.Log("SpawnManager: Midi beats initialized");
    }

    //----------------------------- Spawning objects and  Processing Hits------------------------------------

    void CheckBeatObjectspawn() //check if a beat needs to spawn, spawn timings and component values
    {
        if (spawningPaused || GameManager.Instance == null)
            return;
        if (!GameManager.Instance.checkGameStart())
            return;

        if (GameManager.Instance.levelPlaying && GameManager.Instance.songStatus() && GameManager.Instance.checkGameStart())
        {
            float elapsedTime = GameManager.Instance.CheckElapsedTime();
            float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
            bool BeatObjectspawnedThisFrame = false; // track if an BeatObject has been spawned in this frame
            for (int i = 0; i < midiScoreBeats.Count; i++)
            {
                float beatTime = midiScoreBeats[i];
                float BeatObjectspawnTime = beatTime - GetTimeToHitbox();
                float BeatObjectspeed = BeatObjectPrefab.GetComponent<BeatObjects>().speed;
                if (i < BeatObjectsSpawned.Length && !BeatObjectspawnedThisFrame && !BeatObjectsSpawned[i] && elapsedTime >= BeatObjectspawnTime)
                {
                    GameObject newBeatObject = SpawnBeatObject(BeatObjectPrefab, BeatObjectspawnTime, beatTime, BeatObjectspeed); // spawn the BeatObject earlier
                    spawnedBeatObjects.Add(newBeatObject);
                    BeatObjectsSpawned[i] = true;
                    BeatObjectspawnedThisFrame = true;
                    break;
                }
            }
        }
    }

    //game object for the beats 
    public GameObject SpawnBeatObject(GameObject BeatObjectPrefab, float BeatObjectspawnTime, float beatTime, float BeatObjectspeed)
    {
        // instantiate BeatObject prefab
        GameObject newBeatObject = Instantiate(BeatObjectPrefab, GetBeatObjectspawnPosition(), Quaternion.identity);
        float[] perfectHitboxYPositions = FindObjectOfType<HitBox>().perfectHitboxYPositions;
        // assign BeatObjectspawnTime and beatTime to the BeatObject
        if (PlayerPrefs.GetString("gameState") == "game")
        {//BeatObjects spawning randomly in the ypositions
            float randomY = perfectHitboxYPositions[UnityEngine.Random.Range(0, perfectHitboxYPositions.Length)];
            Vector3 spawnPosition = GetBeatObjectspawnPosition();
            spawnPosition.y = randomY;
            newBeatObject.transform.position = spawnPosition;
        }
        //Beat Visibility toggle only avalible to change in practice mode
        if (BeatObjectVisibilityOff) //removing mesh rederer so beat is not visible but still exists 
        {
            Transform sphere = newBeatObject.transform.Find("Sphere"); // sphere object is a child of the full BeatObject prefab
            if (sphere != null)
            {
                MeshRenderer renderer = sphere.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
        BeatObjects BeatObjectscript = newBeatObject.GetComponent<BeatObjects>();
        if (BeatObjectscript != null)
        {
            BeatObjectscript.BeatObjectspawnTime = BeatObjectspawnTime;
            BeatObjectscript.beatTime = beatTime;
            BeatObjectscript.speed = BeatObjectspeed;
        }
        else
        {
            Debug.LogError("BeatObjects component not found on BeatObjectPrefab.");
        }
        return newBeatObject;
    }

    public Vector3 GetBeatObjectspawnPosition() // Practice mode only, when they don't change y-position
    {
        return new Vector3(120f, 0f, 40f);
    }

    private float GetTimeToHitbox() // time it takes from beat spawn position to the perfect area
    {
        if (BeatObjectPrefab != null && hitBoxes.Count > 0)
        {
            // time it takes for BeatObject to reach  hitbox based on speed and hitbox position
            float hitBoxDistance = Vector3.Distance(GetBeatObjectspawnPosition(), hitBoxes[0].transform.position);
            BeatObjects BeatObject = BeatObjectPrefab.GetComponent<BeatObjects>(); // get the BeatObjects component from the BeatObjectPrefab
            if (BeatObject != null)
            {
                return hitBoxDistance / BeatObject.speed;
            }
            else
            {
                Debug.LogError("SpawnManager: BeatObjects component not found on BeatObjectPrefab");
                return 0f;
            }
        }
        else
        {
            Debug.LogError("SpawnManager: BeatObjectPrefab or HitBoxes not assigned");
            return 0f;
        }
    }

    public void ProcessBeatObjectHit(BeatObjects BeatObjectscript)
    {
        int hitBoxIndex = BeatObjectscript.hitBoxIndex;
        float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
        HitBox hitBox = hitBoxes[hitBoxIndex];
        if (hitBox != null)
        {
            hitBox.ProcessHit(BeatObjectscript.beatTime, currentPosition, hitBoxIndex);
        }
        else
        {
            Debug.LogError("SpawnManager: HitBox not found for BeatObject.");
        }
        // remove BeatObject from spawnedBeatObjects list and destroy
        spawnedBeatObjects.Remove(BeatObjectscript.gameObject);
        Destroy(BeatObjectscript.gameObject);
    }

    void HandlePlayerInput() //checking player inputs and which beat objects need to have the hit processed
    {
        if (spawningPaused || GameManager.Instance == null)
            return;
        if (!GameManager.Instance.levelPlaying)
            return;
        if (Input.GetKeyDown(GameManager.Instance.getInputKey()))
        {
            for (int i = 0; i < spawnedBeatObjects.Count; i++)
            {
                GameObject BeatObjectObject = spawnedBeatObjects[i];
                // check if BeatObject object active
                if (BeatObjectObject != null && BeatObjectObject.activeSelf)
                {
                    BeatObjects BeatObjectscript = BeatObjectObject.GetComponent<BeatObjects>();
                    if (BeatObjectscript != null)
                    {
                        int hitBoxIndex = BeatObjectscript.hitBoxIndex;
                        HitBox hitBox = hitBoxes[hitBoxIndex];
                        if (hitBox != null)
                        {
                            hitBox.ProcessHit(BeatObjectscript.beatTime, (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds, hitBoxIndex);
                        }
                        else //there should always be a hitbox 
                        {
                            Debug.LogError("SpawnManager: HitBox not found for BeatObject.");
                        }
                        // remove BeatObject from list then distroy
                        spawnedBeatObjects.RemoveAt(i);
                        Destroy(BeatObjectObject);
                        //exit loop
                        break;
                    }
                }
            }
        }
    }
    //----------------------------- Practice Mode Only --------------------------------

    public void setBeatObjectVisibility()
    {
        BeatObjectVisibilityOff = true;
    }

    public void setBeatObjectVisibilityOn()
    {
        BeatObjectVisibilityOff = false;
    }

    //----------------------------- Pausing/Ending Level --------------------------------

    public void pauseBeatObjects() //stops objects from moving as part of level pause
    {
        foreach (GameObject BeatObjectObject in spawnedBeatObjects)
        {
            if (BeatObjectObject != null)
            {
                Rigidbody2D rb = BeatObjectObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }

    public void levelReset() //clears all the lists
    {
        spawnedBeatObjects.Clear();
        Array.Clear(BeatObjectsSpawned, 0, BeatObjectsSpawned.Length);
        hitBoxes.Clear();
        midiScoreBeats.Clear();
    }

    public void destroyBeatObjects()//ref in gameUI
    {
        foreach (GameObject BeatObjectObject in spawnedBeatObjects)
        {
            if (BeatObjectObject != null)
            {
                Destroy(BeatObjectObject);
            }
        }
    }

    public void stopSpawn() //pausing beat movement and spawning
    {
        pauseBeatObjects();
        spawningPaused = true;
    }

    public void playSpawn()
    {
        spawningPaused = false;
    }
}
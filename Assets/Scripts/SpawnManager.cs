using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using MidiPlayerTK;
public class SpawnManager : MonoBehaviour
{
 public static SpawnManager Instance { get; private set; }
 [SerializeField] private MidiFilePlayer midiFilePlayer; // Reference to the MidiFilePlayer
 // [SerializeField] private GameManager gameManager; // Reference to the GameManager
 [SerializeField] private GameObject arrowPrefab; // Reference to the arrow prefab
 [SerializeField] private List<HitBox> hitBoxes = new List<HitBox>(); // Reference to the hitboxes
 [SerializeField] private List<float> midiScoreBeats = new List<float>(); // Reference to the MIDI score beats
 [SerializeField] private bool[] arrowsSpawned;
 public List<GameObject> spawnedArrows = new List<GameObject>(); // Track spawned arrows
 public bool initialized = false;

 // private bool gameStarted = false;
 private float gameStartTime = 0f;
 private void Awake()
 {
  if (Instance == null)
  {
   Instance = this;
   findObjects();
  }
  else
  {
   Destroy(gameObject);
  }
 }
 // public bool checkGameStarted(){
 //  if(GameManager.Instance.checkGameStart){
 //   return true;
 //  }else
 //  {
 //   return false;
 //  }
 // }
 void Start()
 {
  // findObjects();
  CheckArrowSpawn();
  initialize();
  if (midiScoreBeats != null)
  {
   arrowsSpawned = new bool[midiScoreBeats.Count];
   for (int i = 0; i < arrowsSpawned.Length; i++)
   {
    arrowsSpawned[i] = false;
   }
  }
  else
  {
   Debug.LogError("Failed to load song data. midiScoreBeats is null.");
  }
 }

 public void findObjects()
 {
  midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
  GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
  foreach (GameObject rootObject in rootObjects)
  {
   FindHitboxesInHierarchy(rootObject.transform);
  }
 }
 private void FindHitboxesInHierarchy(Transform parent)
 {
  foreach (Transform child in parent)
  {
   // Check if the current child has the "Hitbox" tag
   if (child.CompareTag("HitBox"))
   {
    HitBox hitBoxComponent = child.GetComponent<HitBox>();
    if (hitBoxComponent != null)
    {
     hitBoxes.Add(hitBoxComponent);
    }
    else
    {
     Debug.LogError("HitBox component not found on GameObject with tag 'Hitbox'.");
    }
   }
   // Recursively search child objects
   FindHitboxesInHierarchy(child);
  }
 }
 public void initialize()
 {
  InitializeArrowsSpawned(midiScoreBeats.Count);
  InitializeMidiScoreBeats(midiScoreBeats);
 }
 void Update()
 {
  GameManager.Instance.checkGameStart();
  // Debug.Log(GameManager.Instance.checkGameStart());
  CheckArrowSpawn();
  HandlePlayerInput();
 }
 public void InitializeSpawnManager(List<HitBox> hitBoxes, List<float> midiScoreBeats, List<GameObject> spawnedArrows)
 {
  this.hitBoxes = hitBoxes;
  // this.midiScoreBeats = midiScoreBeats;
  initialized = true;
 }
 public void InitializeArrowsSpawned(int count)
 {
  arrowsSpawned = new bool[count];
  for (int i = 0; i < arrowsSpawned.Length; i++)
  {
   arrowsSpawned[i] = false;
  }
  initialized = true;
 }

 public void InitializeMidiScoreBeats(List<float> scoreBeats)
 {
  midiScoreBeats = new List<float>(scoreBeats);
  initialized = true;
 }
 public GameObject SpawnArrow(GameObject arrowPrefab, float arrowSpawnTime, float beatTime, float arrowSpeed)
 {
  // Instantiate the arrow prefab at a random position within a range
  GameObject newArrow = Instantiate(arrowPrefab, GetArrowSpawnPosition(), Quaternion.identity);

  // Assign arrowSpawnTime and beatTime to the arrow
  arrows arrowScript = newArrow.GetComponent<arrows>();
  if (arrowScript != null)
  {
   arrowScript.arrowSpawnTime = arrowSpawnTime;
   arrowScript.beatTime = beatTime;
   arrowScript.speed = arrowSpeed;
  }
  else
  {
   Debug.LogError("Arrows component not found on arrowPrefab.");
  }
  return newArrow;
 }
 void CheckArrowSpawn()
 {
  // if (!gameStarted) // Check if the game has started
  //     return;

  // if (spawningPaused)
  //  return;

    float elapsedTime = GameManager.Instance.CheckElapsedTime();
  float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
  bool arrowSpawnedThisFrame = false; // Flag to track if an arrow has been spawned in this frame

  for (int i = 0; i < midiScoreBeats.Count; i++)
  {
   float beatTime = midiScoreBeats[i];
   float arrowSpawnTime = beatTime - GetTimeToHitbox();

   // Debug.Log((arrowSpawnTime));
   float arrowSpeed = arrowPrefab.GetComponent<arrows>().speed;

   if (i < arrowsSpawned.Length && !arrowSpawnedThisFrame && !arrowsSpawned[i] && elapsedTime >= arrowSpawnTime)
   {
    GameObject newArrow = SpawnArrow(arrowPrefab, arrowSpawnTime, beatTime, arrowSpeed); // Spawn the arrow earlier
    spawnedArrows.Add(newArrow);
    arrowsSpawned[i] = true;
    arrowSpawnedThisFrame = true; // Set the flag to true since an arrow has been spawned in this frame
    break;
   }
  }
 }


public Vector3 GetArrowSpawnPosition()
{
 // Return the spawn position of the arrow (you may need to adjust this based on your scene setup)
 return new Vector3(50f, 100f, -2.5f); // Example position, adjust as needed
}

private float GetTimeToHitbox()
{
 if (arrowPrefab != null && hitBoxes.Count > 0)
 {
  // Calculate the time it takes for an arrow to reach the hitbox based on its speed and hitbox position
  float hitBoxDistance = Vector3.Distance(GetArrowSpawnPosition(), hitBoxes[0].transform.position); // Assuming there's only one hitbox
  arrows arrow = arrowPrefab.GetComponent<arrows>(); // Get the arrows component from the arrowPrefab
  if (arrow != null)
  {
   return hitBoxDistance / arrow.speed;
  }
  else
  {
   Debug.LogError("Arrows component not found on arrowPrefab.");
   return 0f; // Or handle the error in a way appropriate for your application
  }
 }
 else
 {
  Debug.LogError("ArrowPrefab or HitBoxes not assigned.");
  return 0f;
 }
}



public void ProcessArrowHit(arrows arrowScript)
{
 int hitBoxIndex = arrowScript.hitBoxIndex;
 float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
 HitBox hitBox = hitBoxes[hitBoxIndex];
 if (hitBox != null)
 {
  hitBox.ProcessHit(arrowScript.beatTime, currentPosition, hitBoxIndex);
 }
 else
 {
  Debug.LogError("HitBox not found for arrow.");
 }

 // Remove the arrow from spawnedArrows list and destroy it
 spawnedArrows.Remove(arrowScript.gameObject);
 Destroy(arrowScript.gameObject);
}
void HandlePlayerInput()
{
 if (Input.GetKeyDown(KeyCode.Space)) // Check if the game has started
 {
  // Iterate over all spawned arrows
  for (int i = 0; i < spawnedArrows.Count; i++)
  {
   GameObject arrowObject = spawnedArrows[i];

   // Check if the arrow object is valid and active
   if (arrowObject != null && arrowObject.activeSelf)
   {
    arrows arrowScript = arrowObject.GetComponent<arrows>();

    // Check if the arrow script is valid
    if (arrowScript != null)
    {
     int hitBoxIndex = arrowScript.hitBoxIndex;

     // Process the hit with the appropriate timing parameters
     HitBox hitBox = hitBoxes[hitBoxIndex];
     if (hitBox != null)
     {
      hitBox.ProcessHit(arrowScript.beatTime, (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds, hitBoxIndex);
     }
     else
     {
      Debug.LogError("HitBox not found for arrow.");
     }
     // Remove the arrow from spawnedArrows list and destroy it
     spawnedArrows.RemoveAt(i);
     Destroy(arrowObject);
     // Exit the loop to prevent interacting with subsequent arrows
     break;
    }
   }
  }
 }
}

public void levelReset()
{
 spawnedArrows.Clear();
 // arrowsSpawned.Clear();
 arrowsSpawned = new bool[midiScoreBeats.Count]; // You need to initialize the array first
 Array.Clear(arrowsSpawned, 0, arrowsSpawned.Length);
 hitBoxes.Clear();
 midiScoreBeats.Clear();
}
public void stopSong()
{
 // spawningPaused = true;
}
}
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
 public GameObject arrowPrefab; // public so hitbox can access for speed
 [SerializeField] private List<HitBox> hitBoxes = new List<HitBox>(); // Reference to the hitboxes
 [SerializeField] private List<float> midiScoreBeats = new List<float>(); // Reference to the MIDI score beats
 [SerializeField] private bool[] arrowsSpawned;
 public List<GameObject> spawnedArrows = new List<GameObject>(); // Track spawned arrows
 public bool initialized = false;
 public bool spawningPaused = false;
 private float gameStartTime = 0f;
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
  findObjects();
  Debug.Log("spawnmanager start");
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
  if (midiFilePlayer == null)
  {
   Debug.Log("Find Objects");
   midiFilePlayer = FindObjectOfType<MidiFilePlayer>();
   GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
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
     Debug.LogError("HitBox component not found on GameObject with tag 'Hitbox'.");
    }
   }
   //search child objects
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
  if (PlayerPrefs.GetString("sceneType") == "gameplay")
  {
   findObjects();
  }
  GameManager.Instance.checkGameStart();
  CheckArrowSpawn();
  HandlePlayerInput();
 }
 public void InitializeSpawnManager(List<HitBox> hitBoxes, List<float> midiScoreBeats, List<GameObject> spawnedArrows)
 {
  this.hitBoxes = hitBoxes;
 }
 public void InitializeArrowsSpawned(int count)
 {
  arrowsSpawned = new bool[count];
  for (int i = 0; i < arrowsSpawned.Length; i++)
  {
   arrowsSpawned[i] = false;
  }
  initialized = true;
  Debug.Log("arrows spawned initialized");
 }

 public void InitializeMidiScoreBeats(List<float> scoreBeats)
 {

  midiScoreBeats = new List<float>(scoreBeats);
  Debug.Log("midiinitialized");
 }
 public GameObject SpawnArrow(GameObject arrowPrefab, float arrowSpawnTime, float beatTime, float arrowSpeed)
 {
  // instantiate arrow prefab
  GameObject newArrow = Instantiate(arrowPrefab, GetArrowSpawnPosition(), Quaternion.identity);

  // assign arrowSpawnTime and beatTime to the arrow
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
  if (spawningPaused || GameManager.Instance == null)
   return;
  if (!GameManager.Instance.checkGameStart())
   return;

  if (GameManager.Instance.levelPlaying && GameManager.Instance.songStatus() && GameManager.Instance.checkGameStart())
  {
   float elapsedTime = GameManager.Instance.CheckElapsedTime();
   float currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
   bool arrowSpawnedThisFrame = false; // track if an arrow has been spawned in this frame
   for (int i = 0; i < midiScoreBeats.Count; i++)
   {
    float beatTime = midiScoreBeats[i];
    float arrowSpawnTime = beatTime - GetTimeToHitbox();
    float arrowSpeed = arrowPrefab.GetComponent<arrows>().speed;
    if (i < arrowsSpawned.Length && !arrowSpawnedThisFrame && !arrowsSpawned[i] && elapsedTime >= arrowSpawnTime)
    {
     GameObject newArrow = SpawnArrow(arrowPrefab, arrowSpawnTime, beatTime, arrowSpeed); // spawn the arrow earlier
     spawnedArrows.Add(newArrow);
     arrowsSpawned[i] = true;
     arrowSpawnedThisFrame = true;
     break;
    }
   }
  }
 }

 public Vector3 GetArrowSpawnPosition()
 {
  return new Vector3(100f, 50f, -2.5f);
 }

 private float GetTimeToHitbox()
 {
  if (arrowPrefab != null && hitBoxes.Count > 0)
  {
   // time it takes for arrow to reach  hitbox based on speed and hitbox position
   float hitBoxDistance = Vector3.Distance(GetArrowSpawnPosition(), hitBoxes[0].transform.position);
   arrows arrow = arrowPrefab.GetComponent<arrows>(); // get the arrows component from the arrowPrefab
   if (arrow != null)
   {
    return hitBoxDistance / arrow.speed;
   }
   else
   {
    Debug.LogError("Arrows component not found on arrowPrefab.");
    return 0f;
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
  // remove arrow from spawnedArrows list and destroy
  spawnedArrows.Remove(arrowScript.gameObject);
  Destroy(arrowScript.gameObject);
 }

 void HandlePlayerInput()
 {
  if (spawningPaused || GameManager.Instance == null)
   return;
  if (!GameManager.Instance.levelPlaying)
   return;

  if (Input.GetKeyDown(KeyCode.Space))
  {
   for (int i = 0; i < spawnedArrows.Count; i++)
   {
    GameObject arrowObject = spawnedArrows[i];
    // check if arrow object active
    if (arrowObject != null && arrowObject.activeSelf)
    {
     arrows arrowScript = arrowObject.GetComponent<arrows>();
     if (arrowScript != null)
     {
      int hitBoxIndex = arrowScript.hitBoxIndex;
      HitBox hitBox = hitBoxes[hitBoxIndex];
      if (hitBox != null)
      {
       hitBox.ProcessHit(arrowScript.beatTime, (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds, hitBoxIndex);
      }
      else
      {
       Debug.LogError("HitBox not found for arrow.");
      }
      // remove arrow from list then distry
      spawnedArrows.RemoveAt(i);
      Destroy(arrowObject);
      //exit loop
      break;
     }
    }
   }
  }
 }
 public void pauseArrows()
 {
  foreach (GameObject arrowObject in spawnedArrows)
  {
   if (arrowObject != null)
   {
    Rigidbody2D rb = arrowObject.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
     rb.velocity = Vector2.zero;
    }
   }
  }
 }
 public void levelReset()
 {
  spawnedArrows.Clear();
  Array.Clear(arrowsSpawned, 0, arrowsSpawned.Length);
  // Debug.Log(arrowsSpawned);
  hitBoxes.Clear();
  midiScoreBeats.Clear();
 }
 public void destroyArrows()
 {
  foreach (GameObject arrowObject in spawnedArrows)
  {
   if (arrowObject != null)
   {
    Destroy(arrowObject);
   }
  }
 }
 public void stopSpawn()
 {
  pauseArrows();
  spawningPaused = true;
 }
 public void playSpawn()
 {
  spawningPaused = false;
 }
}
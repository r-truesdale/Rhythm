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
    return -1; // Invalid hitbox
  }
 }
 public void Start()
 {

  GameObject arrowPrefab = SpawnManager.Instance.arrowPrefab;
  arrows arrow = arrowPrefab.GetComponent<arrows>();
  if (arrow == null)
  {
   return;
  } // hitboxes change size to match change in arrow speed
  float arrowSpeed = arrow.speed;
  float perfectHitboxSize = arrowSpeed * perfectTimingWindow;
  float sizeDifference = perfectHitboxSize - this.perfectHitboxSize;
  float otherSize = perfectHitboxSize * otherHitboxMultiplier;
  float otherHitboxOffset = sizeDifference * 0.5f;
  // Debug.Log("sethitboxsize" + otherSize);
  this.perfectHitboxSize = perfectHitboxSize;
  transform.localScale = new Vector3(perfectHitboxSize, transform.localScale.y, transform.localScale.z);
  Vector3 perfectPosition = transform.position;
  Vector3 tooEarlyPosition = perfectPosition + transform.right * (0.5f * (transform.localScale.x + otherSize));
  Vector3 tooLatePosition = perfectPosition - transform.right * (0.5f * (transform.localScale.x + otherSize));
  Vector3 earlyMissPosition = tooEarlyPosition + transform.right * otherSize;
  Vector3 lateMissPosition = tooLatePosition - transform.right * otherSize;

  float earlyMissXOffset = 0.5f * (transform.localScale.x + otherSize);
  float lateMissXOffset = 0.5f * (transform.localScale.x + otherSize);

  Vector3 earlyMissOffset = transform.right * (earlyMissXOffset*2f);
  Vector3 lateMissOffset = transform.right * lateMissXOffset;

  Vector3 earlyMissAdjustedPosition = earlyMissPosition + earlyMissOffset*2f;
  Vector3 lateMissAdjustedPosition = lateMissPosition - lateMissOffset;
float screenWidth = Camera.main.aspect * Camera.main.orthographicSize * 2f;
  HitBox tooEarlyHitbox = FindHitBox("TooEarly");
  HitBox tooLateHitbox = FindHitBox("TooLate");
  HitBox earlyMissHitbox = FindHitBox("EarlyMiss");
  HitBox lateMissHitbox = FindHitBox("LateMiss");
  HitBox perfectHitbox = FindHitBox("Perfect");
  if (tooEarlyHitbox != null && tooLateHitbox != null)
  {
   perfectHitbox.transform.position = perfectPosition;
   tooEarlyHitbox.transform.position = tooEarlyPosition;
   tooLateHitbox.transform.position = tooLatePosition;
   earlyMissHitbox.transform.position = earlyMissAdjustedPosition;
   lateMissHitbox.transform.position = lateMissAdjustedPosition;
   float earlyMissScaleX = Mathf.Abs(earlyMissAdjustedPosition.x - tooEarlyPosition.x - (earlyMissXOffset/2)) * 2f;
   float lateMissScaleX = Mathf.Abs(tooLatePosition.x - lateMissAdjustedPosition.x - (lateMissXOffset/2)) * 2f;

   earlyMissHitbox.transform.localScale = new Vector3(earlyMissScaleX, earlyMissHitbox.transform.localScale.y, earlyMissHitbox.transform.localScale.z);
   lateMissHitbox.transform.localScale = new Vector3(lateMissScaleX, lateMissHitbox.transform.localScale.y, lateMissHitbox.transform.localScale.z);
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
public class HitBox : MonoBehaviour
{
 public string hitBoxName;
 private float perfectHitboxSize = 1.0f;
 private float otherHitboxMultiplier = 1.5f;
 private arrows arrow;
 private float perfectTimingWindow = 0.2f;
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
    // Debug.Log(currentPlaybackTime);
    // Calculate accuracy and score

    int hitBoxIndex = GetHitBoxIndex();
    // AccuracyManager.Instance.CalculateAccuracyAndScore(arrow.beatTime, currentPlaybackTime, hitBoxIndex);
    arrow.SetHitBoxIndex(hitBoxIndex);
    // Set the hit box index in the arrows script
   }
   else
   {
    Debug.LogError("Arrows component not found on the object triggering the collider.");
   }
  }
 }

 void Start()
 {
  // hitboxTimings();
  SetHitboxSize();
 }
 public int GetHitBoxIndex()
 {
  // Determine the hitbox index based on its name or position in the array
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
 public void SetHitboxSize()
 {
  arrow = GetComponentInParent<arrows>();
  if (arrow == null)
  {
   return;
  }
  float arrowSpeed = arrow.speed;
  float perfectHitboxSize = arrowSpeed * perfectTimingWindow;
  float otherSize = perfectHitboxSize * otherHitboxMultiplier;
  float otherHitboxOffset = (otherSize - transform.localScale.y);

  transform.localScale = new Vector3(transform.localScale.x, perfectHitboxSize, transform.localScale.z);
  hitBoxScale("TooEarly", otherSize, otherHitboxOffset);
  hitBoxScale("EarlyMiss", otherSize, otherHitboxOffset * otherHitboxMultiplier);
  hitBoxScale("TooLate", otherSize, -otherHitboxOffset);
 }
 private void hitBoxScale(string hitBoxName, float otherSize, float yOffset)
 {
  HitBox hitBox = FindHitBox(hitBoxName);
  if (hitBox != null)
  {
   hitBox.transform.localPosition = new Vector3(hitBox.transform.localPosition.x, yOffset, hitBox.transform.localPosition.z);
   hitBox.transform.localScale = new Vector3(hitBox.transform.localScale.x, otherSize, hitBox.transform.localScale.z);
  }
  else
  {
   Debug.LogError(hitBoxName + " hitbox not found.");
  }
 }

 private HitBox FindHitBox(string hitBoxName)
 {
  // Find the hitbox with the specified name within the same parent
  foreach (Transform child in transform.parent)
  {
   if (child.CompareTag("HitBox") && child.GetComponent<HitBox>().hitBoxName == hitBoxName)
   {
    return child.GetComponent<HitBox>();
   }
  }
  Debug.Log("null");
  return null; // Return null if the hitbox is not found
 }
 public bool ProcessHit(float beatTime, float currentPlaybackTime, int hitBoxIndex)
 {
  // call accuracyManager to calculate accuracy and score
  AccuracyManager.Instance.CalculateAccuracyAndScore(beatTime, currentPlaybackTime, hitBoxIndex);
  return true;
 }
}
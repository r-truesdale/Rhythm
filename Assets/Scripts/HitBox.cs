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
 [SerializeField] private MidiFilePlayer midiFilePlayer;
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
    // float currentPlaybackTime = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
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
  float arrowSpeed = arrow.GetComponent<arrows>().speed;
  float perfectHitboxSize = arrowSpeed * perfectTimingWindow;
  // Calculate the size of the other hitboxes
  float otherSize = perfectHitboxSize * otherHitboxMultiplier;
  // Calculate the offset for positioning the other hitboxes relative to the perfect hitbox
  float otherHitboxOffset = (otherSize - transform.localScale.y);

  // Set the scale of the perfect hitbox
  transform.localScale = new Vector3(transform.localScale.x, perfectHitboxSize, transform.localScale.z);

  // Set the scale and position of the early hitbox
  HitBox earlyHitbox = FindHitBox("TooEarly");
  if (earlyHitbox != null)
  {
   earlyHitbox.transform.localPosition = new Vector3(earlyHitbox.transform.localPosition.x, otherHitboxOffset, earlyHitbox.transform.localPosition.z);
   earlyHitbox.transform.localScale = new Vector3(earlyHitbox.transform.localScale.x, otherSize, earlyHitbox.transform.localScale.z);
  }

  // Set the scale and position of the early miss hitbox
  HitBox earlyMissHitbox = FindHitBox("EarlyMiss");
  if (earlyMissHitbox != null)
  {
   earlyMissHitbox.transform.localPosition = new Vector3(earlyMissHitbox.transform.localPosition.x, otherHitboxOffset * otherHitboxMultiplier, earlyMissHitbox.transform.localPosition.z);
  }
  else
  {
   Debug.LogError("earlyMiss null");
  }

  // Set the scale and position of the late hitbox
  HitBox lateHitbox = FindHitBox("TooLate");
  if (lateHitbox != null)
  {
   lateHitbox.transform.localPosition = new Vector3(lateHitbox.transform.localPosition.x, -otherHitboxOffset, lateHitbox.transform.localPosition.z);
   lateHitbox.transform.localScale = new Vector3(lateHitbox.transform.localScale.x, otherSize, lateHitbox.transform.localScale.z);
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
 // public void hitboxTimings()
 // {
 //  // if (arrow != null)
 //  // {
 //   // float arrowSpeed = arrow.speed;

 //   // SetHitboxSize(perfectHitboxSize);
 //  // }
 //  // else
 //  // {
 //  //  Debug.LogError("Arrows component not found on the HitBox GameObject.");
 //  // }
 // }
 public bool ProcessHit(float beatTime, float currentPlaybackTime, int hitBoxIndex)
 {
  // Call the AccuracyManager to calculate accuracy and score
  AccuracyManager.Instance.CalculateAccuracyAndScore(beatTime, currentPlaybackTime, hitBoxIndex);
  return true;
 }
}
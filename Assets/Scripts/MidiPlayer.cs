using System.Collections;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;

public class MidiPlayer : MonoBehaviour
{
 public static MidiPlayer Instance { get; private set; }
 public MidiFilePlayer midiFilePlayer;
 public float currentPosition;
 void Start()
 {

 }

 // Update is called once per frame
 void Update()
 {
  currentPosition = (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
 }

 public void Play()
 {
  midiFilePlayer.MPTK_Play();
 }

 public float GetPlaybackTime()
 {
  if (midiFilePlayer != null)
  {
   return (float)midiFilePlayer.MPTK_PlayTime.TotalSeconds;
  }
  else
  {
   Debug.LogError("MidiFilePlayer is not assigned in GameManager.");
   return 0f;
  }
 }
}

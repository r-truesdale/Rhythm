using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
public class BeatObjects : MonoBehaviour
{
 public float speed = 1.0f;
 public float BeatObjectspawnTime;
 public float beatTime;
 public int hitBoxIndex;
 void Update()
 {//if the game isn't pause, move the beat to the left
    if (!GameManager.Instance.gamePaused)
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
 }
 public void SetHitBoxIndex(int index)
 {
  hitBoxIndex = index;
  if (hitBoxIndex == 4)
  { //if beat gets to the latemiss hitbox, destroy and count as latemiss score
  Destroy(gameObject);
  AccuracyManager.Instance.tooLateMiss();
  }
 }
}
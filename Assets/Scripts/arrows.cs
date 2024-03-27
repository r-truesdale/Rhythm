using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
public class arrows : MonoBehaviour
{
    public float speed = 1.0f;
    public float arrowSpawnTime;
    public float beatTime;
    public int hitBoxIndex;
    void Update()
    {
        // Move the arrow continuously
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    public void SetHitBoxIndex(int index)
    {
        hitBoxIndex = index;
        if (hitBoxIndex == 4)
        { //if it gets to the latemiss hitbox, destroy
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        // If the arrow becomes invisible, destroy it
        Destroy(gameObject);
    }
}
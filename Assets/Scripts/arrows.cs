using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
public class arrows : MonoBehaviour
{
    public float speed = 1.0f;
    public float arrowSpawnTime;
    public float beatTime;
    private int hitBoxIndex;
    void Update()
    {
        // Move the arrow continuously
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        // If the arrow becomes invisible, destroy it
        Destroy(gameObject);
    }
}
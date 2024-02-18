using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrows : MonoBehaviour
{
    // Start is called before the first frame update    public float speed = 1.0f;
    public float speed = 1.0f;
    private void Update()
    {
        // Move the arrow continuously
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    private void OnBecameInvisible(){
        Destroy(gameObject);
    }
}

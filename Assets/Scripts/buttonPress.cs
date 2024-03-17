// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class buttonPress : MonoBehaviour
// {
//     public float timeWindow = 0.1f;
//     public ScoreSystem scoreSystem;
//     private List<float> correctTime = new List<float>();
//     private bool buttonPressed = false;
//     // Variable to keep track of the time the button was pressed
//     private float pressTime;
//     // Start is called before the first frame update
//     void Start()
//     {
//         LoadcorrectTime();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Check for button press
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             if (!buttonPressed)
//             {
//                 buttonPressed = true;
//                 pressTime = Time.time;
//                 // Check if the timing is correct
//                 foreach (float timing in correctTime)
//                 {
//                     if (Mathf.Abs(pressTime - timing) <= timeWindow)
//                     {
//                         // Correct timing, give a point
//                         scoreSystem.AddPoint();
//                         break;
//                     }
//                 }
//             }
//         }
//         else if (Input.GetKeyUp(KeyCode.Space)) // Change KeyCode.Space to your desired button
//         {
//             // Reset the button state when the key is released
//             buttonPressed = false;
//         }
//     }

//     // Method to load correct timings from JSON file
//     private void LoadcorrectTime()
//     {
//         TextAsset jsonFile = Resources.Load<TextAsset>("correctTime");
//         if (jsonFile != null)
//         {
//             correctTimeData data = JsonUtility.FromJson<correctTimeData>(jsonFile.text);
//             correctTime = data.correctTime;
//         }
//         else
//         {
//             Debug.LogError("Failed to load JSON file.");
//         }
//     }
// }

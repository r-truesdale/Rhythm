// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// [InitializeOnLoad]
// public class mouseposition : MonoBehaviour
// {
//     static mouseposition()
//     {
//         SceneView.duringSceneGui += OnSceneGUI;
//     }

//     static void OnSceneGUI(SceneView sceneView)
//     {
//         // Get the position of the mouse cursor in screen coordinates
//         Vector2 mousePosition = Event.current.mousePosition;

//         // Convert the screen coordinates to world coordinates
//         Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
//         Vector3 worldPosition = ray.origin + ray.direction * (Camera.current.transform.position.y / -ray.direction.y);

//         // Display the coordinates in the scene view
//         Handles.Label(worldPosition, "Mouse Position: " + worldPosition);
//     }
// }

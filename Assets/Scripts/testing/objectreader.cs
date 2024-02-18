using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectreader : MonoBehaviour
{
    void Start()
    {
        // Debug.Log("Object is being read: " + gameObject.name);
        string jsonFilePath = Path.Combine(Application.dataPath + "/scripts", "jsontest.json");
        Debug.Log("File Path: " + jsonFilePath);

        if (File.Exists(jsonFilePath))
        {
            //fetching the data from the json file
            string jsonContent = File.ReadAllText(jsonFilePath);
            JObject songsObject = JObject.Parse(jsonContent);
            JArray songsArray = (JArray)songsObject["songs"];

            // Debug.Log("JSON Content: " + jsonContent);
            // Debug.Log((JObject)songsArray[1]);
            for (int i = 0; i < songsArray.Count; i++)
            {
                // Debug.Log((string)songsArray[i]["name"]);
                string name = (string)songsArray[i]["name"];
            }
            // Rest of your code to parse JSON and fetch the title
        }
        else
        {
            Debug.LogError("File not found: " + jsonFilePath);
        }

    }
    public void selectSong()
    {

    }
}

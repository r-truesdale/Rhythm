using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    void start()
    {
        Debug.Log("testing 1 2 3");
        string jsonFilePath = Path.Combine(Application.dataPath, "jsontest.json");
        Debug.Log("File Path: " + jsonFilePath);

        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            Debug.Log("JSON Content: " + jsonContent);

            // Rest of your code to parse JSON and fetch the title
        }
        else
        {
            Debug.LogError("File not found: " + jsonFilePath);
        }
    }

}


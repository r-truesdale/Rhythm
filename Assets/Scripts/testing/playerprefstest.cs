using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerprefstest : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text text;
    List<int> myList;
    public string PlayerPrefString;
    public List<string> ExportList;

    void Start()
    {
        test();
        
    }

    void test()
    {
        PlayerPrefs.SetString("examplestr", "Something[EndInp123]SomethingElse[EndInp123]Good[EndInp123]");
        PlayerPrefString = PlayerPrefs.GetString("examplestr");
        StringToList(PlayerPrefString, "[EndInp123]");
        foreach (string str in ExportList)
        {
            print(str);
        }
    }
    void StringToList(string message, string seperator)
    {
        ExportList.Clear();
        string tok = "";
        foreach(char character in message)
        {
            tok = tok + character;
            if (tok.Contains(seperator))
            {
                tok = tok.Replace(seperator, "");
                ExportList.Add(tok);
                tok = "";
            }
        }
    }
}

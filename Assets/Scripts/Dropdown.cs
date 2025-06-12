using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown : MonoBehaviour
//controls the beat types selected by the player before the level starts 
{// reference for each UI button element for inspector and 
    [SerializeField] private Button Btn1;
    [SerializeField] private Button Btn2;
    [SerializeField] private Button Btn3;
    [SerializeField] private Button selectedBtn;
    void Start()
    {
        Btn1.onClick.AddListener(() => OptionSelected(Btn1));
        Btn2.onClick.AddListener(() => OptionSelected(Btn2));
        Btn3.onClick.AddListener(() => OptionSelected(Btn3));
        StartCoroutine(GameManagerInitialization());
    }
    IEnumerator GameManagerInitialization()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }
        // load the saved beat type option or set default
        int savedBeatType = PlayerPrefs.GetInt("beatType", 0); // Default to 0
    }
    void OptionSelected(Button selected)
    {
        int selectedOption = 0;
        if (selected == Btn1)
        {
            selectedOption = 0;
        }
        else if (selected == Btn2)
        {
            selectedOption = 1;
        }
        else if (selected == Btn3)
        {
            selectedOption = 2;
        }
        PlayerPrefs.SetInt("beatType", selectedOption);
        PlayerPrefs.Save();
        GameManager.Instance.UpdateBeatOptions();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownMenu : MonoBehaviour
{
 public Button Btn1;
 public Button Btn2;
 public Button Btn3;
 private Button selectedBtn;
 private string beatTypeKey = "beatType";

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
  int savedBeatType = PlayerPrefs.GetInt(beatTypeKey, 0); // Default to 0
  Debug.Log(savedBeatType);
  buttonHighlight(savedBeatType);
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
  PlayerPrefs.SetInt(beatTypeKey, selectedOption);
  PlayerPrefs.Save();
  GameManager.Instance.UpdateBeatOptions();
  buttonHighlight(selectedOption);
 }
 void buttonHighlight(int selectedOption)
 {
  switch (selectedOption)
  {
   case 0:
    selectedBtn = Btn1;
    break;
   case 1:
    selectedBtn = Btn2;
    break;
   case 2:
    selectedBtn = Btn3;
    break;
  }
 }
}
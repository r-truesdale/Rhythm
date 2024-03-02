using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownMenu : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    // PlayerPrefs key for storing the selected beat type
    private string beatTypeKey = "beatType";

    void Start()
    {
        // Add listener for value changes in the dropdown
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });

        // Wait for GameManager to be initialized before setting the default value
        StartCoroutine(WaitForGameManagerInitialization());
    }

    IEnumerator WaitForGameManagerInitialization()
    {
        // Wait until GameManager.Instance is not null
        while (GameManager.Instance == null)
        {
            yield return null;
        }

        // Load the saved beat type option or set default
        int savedBeatType = PlayerPrefs.GetInt(beatTypeKey, 0); // Default to 0
        dropdown.value = savedBeatType;
    }

    // Method called when dropdown value changes
    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Save the selected value to PlayerPrefs
        PlayerPrefs.SetInt(beatTypeKey, dropdown.value);
        // Force PlayerPrefs to be saved immediately
        PlayerPrefs.Save();

        // Notify GameManager or other relevant scripts about the change
        GameManager.Instance.UpdateBeatOptions();
    }
}
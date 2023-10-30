using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    public AudioManager audioManager;

    public bool FullScreenValue;
    public Toggle FullScreenToggle;

    public int ResolutionValue;
    public TMP_Dropdown ResolutionDropdown;

    private void Start()
    {
        FullScreenToggle.isOn = PlayerPrefs.GetInt("IsFullScreen", 1) == 1;
        ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionChoice", 0);
    }

    public void ToggleFullscreen(bool toggle)
    {
        FullScreenValue = toggle;
        Screen.fullScreen = FullScreenValue;
        PlayerPrefs.SetInt("IsFullScreen", FullScreenValue ? 1 : 0);
    }

    public void ChangeResolution(int choice)
    {
        ResolutionValue = choice;

        switch (ResolutionValue)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                Debug.Log("1920x1080");
                break;

            case 1:
                Screen.SetResolution(1366, 768, Screen.fullScreen);
                Debug.Log("1366x768");
                break;

            case 2:
                Screen.SetResolution(1440, 900, Screen.fullScreen);
                Debug.Log("1440x900");
                break;

            case 3:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                Debug.Log("1600x900");
                break;
        }

        PlayerPrefs.SetInt("ResolutionChoice", ResolutionValue);
    }
}

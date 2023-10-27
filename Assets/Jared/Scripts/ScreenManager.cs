using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    public Toggle FullScreenToggle;
    public TMP_Dropdown ResolutionDropdown;

    private void Start()
    {
        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        int resolutionChoice = PlayerPrefs.GetInt("ResolutionChoice", 0);

        Screen.fullScreen = isFullscreen;
        ChangeResolution(resolutionChoice);
        FullScreenToggle.isOn = isFullscreen;
        ResolutionDropdown.value = resolutionChoice;

        Debug.Log("Full screen: " + isFullscreen + " - Resolution: " + resolutionChoice);
    }

    public void ToggleFullscreen(bool toggle)
    {
        Screen.fullScreen = toggle;

        PlayerPrefs.SetInt("IsFullscreen", toggle ? 1 : 0);
        PlayerPrefs.Save();

        if (toggle)
            Debug.Log("Fullscreen on");
        else
            Debug.Log("Fullscreen off");
    }

    public void ChangeResolution(int choice)
    {
        int ScreenWidth = 0;
        int ScreenHeight = 0;

        switch (choice)
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

        Screen.SetResolution(ScreenWidth, ScreenHeight, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionChoice", choice);
        PlayerPrefs.Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionTest : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    bool isFullScreen;
    int resolutionType;

    public void ChangeResolution(int resType) {
        resolutionType = resType;
        switch(resType) {
            case 0:
                Screen.SetResolution(1920, 1080, isFullScreen);
                break;
            case 1: 
                Screen.SetResolution(1280, 720, isFullScreen);
                break;
            case 2:
                Screen.SetResolution(800, 600, isFullScreen);
                break;
        }
        print($"input type : {resType}");
    }

    public void ChangeFullScreen(bool isFull) {
        isFullScreen = isFull;
        Screen.fullScreen = isFull;
        print($"fullscreen : {isFull}");
    }
    // Start is called before the first frame update
    void Start()
    {
        isFullScreen = Screen.fullScreen;
        switch(Screen.width) {
            case 1920:
                resolutionType = 0;
                break;            
            case 1280:
                resolutionType = 1;
                break;
            case 800:
                resolutionType = 2;
                break;
            default :
                resolutionType = 0;
                break;
                
        }

        fullscreenToggle.SetIsOnWithoutNotify(isFullScreen);
        resolutionDropdown.SetValueWithoutNotify(resolutionType);
    }
}

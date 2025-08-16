using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SeetingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] Slider VolumeSlider;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public void Start()
    {

        QualitySettings.SetQualityLevel(2);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {   
            
                string option = resolutions[i] + "X" + resolutions[i].height;
                options.Add(option);
            
            
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume()
    {
        AudioListener.volume = VolumeSlider.value;
    }

 

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(Mathf.Abs(2-qualityIndex));
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


}
   
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// singleton responsible for managing settings
// when a setting change occurs, it passes through this class
// and raises events to notify listeners (such as AudioManager)
// also handles save/revert requests for settings and marks changed settings as "dirty"
// the settings themselves are located in SETTINGS
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    public Dictionary<string, bool> isSettingAltered;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void Start()
    {
        isSettingAltered = new Dictionary<string, bool>
        {
            { "BGM", false },
            { "SFX", false }
        };
    }


    public void HandleBGMLevelAltered(float newVolume)
    {
        SETTINGS.CurrentBGMVolume = newVolume;

        if (SETTINGS.CurrentBGMVolume == SETTINGS.PreviousVolumeSettings.BGMVolume)
        {
            isSettingAltered["BGM"] = false;
        }
        else
        {
            isSettingAltered["BGM"] = true;
        }

        SettingsEvents.RaiseSettingsAltered();

    }

    public void HandleSFXLevelAltered(float newVolume)
    {
        SETTINGS.CurrentSFXVolume = newVolume;

        if (SETTINGS.CurrentSFXVolume == SETTINGS.PreviousVolumeSettings.SFXVolume)
        {
            isSettingAltered["SFX"] = false;
        }
        else
        {
            isSettingAltered["SFX"] = true;
        }


        SettingsEvents.RaiseSettingsAltered();

    }

    public void HandleSettingsSaved()
    {
        Debug.Log("Saving settings changes");
        SETTINGS.SaveSettings();
        SettingsEvents.RaiseSettingsApplied();
        ResetAlteredState();
    }

    public void HandleSettingsReverted()
    {
        Debug.Log("Settings changes reverted.");
        SETTINGS.RevertSettings();
        SettingsEvents.RaiseSettingsReverted();
        ResetAlteredState();
    }

    public bool CheckIfAltered()
    {
        foreach (bool altered in isSettingAltered.Values)
        {
            if (altered)
                return true;
        }

        return false;
    }

    void ResetAlteredState()
    {
        foreach (var key in isSettingAltered.Keys.ToList())
        {
            isSettingAltered[key] = false;
        }
    }
}

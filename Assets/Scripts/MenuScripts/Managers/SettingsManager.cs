using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// Singleton responsible for managing settings
// when a setting change occurs, it passes through this class
// and raises events to notify listeners (such as AudioManager)
// also handles save/revert requests for settings and marks changed settings as "dirty"
// the settings themselves are located in SETTINGS
public class SettingsManager : MonoBehaviour
{
    
    public static Action OnSFXLevelAltered;
    public static Action OnWeaponSFXLevelAltered;
    public static Action OnBGMLevelAltered;
    public static Action OnChangesApplied;
    public static Action OnChangesReverted;

    Dictionary<string, bool> _isSettingDirty;

    public static SettingsManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _isSettingDirty = new Dictionary<string, bool>
        {
            { "BGM", false },
            { "SFX", false },
            {"WeaponSFX", false}
        };
    }


    public void HandleBGMLevelAltered(float newVolume)
    {
        SETTINGS.CurrentBGMVolume = newVolume;

        if (SETTINGS.CurrentBGMVolume == SETTINGS.PreviousVolumeSettings.BGMVolume)
        {
            _isSettingDirty["BGM"] = false;
        }
        else
        {
            _isSettingDirty["BGM"] = true;
        }


        OnBGMLevelAltered?.Invoke();
    }

    public void HandleSFXLevelAltered(float newVolume)
    {
        SETTINGS.CurrentSFXVolume = newVolume;

        if (SETTINGS.CurrentSFXVolume == SETTINGS.PreviousVolumeSettings.SFXVolume)
        {
            _isSettingDirty["SFX"] = false;
        }
        else
        {
            _isSettingDirty["SFX"] = true;
        }

        OnSFXLevelAltered?.Invoke();

    }

    public void HandleWeaponSFXLevelAltered(float newVolume)
    {
        SETTINGS.CurrentWeaponSFXVolume = newVolume;

        if (SETTINGS.CurrentWeaponSFXVolume == SETTINGS.PreviousVolumeSettings.WeaponSFXVolume)
        {
            _isSettingDirty["WeapoNSFX"] = false;
        }
        else
        {
            _isSettingDirty["WeapoNSFX"] = true;
        }

        OnWeaponSFXLevelAltered?.Invoke();

    }

    public void HandleSettingsSaved()
    {
        Debug.Log("Saving settings changes");
        SETTINGS.SaveSettings();
        ResetDirtyDictionary();
    }

    public void HandleSettingsReverted()
    {
        Debug.Log("Settings changes reverted.");
        SETTINGS.RevertSettings();
        OnChangesReverted?.Invoke();
        ResetDirtyDictionary();
    }

    public bool CheckIfDirty()
    {
        foreach (bool altered in _isSettingDirty.Values)
        {
            if (altered)
                return true;
        }

        return false;
    }

    void ResetDirtyDictionary()
    {
        foreach (var key in _isSettingDirty.Keys.ToList())
        {
            _isSettingDirty[key] = false;
        }
    }
}

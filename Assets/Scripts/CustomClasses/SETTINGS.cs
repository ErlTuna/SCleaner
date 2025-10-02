using UnityEngine;

public static class SETTINGS
{

    public const float MIN_VOLUME = 0f;
    public const float MAX_VOLUME = 1f;
    private static readonly string s_SFXVolume = "SFXVolume";
    private static readonly string s_BGMVolume = "BGMVolume";
    private static readonly string s_AutoSwitchWhenNoAmmo = "AutoSwitchWhenNoAmmoLeft";
    private static readonly string s_AutoSwitchToPickedUpWeapon = "AutoSwitchToPickedUpWeapon";
    public static bool AutoSwitchWhenNoAmmo
    {
        get
        {
            if (PlayerPrefs.HasKey(s_AutoSwitchWhenNoAmmo))
            {
                return PlayerPrefsExtensions.GetBool(s_AutoSwitchWhenNoAmmo);
            }
            else
            {
                if (_defaultSettings != null)
                {
                    return _defaultSettings.AutoSwitchWhenNoAmmo;
                }
                else
                {
                    Debug.Log("Default settings SO is missing!");
                    return false;
                }
            }
        }
        set { PlayerPrefsExtensions.SetBool(s_AutoSwitchWhenNoAmmo, value); }
    }
    public static bool AutoSwitchToPickedUpWeapon
    {
        get
        {
            if (PlayerPrefs.HasKey(s_AutoSwitchToPickedUpWeapon))
            {
                return PlayerPrefsExtensions.GetBool(s_AutoSwitchToPickedUpWeapon);
            }
            else
            {
                if (_defaultSettings != null)
                {
                    return _defaultSettings.AutoSwitchToPickedUpWeapon;
                }
                else
                {
                    Debug.Log("Default settings SO is missing!");
                    return false;
                }
            }
        }
        set { PlayerPrefsExtensions.SetBool(s_AutoSwitchToPickedUpWeapon, value); }
    }
    public static float CurrentSFXVolume
    {
        get
        {
            if (PlayerPrefs.HasKey(s_SFXVolume))
            {
                return PlayerPrefs.GetFloat(s_SFXVolume);
            }
            else
            {
                if (_defaultSettings != null)
                {
                    return _defaultSettings.SFX_volume;
                }
                else
                {
                    Debug.Log("Default settings SO is missing!");
                    return .5f;
                }
            }
        }
        set { PlayerPrefs.SetFloat(s_SFXVolume, value); }
    }
    public static float CurrentBGMVolume
    {
        get
        {
            if (PlayerPrefs.HasKey(s_BGMVolume))
            {
                return PlayerPrefs.GetFloat(s_BGMVolume);
            }
            else
            {
                if (_defaultSettings != null)
                {
                    return _defaultSettings.BGM_volume;
                }
                else
                {
                    Debug.Log("Default settings SO is missing!");
                    return .5f;
                }
            }
        }
        set
        {
            PlayerPrefs.SetFloat(s_BGMVolume, value);
        }
    }
    private static MainMenuDefaultsSO _defaultSettings;
    public static VolumeSettings CurrentVolumeSettings { get; private set; } = new(CurrentSFXVolume, CurrentBGMVolume);
    public static VolumeSettings PreviousVolumeSettings { get; private set; } = new();

    public static void Initialize(MainMenuDefaultsSO defaultSettingsSO)
    {
        Debug.Log("Initialized with these settings : " + defaultSettingsSO);
        _defaultSettings = defaultSettingsSO;
        PreviousVolumeSettings = CurrentVolumeSettings;
    }

    public static void SaveSettings()
    {
        CurrentVolumeSettings = new(CurrentSFXVolume, CurrentBGMVolume);
        PreviousVolumeSettings = CurrentVolumeSettings;
        PlayerPrefs.Save();
    }

    public static void RevertSettings()
    {
        CurrentVolumeSettings = PreviousVolumeSettings;

        PlayerPrefs.SetFloat(s_SFXVolume, CurrentVolumeSettings.SFXVolume);
        PlayerPrefs.SetFloat(s_BGMVolume, CurrentVolumeSettings.BGMVolume);

        PlayerPrefs.Save();
    }

    /*public static void LoadPreviousSettings()
    {
        PreviousVolumeSettings = new VolumeSettings(CurrentSFXVolume, CurrentBGMVolume);
    }

    public static bool HasChanged()
    {
        var current = new VolumeSettings(CurrentSFXVolume, CurrentBGMVolume);
        return !PreviousSettings.Equals(current);
    }*/
}

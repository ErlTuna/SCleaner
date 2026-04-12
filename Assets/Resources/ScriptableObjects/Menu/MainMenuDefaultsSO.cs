using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainMenuDefaultsSO", menuName = "ScriptableObjects/Misc/Main Menu Settings")]
public class MainMenuDefaultsSO : ScriptableObject
{
    [Range(0, 1)] public float SFX_volume = 0.5f;
    [Range(0, 1)] public float WeaponSFX_volume = 0.5f;
    [Range(0, 1)] public float BGM_volume = 0.2f;
    public bool AutoSwitchWhenNoAmmo = false;
    public bool AutoSwitchToPickedUpWeapon = false;
    public int minVolume = 0;
    public int maxVolume = 1;
}

using UnityEngine;
public struct VolumeSettings
{
    public float SFXVolume;
    public float BGMVolume;

    public VolumeSettings(float sfx, float bgm)
    {
        SFXVolume = sfx;
        BGMVolume = bgm;
    }

    public bool Equals(VolumeSettings other, float tolerance = 0.001f)
    {
        return Mathf.Abs(SFXVolume - other.SFXVolume) < tolerance &&
               Mathf.Abs(BGMVolume - other.BGMVolume) < tolerance;
    }
}

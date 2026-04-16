using System;

[Serializable]
public class WeaponClipSizeModifier : IWeaponModifier
{
    public WeaponType? TypeFilter;
    public int Amount;

    public void RequestApply(WeaponRuntime weapon)
    {
        throw new NotImplementedException();
    }

    public void RequestRemove(WeaponRuntime weapon)
    {
        throw new NotImplementedException();
    }
}

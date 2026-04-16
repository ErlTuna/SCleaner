using System;

[Serializable]
public class WeaponClipSizeModifier : IWeaponModifier
{
    public WeaponType? TypeFilter;
    public int Amount;

    public void RequestApply(WeaponRuntimeData weapon)
    {
        throw new NotImplementedException();
    }

    public void RequestRemove(WeaponRuntimeData weapon)
    {
        throw new NotImplementedException();
    }
}

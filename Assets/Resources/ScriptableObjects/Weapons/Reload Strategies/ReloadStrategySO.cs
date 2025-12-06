using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ReloadStrategySO : ScriptableObject
{
    public bool CanBeReloadCanceled = false;

    public abstract void ReloadStart(BaseWeapon owner);
    public abstract void PerformReload(BaseWeapon owner);
    public abstract void ReloadEnd(BaseWeapon owner);
    public virtual void HandleReloadContinuation(BaseWeapon weapon)
    {
        weapon.WeaponRuntimeData.State = WeaponState.IDLE;
        weapon.WeaponAnimator.ResetAnimParams();
    }

}




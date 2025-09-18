using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ReloadStrategySO : ScriptableObject
{
    public bool CanBeReloadCanceled = false;

    public abstract void ReloadStart(BaseWeapon_v2 owner);
    public abstract void PerformReload(BaseWeapon_v2 owner);
    public abstract void ReloadEnd(BaseWeapon_v2 owner);
    public virtual void HandleReloadContinuation(BaseWeapon_v2 weapon)
    {
        weapon.WeaponRuntimeData.State = WeaponState.IDLE;
        weapon.WeaponAnimator.ResetAnimParams();
    }

}




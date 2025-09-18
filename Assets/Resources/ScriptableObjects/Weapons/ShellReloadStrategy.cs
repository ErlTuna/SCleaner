using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ReloadStrategy", menuName = "Weapons/Reload Strategies/Shell Reload")]
public class ShellReloadStrategy : ReloadStrategySO
{
    readonly int taken = 1;
    int needed = 0;

    public override void ReloadStart(BaseWeapon_v2 owner)
    {
        if (owner.AmmoManager.HasReserveAmmo() != true || owner.WeaponRuntimeData.State == WeaponState.RELOADING) return;

        owner.WeaponRuntimeData.State = WeaponState.RELOADING;
        owner.WeaponAnimator.StartReloadAnim();
    }

    public override void PerformReload(BaseWeapon_v2 owner)
    {
        needed = owner.WeaponConfig.RoundCapacity - owner.WeaponRuntimeData.CurrentAmmo;
        if (needed != 0 && owner.AmmoManager.HasReserveAmmo())
        {
            Debug.Log("Loading shell");
            owner.WeaponRuntimeData.CurrentAmmo += taken;
            owner.WeaponRuntimeData.ReserveAmmo -= taken;
        }


        //WeaponEvents.RaiseWeaponReload(Owner.WeaponRuntimeData);
    }

    public override void HandleReloadContinuation(BaseWeapon_v2 weapon)
    {
        if (weapon.AmmoManager.CanReload())
        {
            weapon.WeaponAnimator.SetTrigger("LoopReload");
            return;
        }

        weapon.WeaponAnimator.ResetAnimParams();
        weapon.WeaponAnimator.SetTrigger("ReloadCompleted");
        weapon.WeaponAnimator.SetBool("isReloading", false);
        ReloadEnd(weapon);        
    }

    public override void ReloadEnd(BaseWeapon_v2 owner)
    {
        owner.WeaponRuntimeData.State = WeaponState.IDLE;
    }
}

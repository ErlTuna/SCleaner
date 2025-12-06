using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ReloadStrategy", menuName = "ScriptableObjects/Weapon/Reload Strategies/Shell Reload")]
public class ShellReloadStrategy : ReloadStrategySO
{
    readonly int taken = 1;
    int needed = 0;

    public override void ReloadStart(BaseWeapon owner)
    {
        if (owner.AmmoManager.HasReserveAmmo() != true || owner.WeaponRuntimeData.State == WeaponState.RELOADING) return;
        if (owner.AmmoManager.CanReload() != true) return;

        owner.WeaponRuntimeData.State = WeaponState.RELOADING;
        owner.WeaponAnimator.StartReloadAnim();
    }

    public override void PerformReload(BaseWeapon owner)
    {
        needed = owner.WeaponConfig.RoundCapacity - owner.WeaponRuntimeData.CurrentAmmo;
        if (needed != 0 && owner.AmmoManager.HasReserveAmmo())
        {

            owner.WeaponRuntimeData.CurrentAmmo += taken;

            if (owner.WeaponConfig.HasInfiniteReserveAmmo == false)
                owner.WeaponRuntimeData.ReserveAmmo -= taken;
        }


        //WeaponEvents.RaiseWeaponReload(Owner.WeaponRuntimeData);
    }

    public override void HandleReloadContinuation(BaseWeapon weapon)
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

    public override void ReloadEnd(BaseWeapon owner)
    {
        owner.WeaponRuntimeData.State = WeaponState.IDLE;
    }
}

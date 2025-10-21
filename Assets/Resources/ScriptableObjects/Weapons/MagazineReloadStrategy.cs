using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReloadStrategy", menuName = "Weapons/Reload Strategies/Magazine Reload")]
public class MagazineReloadStrategy : ReloadStrategySO
{

    public override void ReloadStart(BaseWeapon owner)
    {
        if (owner.AmmoManager.CanReload() != true || owner.WeaponRuntimeData.State != WeaponState.IDLE) return;

        owner.WeaponRuntimeData.State = WeaponState.RELOADING;
        owner.WeaponAnimator.StartReloadAnim();
    }
    public override void PerformReload(BaseWeapon owner)
    {
        if (owner.WeaponConfig.HasInfiniteReserveAmmo){
            owner.WeaponRuntimeData.CurrentAmmo = owner.WeaponConfig.RoundCapacity;
            Debug.Log("Has infinite ammo");
            return;
        }

        int needed = owner.WeaponConfig.RoundCapacity - owner.WeaponRuntimeData.CurrentAmmo;
        //owner.WeaponRuntimeData.CurrentAmmo = 0;
        int taken = Mathf.Min(needed, owner.WeaponRuntimeData.ReserveAmmo);

        owner.WeaponRuntimeData.CurrentAmmo += taken;
        owner.WeaponRuntimeData.ReserveAmmo -= taken;

        //notify UI
        //WeaponEvents.RaiseWeaponReload(owner.WeaponRuntimeData);

    }

    public override void ReloadEnd(BaseWeapon owner)
    {
        owner.WeaponRuntimeData.State = WeaponState.IDLE;
    }

    
}

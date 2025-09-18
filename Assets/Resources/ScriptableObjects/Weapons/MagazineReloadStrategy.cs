using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReloadStrategy", menuName = "Weapons/Reload Strategies/Magazine Reload")]
public class MagazineReloadStrategy : ReloadStrategySO
{

    public override void ReloadStart(BaseWeapon_v2 owner)
    {
        if (owner.AmmoManager.CanReload() != true || owner.WeaponRuntimeData.State != WeaponState.IDLE) return;

        owner.WeaponRuntimeData.State = WeaponState.RELOADING;
        owner.WeaponAnimator.StartReloadAnim();
    }
    public override void PerformReload(BaseWeapon_v2 owner)
    {
        int needed = owner.WeaponConfig.RoundCapacity - owner.WeaponRuntimeData.CurrentAmmo;
        int taken = Mathf.Min(needed, owner.WeaponRuntimeData.ReserveAmmo);

        owner.WeaponRuntimeData.CurrentAmmo += taken;
        owner.WeaponRuntimeData.ReserveAmmo -= taken;

        //notify UI
        //WeaponEvents.RaiseWeaponReload(owner.WeaponRuntimeData);

    }

    public override void ReloadEnd(BaseWeapon_v2 owner)
    {
        owner.WeaponRuntimeData.State = WeaponState.IDLE;
    }

    
}

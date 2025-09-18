using UnityEngine;

public class ReloadHandler
{
    [SerializeField] ReloadStrategySO reloadStrategy;
    public BaseWeapon_v2 owner;
    protected AmmoTracker _ammoTracker;
    public WeaponAnimator _weaponAnimator;

    public ReloadHandler(BaseWeapon_v2 owner, AmmoTracker tracker, WeaponAnimator animator)
    {
        this.owner = owner;
        _ammoTracker = tracker;
        //_animator = animator;
    }

    protected bool CanReload()
    {
        return
        _ammoTracker.CurrentAmmo < _ammoTracker.RoundCapacity
        && _ammoTracker.ReserveAmmo > 0
        && owner.WeaponRuntimeData.State != WeaponState.RELOADING;
    }

    public virtual void ReloadStart()
    {
        if (owner.WeaponRuntimeData.State == WeaponState.RELOADING || !CanReload()) return;

        owner.WeaponRuntimeData.State = WeaponState.RELOADING;
        //_weaponAnimator.StartReloadAnim();
        _weaponAnimator.animator.SetBool("isReloading", true);
    }

    public void Reload()
    {
        //reloadStrategy.StartReload(this);
        WeaponEvents.RaiseWeaponReload(owner.WeaponRuntimeData);
        FinishReload();
    }

    protected virtual void FinishReload()
    {
        if (CanReload() != true)
        {
            owner.WeaponRuntimeData.State = WeaponState.IDLE;
            WeaponEvents.RaiseWeaponReload(owner.WeaponRuntimeData);
        }

    } 
    public void ResetState() => owner.WeaponRuntimeData.State = WeaponState.IDLE;

}

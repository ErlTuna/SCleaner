using System.Collections.Generic;
using UnityEngine;

public class BasicShotgun : PlayerWeapon, IInstantFiringWeapon
{
    List<GameObject> _bullets = new();
    IShellBasedWeaponAnimator _shellBasedWeaponAnimator;

    void OnEnable()
    {
        weaponAnimatorComponent.OnBulletSpawnPointReached += SpawnBullet;
        weaponAnimatorComponent.OnAttackAnimEnd += OnPrimaryAttackAnimEnd;

        _shellBasedWeaponAnimator = weaponAnimatorComponent as IShellBasedWeaponAnimator;
        _shellBasedWeaponAnimator.OnShellInserted += HandleShellInserted;
    }

    void OnDisable()
    {
        weaponAnimatorComponent.OnBulletSpawnPointReached -= SpawnBullet;
        weaponAnimatorComponent.OnAttackAnimEnd -= OnPrimaryAttackAnimEnd;
        
        _shellBasedWeaponAnimator.OnShellInserted -= HandleShellInserted;
        
    }
    

    void Update()
    {
        CheckAmmoStatus();
    }

    #region Overrides

    // This weapon can fire while reloading if it has any ammo (reload cancellable)
    public override bool CanFire()
    {
        return AmmoManager.HasAmmo() && PrimaryAttackStrategy && WeaponRuntimeData.State != WeaponState.PRIMARY_ATTACK;
    }


    # region Input Handlers
    public override void HandlePrimaryAttackInput()
    {
        if (CanFire())
        {
            PrimaryAttackStrategy.OnTriggerPressed(this);
        }

        else
        {
            weaponRuntimeData.isTriggerHeld = true;
        }
    }
    
    public override void HandlePrimaryAttackInputRelease()
    {
        if (weaponRuntimeData.isTriggerHeld == false) return;

        weaponRuntimeData.isTriggerHeld = false;
        weaponAnimatorComponent.SetBool("isTriggerHeld", false);
    }

    public override void HandleReloadInput()
    {
        if (AmmoManager.CanReload() && WeaponRuntimeData.State != WeaponState.RELOADING)
        {    
            ReloadContext reloadContext = CreateReloadContext();
            AmmoManager.SetReloadContext(reloadContext);

            SetState(WeaponState.RELOADING);
            WeaponAnimator.StartReloadAnim();
        }
    }

    // This is called through an event inside WeaponAnimator.
    protected override void SpawnBullet()
    {
        BulletConfigSO bulletData;
        for (int i = 0; i < FiringPoints.Length; ++i)
        {
            bulletData = WeaponConfig.BulletConfig;
            GameObject bullet = Instantiate(bulletData.Prefab, FiringPoints[i].transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(gameObject, FiringPoints[i].transform.right, WeaponRuntimeData, WeaponConfig, i);
            _bullets.Add(bullet);
        }

        AmmoManager.UseAmmo();
    }

    #endregion
    


    # region Animation End Handlers (subscribed in OnEnable, called by WeaponAnimator)

    // This weapon spawns bullet using an animation event.
    protected override void OnPrimaryAttackAnimEnd()
    {
        if (weaponRuntimeData.isTriggerHeld && CanContinuePrimaryAttack())
            weaponAnimatorComponent.LoopPrimaryAttackAnim();
        
        else 
            SetState(WeaponState.IDLE);
    }

    protected override void OnReloadAnimEnd()
    {
        weaponAnimatorComponent.ResetAnimParams();
        weaponAnimatorComponent.SetTrigger("ReloadCompleted");
        SetState(WeaponState.IDLE);
    }

    // This method is for one cycle of the overall reload
    void HandleShellInserted()
    {
        AmmoManager.UseReloadStrategy();
        
        // If we can keep reloading after a shell insertion, keep going.
        if (AmmoManager.CanReload())
        {
            // Tell the animator to loop the shell insert animation
            _shellBasedWeaponAnimator.LoopReload();
        }
        
        else
        {
            OnReloadAnimEnd();
        }
    }

    public void OnAttackInputReleased()
    {
        if (WeaponRuntimeData.State == WeaponState.PRIMARY_ATTACK)
        {
            weaponAnimatorComponent.ResetAnimParams();
            SetState(WeaponState.IDLE);
        }
    }

    #endregion

    #endregion
}


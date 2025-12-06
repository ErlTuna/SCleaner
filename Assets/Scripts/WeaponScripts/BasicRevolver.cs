using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRevolver : PlayerWeapon
{
    void Start()
    {
        //InitializeWithConfig();
    }

    void Update()
    {
        //weapon runs out of ammo but has reserve and isn't actively reloading, try starting a reload
        if (AmmoManager.HasAmmo() == false && AmmoManager.CanReload() && WeaponRuntimeData.State != WeaponState.RELOADING)
        {
            AmmoManager.HandleReloadStart();
        }
    }


    public override void HandlePrimaryAttackInput()
    {
        if (CanFire() == false && CanDryFire())
        {
            TryDryFire();
            return;
        }

        if (WeaponRuntimeData.State != WeaponState.IDLE) return;
            HandlePreAttack();
    
    }

    public override void HandlePrimaryAttackInputCancel()
    {
        if (WeaponRuntimeData.State == WeaponState.PRE_PRIMARY_ATTACK)
            PrimaryAttackStrategy.HandleAttackCanceled(this);
    }

    public void HandlePreAttack()
    {
        if (PrimaryAttackStrategy != null)
            PrimaryAttackStrategy.HandlePreAttack(this);
    }

    public void HandlePreAttackEnd()
    {
        if (PrimaryAttackStrategy != null)
            PrimaryAttackStrategy.HandleAttackStart(this);
    }

    public override void HandleReloadInput()
    {
        AmmoManager.HandleReloadStart();
    }

    public override void HandleReloadStart()
    {
        //if (AmmoManager.CanReload() != true || WeaponRuntimeData.State == WeaponState.FIRING) return;

        //WeaponAnimator.StartReloadAnim();
    }

    public override void SpawnBullet()
    {
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletConfig.Prefab, FiringPoints[0].transform.position, transform.rotation);
        instantiatedBullet.GetComponent<Bullet>().Initialize(gameObject, FiringPoints[0].transform.right, WeaponRuntimeData, WeaponConfig);
        AmmoManager.UseAmmo();
        //WeaponEvents.RaiseAmmoUsed(WeaponRuntimeData);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRevolver : BaseWeapon_v2
{
        void Start()
    {
        WeaponRuntimeData = new WeaponRuntimeData(WeaponConfig);
    }

    void Update()
    {
        //weapon runs out of ammo but has reserve and isn't actively reloading, try starting a reload
        if (AmmoManager.HasAmmo() == false && WeaponRuntimeData.State != WeaponState.RELOADING)
        {
            AmmoManager.HandleReloadStart();
        }

        //weapon has no ammo and no reserve left, cancel the input of the player and reset the weapon's state and animator params
        else if (AmmoManager.HasAmmo() == false && AmmoManager.HasReserveAmmo() == false)
        {
            WeaponRuntimeData.State = WeaponState.IDLE;
            WeaponAnimator.ResetAnimParams();
        }
    }


    public override void HandlePrimaryAttackInput()
    {
        if (AmmoManager.HasAmmo() == false || WeaponRuntimeData.State != WeaponState.IDLE) return;


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
        Debug.Log("Spawning bullet");
        Bullet instantiatedBullet = Instantiate(WeaponConfig.BulletData.bulletPrefab, FiringPoints[0].transform.position, transform.rotation).GetComponent<Bullet>();
        instantiatedBullet.SetupBulletParameters(WeaponConfig.BulletData.projectileSpeed, WeaponConfig.BulletData.size, WeaponConfig.Damage, WeaponConfig.BulletData.lifeTime);

        AmmoManager.UseAmmo();

    }
}

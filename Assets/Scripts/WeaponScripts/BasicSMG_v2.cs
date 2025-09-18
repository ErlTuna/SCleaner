using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSMG_v2 : BaseWeapon_v2
{
    void Start()
    {
        //WeaponRuntimeData = new WeaponRuntimeData(WeaponConfig);
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
        if (WeaponRuntimeData.State != WeaponState.IDLE) return;

        if(PrimaryAttackStrategy != null)
            PrimaryAttackStrategy.HandleAttackStart(this);
    }

    /*
    public void PerformPrimaryAttack()
    {
        Debug.Log("Performing primary attack");
        WeaponAnimator.StartPrimaryAttackAnim();
    }
    */

    //public override void HandlePrimaryAttackInputCancel(){}

    /*
    public void HandlePrimaryAttackEnd()
    {
        WeaponRuntimeData.State = WeaponState.IDLE;
        WeaponAnimator.ResetState();
    }
    */
    public override void HandleReloadInput()
    {
         AmmoManager.HandleReloadStart();
    }

    public override void HandleReloadStart()
    {
        //if (AmmoManager.CanReload() != true || WeaponRuntimeData.State == WeaponState.FIRING) return;

        //AmmoManager.ReloadStrategy.ReloadStart(this);
    }

    public override void SpawnBullet()
    {
        Bullet instantiatedBullet = Instantiate(WeaponConfig.BulletData.bulletPrefab, FiringPoints[0].transform.position, transform.rotation).GetComponent<Bullet>();
        instantiatedBullet.SetupBulletParameters(WeaponConfig.BulletData.projectileSpeed, WeaponConfig.BulletData.size, WeaponConfig.Damage, WeaponConfig.BulletData.lifeTime);


    }
}

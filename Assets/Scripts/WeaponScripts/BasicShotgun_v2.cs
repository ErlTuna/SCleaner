using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShotgun_v2 : BaseWeapon_v2
{
    List<GameObject> _bullets = new();
    void Start()
    {
        WeaponRuntimeData = new WeaponRuntimeData(WeaponConfig);
    }

    void Update()
    {
        if (AmmoManager.HasAmmo() == false && AmmoManager.HasReserveAmmo() == true && WeaponRuntimeData.State != WeaponState.RELOADING)
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
        if (PrimaryAttackStrategy != null)
            PrimaryAttackStrategy.HandleAttackStart(this);
    }

    public override void HandleReloadInput()
    {
        //HandleReloadStart();
        AmmoManager.HandleReloadStart();
    }

    public override void HandleReloadStart()
    {
        //if (AmmoManager.HasReserveAmmo() != true || WeaponRuntimeData.State == WeaponState.RELOADING) return;
    }

    public override void SpawnBullet()
    {
        BulletSO bulletData;
        for (int i = 0; i < FiringPoints.Length; ++i)
        {
            bulletData = WeaponConfig.BulletData;
            GameObject bullet = Instantiate(bulletData.bulletPrefab, FiringPoints[i].transform.position, FiringPoints[i].transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            bulletScript.SetupBulletParameters(bulletData.projectileSpeed, bulletData.size, WeaponRuntimeData.Damage, bulletData.lifeTime);
            _bullets.Add(bullet);
        }


    }

}
    



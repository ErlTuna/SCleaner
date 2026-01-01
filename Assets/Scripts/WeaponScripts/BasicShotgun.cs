using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShotgun : PlayerWeapon
{
    List<GameObject> _bullets = new();

    void Update()
    {
        if (AmmoManager.HasAmmo() == false && AmmoManager.HasReserveAmmo() == true && WeaponRuntimeData.State != WeaponState.RELOADING)
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

        //if (ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint, rayCastEndPoint, WeaponConfig.environmentLayers, WeaponConfig.enemyLayer)) return;
        if (CanFire() && PrimaryAttackStrategy != null)
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
        //WeaponEvents.RaiseAmmoUsed(WeaponRuntimeData);
    }
    
    public override bool CanFire()
    {
        return AmmoManager.HasAmmo();
    }

}
    



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSMG : BaseWeapon
{
    void Start()
    {
        //WeaponRuntimeData = new WeaponRuntimeData(WeaponConfig);
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
        
        //if (ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint, rayCastEndPoint, WeaponConfig.environmentLayers, WeaponConfig.enemyLayer)) return;

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

        //AmmoManager.ReloadStrategy.ReloadStart(this);
    }

    public override void SpawnBullet()
    {

        Collider2D overlappingEnemyHitbox = ObstructionChecker.CheckMuzzleEnemyOverlap(muzzleTipCheck, WeaponConfig.enemyLayer);

        if (overlappingEnemyHitbox)
        {
            if (overlappingEnemyHitbox.TryGetComponent<IDamageable>(out var damageable))
            {
                DamageContext context = new(gameObject, transform.position, WeaponRuntimeData.Damage, WeaponConfig.PushForce);
                damageable.TakeDamage(context);
                AmmoManager.UseAmmo();
                return;
            }
        }

        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletData.Prefab, FiringPoints[0].transform.position, transform.rotation);
        instantiatedBullet.SetActive(false);
        instantiatedBullet.GetComponent<Bullet>().Setup(gameObject, WeaponRuntimeData, WeaponConfig);
        instantiatedBullet.SetActive(true);
        
        //instantiatedBullet.SetupBulletParameters(WeaponConfig.BulletData.ProjectileSpeed, WeaponConfig.BulletData.Size, WeaponConfig.Damage, WeaponConfig.BulletData.LifeTime);


    }
}

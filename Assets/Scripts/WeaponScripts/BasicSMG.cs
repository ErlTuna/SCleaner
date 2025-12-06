using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSMG : PlayerWeapon
{
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

        /*
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
        */

        Quaternion spreadRot = CalculateBulletSpread();
        Vector2 trajectory = spreadRot * Vector2.right;
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletConfig.Prefab, FiringPoints[0].transform.position, Quaternion.identity);

        instantiatedBullet.GetComponent<Bullet>().Initialize(gameObject, trajectory, WeaponRuntimeData, WeaponConfig);
        WeaponRuntimeData.TimeSinceLastFired = Time.unscaledTime;
        //weaponSoundManager.TryPlayFiringSFX();
        AmmoManager.UseAmmo();
        //WeaponEvents.RaiseAmmoUsed(WeaponRuntimeData);
    }
}

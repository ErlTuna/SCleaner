using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShotgun : PlayerWeapon
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

        Collider2D overlappingEnemyHitbox = ObstructionChecker.CheckMuzzleEnemyOverlap(muzzleTipCheck, WeaponConfig.enemyLayer);

        if (overlappingEnemyHitbox)
        {
            if (overlappingEnemyHitbox.TryGetComponent<IDamageable>(out var damageable))
            {
                DamageContext context = new(gameObject, transform.position, WeaponRuntimeData.Damage * WeaponConfig.BulletPerShot, WeaponConfig.PushForce);
                damageable.TakeDamage(context);
                AmmoManager.UseAmmo();
                return;
            }
        }

        BulletConfigSO bulletData;
        for (int i = 0; i < FiringPoints.Length; ++i)
        {
            bulletData = WeaponConfig.BulletData;
            GameObject bullet = Instantiate(bulletData.Prefab, FiringPoints[i].transform.position, FiringPoints[i].transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Setup(gameObject, WeaponRuntimeData, WeaponConfig);

            //bulletScript.SetupBulletParameters(bulletData.ProjectileSpeed, bulletData.Size, WeaponRuntimeData.Damage, bulletData.LifeTime);
            _bullets.Add(bullet);
        }

    }
    
    public override bool CanFire()
    {
        return AmmoManager.HasAmmo();
    }

}
    



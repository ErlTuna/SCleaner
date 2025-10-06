using Unity.Mathematics;
using UnityEngine;

public class BasicPistol : BaseWeapon
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

        //if (ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint, rayCastEndPoint, WeaponConfig.environmentLayers, WeaponConfig.enemyLayer)) return;
        if (CanFire() && PrimaryAttackStrategy != null)
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

        
        Quaternion bulletRotation = CalculateBulletSpread();
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletData.Prefab, FiringPoints[0].transform.position, bulletRotation);
        instantiatedBullet.SetActive(false);
        instantiatedBullet.GetComponent<Bullet>().Setup(gameObject, WeaponRuntimeData, WeaponConfig);
        instantiatedBullet.SetActive(true);
        AmmoManager.UseAmmo();
        WeaponRuntimeData.TimeSinceLastFired = Time.unscaledTime;
    }

}

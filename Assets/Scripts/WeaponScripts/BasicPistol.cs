using Unity.Mathematics;
using UnityEngine;

public class BasicPistol : PlayerWeapon
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
        Quaternion spreadRot = CalculateBulletSpread();
        Vector2 trajectory = spreadRot * Vector2.right;
        GameObject instantiatedBullet = Instantiate(WeaponConfig.BulletPrefab, FiringPoints[0].transform.position, Quaternion.identity);

        
        instantiatedBullet.GetComponent<Bullet>().Initialize(gameObject, trajectory, WeaponRuntimeData, WeaponConfig);
        WeaponRuntimeData.TimeSinceLastFired = Time.unscaledTime;
        AmmoManager.UseAmmo();
        
        //WeaponEvents.RaiseAmmoUsed(WeaponRuntimeData);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class OLD_BasicShotgun : OLD_BaseWeapon
{
    public GameObject rayCastStartPoint;
    public GameObject rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    private BulletConfigSO bulletInfo;
    List<GameObject> bullets = new List<GameObject>();
    [SerializeField] Transform[] firingPoints;
    bool _hasReloadStarted;

    void HandleBulletDestroyed(GameObject bullet)
    {
        bullets.Remove(bullet);
    }

    void OnDestroy()
    {
        Bullet.OnBulletDestroyedEvent_v2 -= HandleBulletDestroyed;
    }

    void Awake()
    {
        Bullet.OnBulletDestroyedEvent_v2 += HandleBulletDestroyed;

        if (weaponConfig == null)
        {
            //Debug.Log("Shotgun weapon info missing. Loading resource.");
            weaponConfig = Resources.Load<WeaponConfigSO>("ScriptableObjects/Weapons/BasicShotgunData");
        }

        if (weaponConfig.BulletData == null)
        {
            //Debug.Log("Shotgun bullet info (part of shotgun weapon info) missing. Loading shotgun bullet info resource.");
            //bulletInfo = Resources.Load<BulletSO>("ScriptableObjects/Weapons/ShotgunBulletData");
            //WeaponInfo.bulletInfo = bulletInfo;
        }
        else
        {
            //Debug.Log("Shotgun bullet info (part of shotgun weapon info) already exists.");
            bulletInfo = weaponConfig.BulletData;
        }

        if (!_animator)
        {
            _animator = GetComponent<Animator>();
        }

        if (_animator.runtimeAnimatorController == null)
        {
            print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Shotgun_AC");
            _animator.runtimeAnimatorController = _runtimeAnimatorController;
        }
    }

    void Start()
    {
        weaponRuntimeData = new WeaponRuntimeData(weaponConfig);
    }

    void Update()
    {
        if (!HasAmmo() && weaponRuntimeData.ReserveAmmo != 0 && weaponRuntimeData.State != WeaponState.RELOADING)
        {
            HandlePrimaryAttackInputCancel();
            HandleReloadStart();
        }
        else if (!HasAmmo() && weaponRuntimeData.ReserveAmmo == 0 && weaponRuntimeData.State != WeaponState.RELOADING)
        {
            HandlePrimaryAttackInputCancel();
        }
    }
    public override void PrimaryAttack()
    {

        
        GameObject overlappingEnemy = ObstructionChecker.CheckMuzzleEnemyOverlap(muzzleTipCheck, enemyLayer);
        if (overlappingEnemy != null)
        {
            IDamageable damageable = overlappingEnemy.GetComponent<IDamageable>();
            //damageable?.TakeDamage(weaponRuntimeData.Damage * weaponRuntimeData.PelletCount);
        }
        
        else
        {
            for (int i = 0; i < firingPoints.Length; ++i)
            {
                GameObject bullet = Instantiate(weaponConfig.BulletData.Prefab, firingPoints[i].transform.position, firingPoints[i].transform.rotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                //bulletScript.SetupBulletParameters(bulletInfo.ProjectileSpeed, bulletInfo.Size, weaponRuntimeData.Damage, bulletInfo.LifeTime);
                bullets.Add(bullet);
            }
        }
        
        weaponRuntimeData.CurrentAmmo--;
        WeaponEvents.RaiseWeaponFired(weaponRuntimeData);
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.5f);
        //Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.green);
    }


    public override void Reload()
    {

        ++weaponRuntimeData.CurrentAmmo;
        --weaponRuntimeData.ReserveAmmo;
        WeaponEvents.RaiseWeaponReload(weaponRuntimeData);

        if (weaponRuntimeData.CurrentAmmo < weaponConfig.RoundCapacity)
        {
            _animator.SetTrigger("ReloadLoopTrigger");
            return;
        }
        HandleReloadEnd();
    }

    public override void HandlePrimaryAttackInput()
    {
        if (ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;
        //if(ObstructionChecker.CheckMuzzleEnvironmentOverlap(muzzleTipCheck, environmentLayers)) return;

        if (!HasAmmo()) return;
        if (weaponRuntimeData.State == WeaponState.PRIMARY_ATTACK) return;

        if (_hasReloadStarted && HasAmmo() || weaponRuntimeData.State == WeaponState.RELOADING && HasAmmo())
        {
            _hasReloadStarted = false;

            weaponRuntimeData.State = WeaponState.IDLE;
            _animator.SetBool("hasReloadStarted", false);
            _animator.ResetTrigger("ReloadLoopTrigger");
        }

        weaponRuntimeData.State = WeaponState.PRIMARY_ATTACK;
        _animator.SetBool("isFiring", true);
    }

    public override void HandlePrimaryAttackInputCancel()
    {
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
    }
    public override void HandleFiringAnimationEnd()
    {

    }

    public override void HandleReloadStart()
    {
        if (weaponRuntimeData.ReserveAmmo == 0 || weaponRuntimeData.CurrentAmmo == weaponConfig.RoundCapacity || weaponRuntimeData.State == WeaponState.RELOADING)
            return;

        weaponRuntimeData.State = WeaponState.RELOADING;
        _animator.SetBool("isFiring", false);

        _animator.SetBool("hasReloaded", false);
        _hasReloadStarted = true;
        _animator.SetBool("hasReloadStarted", true);

    }

    public void HandleReloadLoopStart()
    {

        if (weaponRuntimeData.State != WeaponState.RELOADING)
        {
            _animator.SetBool("hasReloadStarted", false);
            weaponRuntimeData.State = WeaponState.RELOADING;
            _animator.SetBool("isReloading", true);
        }

    }

    public void HandleReloadStartEnd()
    {
        _animator.SetBool("hasReloadStarted", false);
    }

    public override void HandleReloadEnd()
    {
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isReloading", false);
        _animator.SetBool("hasReloaded", true);
    }

    public override void ResetWeaponState()
    {
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
        _animator.SetBool("isReloading", false);
        _animator.SetBool("hasReloadStarted", false);
        _animator.SetBool("hasReloaded", false);
        _animator.ResetTrigger("ReloadTrigger");
        _animator.ResetTrigger("ReloadLoopTrigger");
        _animator.ResetTrigger("ReloadCancelTrigger");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = weaponConfig.Sprite;

    }
}
*/
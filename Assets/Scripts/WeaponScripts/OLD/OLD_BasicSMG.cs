using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class OLD_BasicSMG : OLD_BaseWeapon
{
    public Transform rayCastStartPoint;
    public Transform rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;

    void Awake()
    {
        if (weaponConfig == null)
        {
            //Debug.Log("SMG weapon info missing. Loading resource.");
            //weaponRuntimeData = Resources.Load<WeaponData>("ScriptableObjects/SMGSO");
        }
        //print(WeaponInfo);

        if (weaponConfig.BulletData == null)
        {
            //Debug.Log("SMG bullet info (part of SMG weapon info) missing. Loading pistol bullet info resource.");
            //weaponConfig.bulletData = Resources.Load<BulletSO>("ScriptableObjects/SMGBulletSO");
            //WeaponInfo.bulletInfo = bulletInfo;
        }
        else
        {
            //Debug.Log("SMG bullet info (part of SMG bullet info) already exists.");
        }

        if (firingPoint == null)
        {
            //Debug.Log("FiringPoint not assigned. Finding...");
            firingPoint = GetComponentInChildren<Transform>();
            //print(firingPoint.transform.position.x + "    " + firingPoint.transform.position.y);
        }

        if (_animator.runtimeAnimatorController == null)
        {
            //print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/SMG_AC");
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

    public void ReceiveFiringPoint(Transform firingPoint)
    {
        this.firingPoint = firingPoint;
    }

    public override void PrimaryAttack()
    {

        Bullet instantiatedBullet = Instantiate(weaponConfig.BulletData.Prefab, firingPoint.transform.position, transform.rotation).GetComponent<Bullet>();
        //instantiatedBullet.SetupBulletParameters(weaponConfig.BulletData.ProjectileSpeed, weaponConfig.BulletData.Size, weaponRuntimeData.Damage, weaponConfig.BulletData.LifeTime);


        --weaponRuntimeData.CurrentAmmo;
        WeaponEvents.RaiseWeaponFired(weaponRuntimeData);
    }


    public override void Reload()
    {
        int ammoBeforeReload = weaponRuntimeData.CurrentAmmo;
        //weaponRuntimeData.CurrentAmmo = 0;

        if (weaponConfig.RoundCapacity - ammoBeforeReload <= weaponRuntimeData.ReserveAmmo)
        {
            weaponRuntimeData.ReserveAmmo -= weaponConfig.RoundCapacity - ammoBeforeReload;
            weaponRuntimeData.CurrentAmmo = weaponConfig.RoundCapacity;
        }
        else
        {
            weaponRuntimeData.CurrentAmmo += weaponRuntimeData.ReserveAmmo;
            weaponRuntimeData.ReserveAmmo = 0;
        }


        weaponRuntimeData.State = WeaponState.IDLE;
        WeaponEvents.RaiseWeaponReload(weaponRuntimeData);
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.25f);
        //Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.red);
    }

    public override void HandlePrimaryAttackInput()
    {
        if (!HasAmmo() || weaponRuntimeData.State == WeaponState.RELOADING || weaponRuntimeData.State == WeaponState.PRIMARY_ATTACK) return;
        if (ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;

        weaponRuntimeData.State = WeaponState.PRIMARY_ATTACK;
        _animator.SetBool("isFiring", true);
    }

    public override void HandlePrimaryAttackInputCancel()
    {
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
    }

    public override void HandleReloadStart()
    {

        if (weaponRuntimeData.ReserveAmmo == 0 || weaponRuntimeData.CurrentAmmo == weaponConfig.RoundCapacity || weaponRuntimeData.State == WeaponState.RELOADING)
            return;

        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
        weaponRuntimeData.State = WeaponState.RELOADING;
        _animator.SetTrigger("ReloadTrigger");
    }

    public override void HandleReloadEnd()
    {
        weaponRuntimeData.State = WeaponState.IDLE;
    }


    public override void ResetWeaponState()
    {
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
        _animator.ResetTrigger("ReloadTrigger");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = weaponConfig.Sprite;
    }

    public override void HandleFiringAnimationEnd()
    {
        //no op
    }
}
*/

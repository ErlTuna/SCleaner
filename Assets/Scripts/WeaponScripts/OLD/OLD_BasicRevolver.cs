using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLD_BasicRevolver : OLD_BaseWeapon
{
    public Transform rayCastStartPoint;
    public Transform rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    bool _isHoldingTrigger = false;


    void Awake(){
        if (weaponRuntimeData == null){
            //Debug.Log("Revolver weapon info missing. Loading resource.");
            weaponConfig = Resources.Load<WeaponConfigSO>("ScriptableObjects/Weapons/BasicRevolverData");
        }
        //print(WeaponInfo);

        if(weaponConfig.BulletData == null){
            //Debug.Log("Revolver bullet info (part of Revolver weapon info) missing. Loading Revolver bullet info resource.");
            //bulletInfo = Resources.Load<BulletSO>("ScriptableObjects/RevolverBulletSO");
            //WeaponInfo.bulletInfo = bulletInfo;
        }
        else {
            //Debug.Log("Revolver bullet info (part of Revolver bullet info) already exists.");
            
        }

        if (firingPoint == null){
            //Debug.Log("FiringPoint not assigned. Finding...");
            firingPoint = GetComponentInChildren<Transform>();
            //print(firingPoint.transform.position. x + "    " + firingPoint.transform.position.y);
        }
        if (_animator == null){
            //print("Animator is null, fetching.");
            _animator = GetComponent<Animator>();
            
        }
        
        if (_animator.runtimeAnimatorController == null){
            //print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Revolver_AC");
            _animator.runtimeAnimatorController = _runtimeAnimatorController;
        }
    }

    void Start()
    {
        weaponRuntimeData = new WeaponRuntimeData(weaponConfig);
    }

    void Update(){

        //Weapon.weaponRuntimeData.state != WeaponState.RELOADING;
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
        Bullet instantiatedBullet = Instantiate(weaponConfig.BulletData.Prefab, firingPoint.transform.position, transform.rotation).GetComponent<Bullet>();
        //instantiatedBullet.SetupBulletParameters(weaponConfig.BulletData.ProjectileSpeed, weaponConfig.BulletData.Size, weaponConfig.Damage, weaponConfig.BulletData.LifeTime);


        --weaponRuntimeData.CurrentAmmo;
        WeaponEvents.RaiseWeaponFired(weaponRuntimeData);
    }

    public override void Reload()
    {
        int ammoBeforeReload = weaponRuntimeData.CurrentAmmo;
        weaponRuntimeData.State = WeaponState.RELOADING;

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

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.5f);
        Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.green);
    }

    public override void HandlePrimaryAttackInput()
    {
        if(ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;  
        if(!HasAmmo() || weaponRuntimeData.State == WeaponState.RELOADING || _isHoldingTrigger) return;
        _isHoldingTrigger = true;

        if (_animator != null)
        {
                _animator.SetBool("isTriggerHeld", true);
        }
        //_animator.SetBool("isTriggerHeld", true);
    }

    public override void HandlePrimaryAttackInputCancel(){
        weaponRuntimeData.State = WeaponState.IDLE;
        _isHoldingTrigger = false;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
    }

    public void HandleTriggerHoldEnd(){
        weaponRuntimeData.State = WeaponState.PRIMARY_ATTACK;
        _animator.SetTrigger("FireTrigger");
        _animator.SetBool("isFiring", true);
    }

    public override void HandleFiringAnimationEnd()
    {
        _isHoldingTrigger = false;
        weaponRuntimeData.State = WeaponState.IDLE;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
        
    }

    public override void HandleReloadStart()
    {
        if(weaponRuntimeData.ReserveAmmo == 0 || weaponRuntimeData.CurrentAmmo == weaponConfig.RoundCapacity || weaponRuntimeData.State == WeaponState.RELOADING) 
        return;
        
        if(weaponRuntimeData.State == WeaponState.PRIMARY_ATTACK) weaponRuntimeData.State = WeaponState.RELOADING;

        //weaponRuntimeData.state = WeaponState.RELOADING;
        //Weapon.isReloading = true;
        _animator.SetBool("isFiring", false);
        _animator.ResetTrigger("FireTrigger");
        _animator.SetTrigger("ReloadTrigger");
    }

    public override void HandleReloadEnd()
    {
        Reload();
    }


    public void ReceiveFiringPoint(Transform firingPoint){
        this.firingPoint = firingPoint;
    }

    public override void ResetWeaponState(){
        //Weapon.isFiring = false;
        //Weapon.isReloading = false;
        weaponRuntimeData.State = WeaponState.IDLE;
        _isHoldingTrigger = false;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
        _animator.SetBool("isLMBHeld", false);
        _animator.ResetTrigger("FireTrigger");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = weaponConfig.Sprite;
    }
}

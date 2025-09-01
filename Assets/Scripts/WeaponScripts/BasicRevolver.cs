using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRevolver : BaseWeapon
{
    public Transform rayCastStartPoint;
    public Transform rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    bool _isHoldingTrigger = false;


    void Awake(){
        if (data == null){
            //Debug.Log("Revolver weapon info missing. Loading resource.");
            data = Resources.Load<WeaponData>("ScriptableObjects/RevolverSO");
        }
        //print(WeaponInfo);

        if(data.bulletData == null){
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

    void Update(){

        //Weapon.data.state != WeaponState.RELOADING;
        if (!HasAmmo() && data.currentReserveAmmo != 0 && data.state != WeaponState.RELOADING)
        {
            HandlePrimaryAttackInputCancel();
            HandleReloadStart();
        }
        else if (!HasAmmo() && data.currentReserveAmmo == 0 && data.state != WeaponState.RELOADING)
        {
            HandlePrimaryAttackInputCancel();
        }
    }

    public override void PrimaryAttack()
    {
        Bullet instantiatedBullet = Instantiate(data.bulletData.bulletPrefab, firingPoint.transform.position, transform.rotation).GetComponent<Bullet>();
        instantiatedBullet.SetupBulletParameters(data.bulletData.projectileSpeed, data.bulletData.size, data.damage, data.bulletData.lifeTime);


        --data.currentAmmo;
    }

    public override void Reload()
    {
        int ammoBeforeReload = data.currentAmmo;
        data.currentAmmo = 0;

        if (data.roundCapacity - ammoBeforeReload <= data.currentReserveAmmo)
        {
            data.currentReserveAmmo -= data.roundCapacity - ammoBeforeReload;
            data.currentAmmo = data.roundCapacity;
        }
        else
        {
            data.currentAmmo += data.currentReserveAmmo;
            data.currentReserveAmmo = 0;
        }

        data.state = WeaponState.IDLE;
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.5f);
        Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.green);
    }

    public override void HandlePrimaryAttackInput()
    {
        if(ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;  
        if(!HasAmmo() || data.state == WeaponState.RELOADING || _isHoldingTrigger) return;
        _isHoldingTrigger = true;

        if (_animator != null)
        {
                _animator.SetBool("isTriggerHeld", true);
        }
        //_animator.SetBool("isTriggerHeld", true);
    }

    public override void HandlePrimaryAttackInputCancel(){
        data.state = WeaponState.IDLE;
        _isHoldingTrigger = false;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
    }

    public void HandleTriggerHoldEnd(){
        data.state = WeaponState.FIRING;
        _animator.SetTrigger("FireTrigger");
        _animator.SetBool("isFiring", true);
    }

    public override void HandleFiringAnimationEnd()
    {
        _isHoldingTrigger = false;
        data.state = WeaponState.IDLE;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
        
    }

    public override void HandleReloadStart()
    {
        if(data.currentReserveAmmo == 0 || data.currentAmmo == data.roundCapacity || data.state == WeaponState.RELOADING) 
        return;
        
        if(data.state == WeaponState.FIRING) data.state = WeaponState.RELOADING;

        //data.state = WeaponState.RELOADING;
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
        data.state = WeaponState.IDLE;
        _isHoldingTrigger = false;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
        _animator.SetBool("isLMBHeld", false);
        _animator.ResetTrigger("FireTrigger");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }
}

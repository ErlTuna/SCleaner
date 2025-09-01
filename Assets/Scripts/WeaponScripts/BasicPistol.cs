using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPistol : BaseWeapon
{
    public Transform rayCastStartPoint;
    public Transform rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;

    void Awake(){
        if (data == null)
        {
            //Debug.Log("Pistol weapon info missing. Loading resource.");
            data = Resources.Load<WeaponData>("ScriptableObjects/Weapons/PistolSO");
        }
         
        if(data.bulletData == null){
            //Debug.Log("Pistol bullet info (part of pistol weapon info) missing. Loading pistol bullet info resource.");
            //bulletInfo = Resources.Load<BulletSO>("ScriptableObjects/Weapons/PistolBulletSO");
        }
        else {
            //Debug.Log("Pistol bullet info (part of pistol bullet info) already exists.");
            //bulletInfo = bulletData;
        }

        if (firingPoint == null){
            //Debug.Log("FiringPoint not assigned. Finding...");
            firingPoint = GetComponentInChildren<Transform>();   
        }

        if (_animator == null){
            _animator = GetComponent<Animator>();
        }
        
        if (_animator.runtimeAnimatorController == null){
            //print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/Pistol_AC");
            _animator.runtimeAnimatorController = _runtimeAnimatorController;
        }
        //data.Init();
    }

    void Update(){
        if(!HasAmmo() && data.currentReserveAmmo != 0 && data.state != WeaponState.RELOADING){
            HandlePrimaryAttackInputCancel();
            HandleReloadStart();
        }
        else if (!HasAmmo() && data.currentReserveAmmo == 0 && data.state != WeaponState.RELOADING){
            HandlePrimaryAttackInputCancel();
        }
    }

    public void ReceiveFiringPoint(Transform firingPoint){
        this.firingPoint = firingPoint;
    }


    public override void PrimaryAttack()
    {
        Debug.Log("ATTACK!!!");
        if (!HasAmmo() || data.state == WeaponState.RELOADING) return;

        Bullet instantiatedBullet = Instantiate(data.bulletData.bulletPrefab, firingPoint.transform.position, transform.rotation).GetComponent<Bullet>();
        instantiatedBullet.SetupBulletParameters(data.bulletData.projectileSpeed, data.bulletData.size, data.damage, data.bulletData.lifeTime);


        --data.currentAmmo;
        WeaponEvents.RaiseWeaponFired(data);
    }

    public override void Reload()
    {
        int ammoBeforeReload = data.currentAmmo;
        data.currentAmmo = 0;
        data.state = WeaponState.RELOADING;

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
        WeaponEvents.RaiseWeaponReload(data);
    }

    void OnDrawGizmosSelected(){
        //Gizmos.DrawWireSphere(muzzleTipCheck.position, 0.5f);
        //Debug.DrawLine(rayCastStartPoint.position, rayCastEndPoint.position, Color.green);
    }

    public override void HandlePrimaryAttackInput()
    {

        if (ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint, rayCastEndPoint, environmentLayers, enemyLayer)) return;
        if (!HasAmmo() || data.state == WeaponState.RELOADING || data.state == WeaponState.FIRING) return;

        data.state = WeaponState.FIRING;
        _animator.SetBool("isFiring", true);
        
    }

    public override void HandlePrimaryAttackInputCancel(){
        data.state = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
    }

    public override void HandleReloadStart(){

        if(data.currentReserveAmmo == 0 || data.currentAmmo == data.roundCapacity || data.state == WeaponState.RELOADING) 
        return;

        data.state = WeaponState.IDLE;
        
        _animator.SetBool("isFiring", false);
        data.state = WeaponState.RELOADING;
        _animator.SetTrigger("ReloadTrigger");
    }

    public override void HandleReloadEnd(){
        data.state = WeaponState.IDLE;
    }

    public override void HandleFiringAnimationEnd()
    {
        //no op
    }
    public override void ResetWeaponState(){
        //Weapon.isFiring = false;
        //Weapon.isReloading = false;
        data.state = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
        _animator.ResetTrigger("ReloadTrigger");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        
    }
}

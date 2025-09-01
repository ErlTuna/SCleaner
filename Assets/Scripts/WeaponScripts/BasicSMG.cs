using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSMG : BaseWeapon
{
    public Transform rayCastStartPoint;
    public Transform rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;

    void Awake(){
        if (data == null){
            //Debug.Log("SMG weapon info missing. Loading resource.");
            data = Resources.Load<WeaponData>("ScriptableObjects/SMGSO");
        }
        //print(WeaponInfo);

        if(data.bulletData == null){
            //Debug.Log("SMG bullet info (part of SMG weapon info) missing. Loading pistol bullet info resource.");
            data.bulletData = Resources.Load<BulletSO>("ScriptableObjects/SMGBulletSO");
            //WeaponInfo.bulletInfo = bulletInfo;
        }
        else {
            //Debug.Log("SMG bullet info (part of SMG bullet info) already exists.");
        }

        if (firingPoint == null){
            //Debug.Log("FiringPoint not assigned. Finding...");
            firingPoint = GetComponentInChildren<Transform>();
            //print(firingPoint.transform.position.x + "    " + firingPoint.transform.position.y);
        }

        if (_animator.runtimeAnimatorController == null){
            //print("Runtime animator controller is null, fetching.");
            _runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimatorControllers/SMG_AC");
            _animator.runtimeAnimatorController = _runtimeAnimatorController;
        }
    }

    void Start(){
        if (!_animator){
            _animator = GetComponent<Animator>();
        }
    }

    void Update(){
        // Weapon.data.state != WeaponState.RELOADING
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

    public void ReceiveFiringPoint(Transform firingPoint){
        this.firingPoint = firingPoint;
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
        Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.25f);
        Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.red);
    }

    public override void HandlePrimaryAttackInput()
    {
        if(!HasAmmo() || data.state == WeaponState.RELOADING || data.state == WeaponState.FIRING) return;
        if(ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;  
        
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


    public override  void ResetWeaponState(){
        data.state = WeaponState.IDLE;
        _animator.SetBool("isFiring", false);
        _animator.ResetTrigger("ReloadTrigger");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    public override void HandleFiringAnimationEnd()
    {
        //no op
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour, IWeapon
{
    public GameObject rayCastStartPoint;
    public GameObject rayCastEndPoint;
    public Transform muzzleTipCheck;
    public LayerMask enemyLayer;
    public LayerMask environmentLayers;
    public LayerMask mixedLayerMask;
    public WeaponSO WeaponInfo { get; set;}
    [SerializeField] BulletSO bulletInfo;
    [SerializeField] Transform firingPoint;
    [SerializeField] Animator _animator;
    [SerializeField] RuntimeAnimatorController _runtimeAnimatorController;
    bool _isHoldingTrigger = false;


    void Awake(){
        if (WeaponInfo == null){
            //Debug.Log("Revolver weapon info missing. Loading resource.");
            WeaponInfo = Resources.Load<WeaponSO>("ScriptableObjects/RevolverSO");
        }
        //print(WeaponInfo);

        if(WeaponInfo.bulletInfo == null){
            //Debug.Log("Revolver bullet info (part of Revolver weapon info) missing. Loading Revolver bullet info resource.");
            bulletInfo = Resources.Load<BulletSO>("ScriptableObjects/RevolverBulletSO");
            //WeaponInfo.bulletInfo = bulletInfo;
        }
        else {
            //Debug.Log("Revolver bullet info (part of Revolver bullet info) already exists.");
            bulletInfo = WeaponInfo.bulletInfo;
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
        WeaponInfo.Init();
    }

    void Update(){

        if(!HasAmmo() && WeaponInfo.currentReserveAmmo != 0 && !WeaponInfo.isReloading){
            HandlePrimaryAttackInputCancel();
            HandleReloadStart();
        }
        else if (!HasAmmo() && WeaponInfo.currentReserveAmmo == 0 && !WeaponInfo.isReloading){
            HandlePrimaryAttackInputCancel();
        }
    }

    public void PrimaryAttack(){

       /* GameObject overlappingEnemy = ObstructionChecker.CheckMuzzleEnemyOverlap(muzzleTipCheck, enemyLayer);
        if(overlappingEnemy != null){
            IDamageable damageable = overlappingEnemy.GetComponent<IDamageable>();
            print(damageable);
            damageable?.TakeDamage(WeaponInfo.damage);
        }
        */
        Bullet instantiatedBullet = Instantiate(WeaponInfo.bulletPrefab, firingPoint.transform.position, transform.rotation).GetComponent<Bullet>();
        instantiatedBullet.SetupBulletParameters(bulletInfo.projectileSpeed, bulletInfo.size, WeaponInfo.damage, bulletInfo.lifeTime);

        IWeapon.Invoke();
        --WeaponInfo.currentAmmo;
        
    }

    public bool HasAmmo(){
        if (WeaponInfo.currentAmmo > 0){
            return true;
        }
        return false;
    }

    public void Reload()
    {
        int ammoBeforeReload = WeaponInfo.currentAmmo;
        WeaponInfo.currentAmmo = 0;

        if( WeaponInfo.roundCapacity - ammoBeforeReload <= WeaponInfo.currentReserveAmmo){
            WeaponInfo.currentReserveAmmo -= WeaponInfo.roundCapacity - ammoBeforeReload;
            WeaponInfo.currentAmmo = WeaponInfo.roundCapacity;
        }
        else {
            WeaponInfo.currentAmmo += WeaponInfo.currentReserveAmmo;
            WeaponInfo.currentReserveAmmo = 0;
        }
        WeaponInfo.isReloading = false;
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(muzzleTipCheck.transform.position, 0.5f);
        Debug.DrawLine(rayCastStartPoint.transform.position, rayCastEndPoint.transform.position, Color.green);
    }

    public void HandlePrimaryAttackInput()
    {
        if(ObstructionChecker.CheckWeaponObstructionOverlap(rayCastStartPoint.transform, rayCastEndPoint.transform, environmentLayers, enemyLayer)) return;  
        //if(ObstructionChecker.CheckMuzzleEnvironmentOverlap(muzzleTipCheck, environmentLayers)) return;
        if(!HasAmmo() || WeaponInfo.isReloading || _isHoldingTrigger) return;
        _isHoldingTrigger = true;

        //print(_animator.gameObject.activeSelf);
        //print(_animator.runtimeAnimatorController);
        if (_animator != null)
        {
                _animator.SetBool("isTriggerHeld", true);
        }
        //_animator.SetBool("isTriggerHeld", true);
    }

    public void HandlePrimaryAttackInputCancel(){
        WeaponInfo.isFiring = false;
        _isHoldingTrigger = false;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
    }

    public void HandleTriggerHoldEnd(){
        WeaponInfo.isFiring = true;
        _animator.SetTrigger("FireTrigger");
        _animator.SetBool("isFiring", true);
    }

    public void HandleFiringAnimationEnd()
    {
        _isHoldingTrigger = false;
        WeaponInfo.isFiring = false;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
        
    }

    public void HandleReloadStart()
    {
        if(WeaponInfo.currentReserveAmmo == 0 || WeaponInfo.currentAmmo == WeaponInfo.roundCapacity || WeaponInfo.isReloading) 
        return;
        
        if(WeaponInfo.isFiring) WeaponInfo.isFiring = false;
        
        WeaponInfo.isReloading = true;
        _animator.SetBool("isFiring", false);
        _animator.ResetTrigger("FireTrigger");
        _animator.SetTrigger("ReloadTrigger");
    }

    public void HandleReloadEnd()
    {
        Reload();
        
    }


    public void ReceiveFiringPoint(Transform firingPoint){
        this.firingPoint = firingPoint;
    }

    public void ResetWeaponState(){
        WeaponInfo.isFiring = false;
        WeaponInfo.isReloading = false;
        _isHoldingTrigger = false;
        _animator.SetBool("isTriggerHeld", false);
        _animator.SetBool("isFiring", false);
        _animator.SetBool("isLMBHeld", false);
        _animator.ResetTrigger("FireTrigger");
        //_animator.Play("Idle");
        _animator.SetTrigger("WeaponSwitchTrigger");
        GetComponent<SpriteRenderer>().sprite = WeaponInfo.sprite;
    }

}
